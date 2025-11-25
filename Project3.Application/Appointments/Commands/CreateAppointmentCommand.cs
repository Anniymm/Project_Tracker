using FluentValidation;
using MediatR;
using Project3.Application.Common.Interfaces;
using Project3.Domain.Common.Response;
using Project3.Domain.Entities;
using Project3.Domain.Enums;

namespace Project3.Application.Appointments.Commands;

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

    public async Task<Result> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = new Appointment(
            id: Guid.NewGuid(),
            providerId: request.ProviderId,
            customerName: request.CustomerName,
            customerEmail: request.CustomerEmail,
            customerPhone: request.CustomerPhone,
            appointmentDate: request.AppointmentDate,
            startTime: request.StartTime,
            endTime: request.EndTime,

            status: AppointmentStatus.no_show,   // axal appointmentebs defaultad rom qondes
            cancellationReason: null,

            isRecurring: request.IsRecurring,
            recurrenceRule: request.RecurrenceRule,

            parentAppointmentId: null,             

            createdAt: DateTime.UtcNow,
            updatedAt: DateTime.UtcNow
        );


        await _unitOfWork.Appointments.AddAsync(appointment);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Appointment created successfully");
    }
}
