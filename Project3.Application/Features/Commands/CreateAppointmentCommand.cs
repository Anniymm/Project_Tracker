using FluentValidation;
using MediatR;
using Project3.Application.Common.Interfaces;
using Project3.Domain.Common.Response;
using Project3.Domain.Entities;
using Project3.Domain.Enums;

namespace Project3.Application.Features.Commands;

public sealed record CreateAppointmentCommand(
    Guid ProviderId,
    string CustomerName,
    string CustomerEmail,
    string CustomerPhone,
    DateOnly AppointmentDate,
    TimeOnly StartTime,
    TimeOnly EndTime,
    bool IsRecurring,
    string? RecurrenceRule
) : IRequest<Result>;


public sealed class CreateAppointmentCommandValidator 
    : AbstractValidator<CreateAppointmentCommand>
{
    public CreateAppointmentCommandValidator()
    {
        RuleFor(x => x.ProviderId)
            .NotEmpty().WithMessage("ProviderId is required.");

        RuleFor(x => x.CustomerName)
            .NotEmpty().WithMessage("Customer name is required.")
            .MaximumLength(100);

        RuleFor(x => x.CustomerEmail)
            .NotEmpty().EmailAddress()
            .MaximumLength(100);

        RuleFor(x => x.CustomerPhone)
            .NotEmpty().MaximumLength(100);

        RuleFor(x => x.AppointmentDate)
            .NotEmpty();

        RuleFor(x => x.StartTime)
            .NotEmpty();

        RuleFor(x => x.EndTime)
            .GreaterThan(x => x.StartTime)
            .WithMessage("End time must be later than start time.");
    }
}


public sealed class CreateAppointmentHandler
    : IRequestHandler<CreateAppointmentCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateAppointmentHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        CreateAppointmentCommand request,
        CancellationToken cancellationToken)
    {
        // provideris arseboba
        var provider = await _unitOfWork.ServiceProviders.GetByIdAsync(request.ProviderId);
        if (provider == null)
            return Error.NotFound("Provider.NotFound", $"Service provider with ID {request.ProviderId} not found.");

        if (!provider.IsActive)
            return Error.Conflict("Provider.Inactive", "Cannot create appointment for inactive service provider.");

        // 2. drois shemowmeba working hoursebshi
        var dayOfWeek = (int)request.AppointmentDate.DayOfWeek;
        var workingHours = await _unitOfWork.WorkingHours.GetAllByProviderAndDayAsync(request.ProviderId, dayOfWeek);

        var activeWorkingHours = workingHours.Where(wh => wh.IsActive).ToList();
        
        if (!activeWorkingHours.Any())
            return Error.Conflict(
                "WorkingHours.NotAvailable", 
                $"Provider does not work on {request.AppointmentDate.DayOfWeek}.");

        bool isWithinWorkingHours = activeWorkingHours.Any(wh =>
            request.StartTime >= wh.StartTime &&
            request.EndTime <= wh.EndTime);

        if (!isWithinWorkingHours)
        {
            var workingHoursDisplay = string.Join(", ", 
                activeWorkingHours.Select(wh => $"{wh.StartTime:hh\\:mm}-{wh.EndTime:hh\\:mm}"));
            
            return Error.Conflict(
                "WorkingHours.OutsideSchedule",
                $"Requested time is outside provider's working hours. Provider works: {workingHoursDisplay}");
        }

        // available aris tu ara magi shemowmeba
        var appointmentStartDateTime = new DateTime(
            request.AppointmentDate.Year,
            request.AppointmentDate.Month,
            request.AppointmentDate.Day,
            request.StartTime.Hour,
            request.StartTime.Minute,
            request.StartTime.Second,
            DateTimeKind.Utc);

        var appointmentEndDateTime = new DateTime(
            request.AppointmentDate.Year,
            request.AppointmentDate.Month,
            request.AppointmentDate.Day,
            request.EndTime.Hour,
            request.EndTime.Minute,
            request.EndTime.Second,
            DateTimeKind.Utc);

        var blockedTimes = await _unitOfWork.BlockedTimes.GetByProviderIdAsync(request.ProviderId);

        // appointments gadafarva xo ar aqvs sxva blocked time-tan
        var blockingTime = blockedTimes.FirstOrDefault(bt =>
            (appointmentStartDateTime >= bt.StartDateTime && appointmentStartDateTime < bt.EndDateTime) ||
            (appointmentEndDateTime > bt.StartDateTime && appointmentEndDateTime <= bt.EndDateTime) ||
            (appointmentStartDateTime <= bt.StartDateTime && appointmentEndDateTime >= bt.EndDateTime));

        if (blockingTime != null)
            return Error.Conflict(
                "TimeSlot.Blocked",
                $"This time slot is blocked. Reason: {blockingTime.Reason}");

        // appointmentebi confliqturad rom ar iyos
        var existingAppointments = await _unitOfWork.Appointments.GetByProviderAsync(request.ProviderId);

        var conflictingAppointment = existingAppointments.FirstOrDefault(apt =>
            apt.AppointmentDate == request.AppointmentDate &&
            apt.Status != AppointmentStatus.cancelled &&
            apt.Status != AppointmentStatus.no_show &&
            (
                (request.StartTime >= apt.StartTime && request.StartTime < apt.EndTime) ||
                (request.EndTime > apt.StartTime && request.EndTime <= apt.EndTime) ||
                (request.StartTime <= apt.StartTime && request.EndTime >= apt.EndTime)
            ));
        // wesit 409
        if (conflictingAppointment != null)
            return Error.Conflict(
                "TimeSlot.Conflict",
                $"This time slot conflicts with an existing appointment at {conflictingAppointment.StartTime:hh\\:mm}-{conflictingAppointment.EndTime:hh\\:mm}.");
        
        var appointment = new Appointment(
            id: Guid.NewGuid(),
            providerId: request.ProviderId,
            customerName: request.CustomerName,
            customerEmail: request.CustomerEmail,
            customerPhone: request.CustomerPhone,
            appointmentDate: request.AppointmentDate,
            startTime: request.StartTime,
            endTime: request.EndTime,
            status: AppointmentStatus.scheduled,
            cancellationReason: null,
            isRecurring: request.IsRecurring,
            recurrenceRule: request.RecurrenceRule,
            parentAppointmentId: null,
            createdAt: DateTime.UtcNow,
            updatedAt: DateTime.UtcNow
        );

        await _unitOfWork.Appointments.AddAsync(appointment);

        // gmailic rom gagzavnos 
        // TODO: checking unda amas
        var confirmationEmail = new EmailQueue(
            id: Guid.NewGuid(),
            appointmentId: appointment.Id,
            toEmail: appointment.CustomerEmail,
            notificationType: EmailNotificationType.Confirmation,
            scheduledAt: DateTimeOffset.UtcNow
        );
        await _unitOfWork.EmailQueues.AddAsync(confirmationEmail);

        // shesaxsenebeli gmail
        var reminderTime = appointmentStartDateTime.AddHours(-24);

        if (reminderTime > DateTime.UtcNow)
        {
            var reminderEmail = new EmailQueue(
                id: Guid.NewGuid(),
                appointmentId: appointment.Id,
                toEmail: appointment.CustomerEmail,
                notificationType: EmailNotificationType.Reminder,
                scheduledAt: new DateTimeOffset(reminderTime, TimeSpan.Zero)
            );
            await _unitOfWork.EmailQueues.AddAsync(reminderEmail);
        }
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Appointment created successfully");
    }
}