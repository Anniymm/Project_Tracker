using MediatR;
using Project3.Application.Common.Interfaces;
using Project3.Domain.Common.Response;
using Project3.Domain.Entities;
using Project3.Domain.Enums;

namespace Project3.Application.Features.Queries;

public sealed record GetAppointmentByIdQuery(Guid Id) :
    IRequest<Result<GetAppointmentQueryResponse>>;

public sealed record GetAppointmentQueryResponse(
    Guid id,
    Guid ProviderId,
    ServiceProvider ServiceProvider,
    string CustomerName,
    string CustomerEmail,
    DateTime StartDate,
    TimeOnly StartTime,
    TimeOnly EndTime,
    AppointmentStatus Status,
    string? CancellationReason,
    bool IsRecurring,
    string? RecurrenceRule,
    Guid? ParentAppointmentId,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public class GetAppointmentByIdQueryHandler(IUnitOfWork _unitOfWork) :  IRequestHandler<GetAppointmentByIdQuery, Result<GetAppointmentQueryResponse>>
{
    public async Task<Result<GetAppointmentQueryResponse>> Handle(GetAppointmentByIdQuery request, CancellationToken cancellationToken)
    {
        var appointment = await _unitOfWork.Appointments.GetByIdAsync(request.Id);

        if (appointment is null)
            return Result<GetAppointmentQueryResponse>.Failure("Can't find appointment");

        var response = new GetAppointmentQueryResponse(
            id: appointment.Id,
            ProviderId: appointment.ProviderId,
            ServiceProvider: appointment.ServiceProvider,
            CustomerName: appointment.CustomerName,
            CustomerEmail: appointment.CustomerEmail,
            StartDate: appointment.AppointmentDate.ToDateTime(TimeOnly.MinValue), // convert DateOnly to DateTime
            StartTime: appointment.StartTime,
            EndTime: appointment.EndTime,
            Status: appointment.Status,
            CancellationReason: appointment.CancellationReason,
            IsRecurring: appointment.IsRecurring,
            RecurrenceRule: appointment.RecurrenceRule,
            ParentAppointmentId: appointment.ParentAppointmentId,
            CreatedAt: appointment.CreatedAt,
            UpdatedAt: appointment.UpdatedAt
            );

        return Result<GetAppointmentQueryResponse>.Success(response);
    }
}