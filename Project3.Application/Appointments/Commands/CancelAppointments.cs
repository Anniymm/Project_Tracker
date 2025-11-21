using System.ComponentModel.DataAnnotations;
using FluentValidation;
using FluentValidation.Validators;
using MediatR;
using Project3.Application.Common.Interfaces;
using Project3.Domain.Common.Response;
using Project3.Domain.Enums;

namespace Project3.Application.Appointments.Commands;

public sealed record CancelAppointmentCommand(
    Guid AppointmentId,
    string Reason) : IRequest<Result>;


public sealed class CancelAppointmentCommandValidator:
    AbstractValidator<CancelAppointmentCommand>
{
    public CancelAppointmentCommandValidator()
    {
        RuleFor(x => x.AppointmentId).NotEmpty()
            .WithMessage("Id is required.");

        RuleFor(x => x.Reason).NotEmpty()
            .WithMessage("Reason is required.")
            .MaximumLength(250);
    }
}


public sealed class CancelAppointmentHandler
    : IRequestHandler<CancelAppointmentCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public CancelAppointmentHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = await _unitOfWork.Appointments.GetByIdAsync(request.AppointmentId);

        if (appointment is null)
            return Result.Failure("Appointment not found.");

        
        if (appointment.Status == AppointmentStatus.completed)
            return Result.Failure("Completed appointments cannot be cancelled.");

        
        if (appointment.Status == AppointmentStatus.no_show)
            return Result.Failure("A no-show appointment cannot be cancelled.");

        
        appointment.Update(
            appointmentDate: null,
            startTime: null,
            endTime: null,
            status: AppointmentStatus.cancelled,
            cancellationReason: request.Reason,
            isRecurring: null,
            recurrenceRule: null
        );

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Appointment cancelled successfully.");
    }
}

    