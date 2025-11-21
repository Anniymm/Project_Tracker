using FluentValidation;
using MediatR;
using Project3.Application.Common.Interfaces;
using Project3.Domain.Common.Response;
using Project3.Domain.Enums;

namespace Project3.Application.Appointments.Commands;

public sealed record RescheduleAppointmentCommand(
    Guid AppointmentId,
    DateOnly NewDate,
    TimeOnly NewStartTime,
    TimeOnly NewEndTime ) : IRequest<Result>;


public sealed class RescheduleAppointmentCommandValidator
    : AbstractValidator<RescheduleAppointmentCommand>
{
    public RescheduleAppointmentCommandValidator()
    {
        RuleFor(x => x.AppointmentId).NotEmpty()
            .WithMessage("Appointment Id is required");

        RuleFor(x => x.NewDate).NotEmpty();
        
        RuleFor(x => x.NewStartTime).NotEmpty();

        RuleFor(x => x.NewEndTime)
            .GreaterThan(x => x.NewStartTime)
            .WithMessage("End time must be greater than start time");
    }
}


public sealed class RescheduleAppointmentHandler
    : IRequestHandler<RescheduleAppointmentCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    
    public RescheduleAppointmentHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        RescheduleAppointmentCommand request, 
        CancellationToken cancellationToken)
    {
        var appointment = await _unitOfWork.Appointments.GetByIdAsync(request.AppointmentId);

        if (appointment is null)
            return Result.Failure("Appointment not found.");

        if (appointment.Status == AppointmentStatus.completed)
            return Result.Failure("Completed appointments cannot be rescheduled.");

        if (appointment.Status == AppointmentStatus.cancelled)
            return Result.Failure("Cancelled appointments cannot be rescheduled.");

        if (appointment.Status == AppointmentStatus.no_show)
            return Result.Failure("No-show appointments cannot be rescheduled.");
        
        // if (DateTime.UtcNow > appointment.AppointmentDate.ToDateTime(appointment.StartTime))
        //     return Result.Failure("You cannot reschedule past appointments.");

        appointment.Update(
            appointmentDate: request.NewDate,
            startTime: request.NewStartTime,
            endTime: request.NewEndTime,
            status: AppointmentStatus.scheduled, // es sheduled unda darches
            cancellationReason: null, 
            isRecurring: null,
            recurrenceRule: null
        );
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success("Appointment rescheduled successfully.");
    }
    
    
}
        
