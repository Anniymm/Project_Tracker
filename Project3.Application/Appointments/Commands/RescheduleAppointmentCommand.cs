using FluentValidation;
using MediatR;
using Project3.Application.Common.DTOs;
using Project3.Application.Common.Interfaces;
using Project3.Domain.Common.Response;
using Project3.Domain.Enums;

namespace Project3.Application.Appointments.Commands;

public sealed record RescheduleAppointmentCommand(RescheduleAppointmentDto Dto)
    : IRequest<Result>;

public sealed class RescheduleAppointmentCommandValidator
    : AbstractValidator<RescheduleAppointmentCommand>
{
    public RescheduleAppointmentCommandValidator()
    {
        RuleFor(x => x.Dto.AppointmentId)
            .NotEmpty()
            .WithMessage("Appointment Id is required");

        RuleFor(x => x.Dto.NewDate)
            .NotEmpty()
            .WithMessage("New date is required");

        RuleFor(x => x.Dto.NewStartTime)
            .NotEmpty()
            .WithMessage("Start time is required");

        RuleFor(x => x.Dto.NewEndTime)
            .GreaterThan(x => x.Dto.NewStartTime)
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
        var dto = request.Dto;

        var appointment = await _unitOfWork.Appointments.GetByIdAsync(dto.AppointmentId);

        if (appointment is null)
            return Result.Failure("Appointment not found.");

        if (appointment.Status == AppointmentStatus.completed)
            return Result.Failure("Completed appointments cannot be rescheduled.");

        if (appointment.Status == AppointmentStatus.cancelled)
            return Result.Failure("Cancelled appointments cannot be rescheduled.");

        if (appointment.Status == AppointmentStatus.no_show)
            return Result.Failure("No-show appointments cannot be rescheduled.");

        appointment.Update(
            appointmentDate: dto.NewDate,
            startTime: dto.NewStartTime,
            endTime: dto.NewEndTime,
            status: AppointmentStatus.scheduled,
            cancellationReason: null, 
            isRecurring: null,
            recurrenceRule: null
        );
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success("Appointment rescheduled successfully.");
    }
}
