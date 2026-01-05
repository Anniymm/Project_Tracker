using MediatR;
using Project3.Application.Common.Interfaces;
using Project3.Domain.Common.Response;
using Project3.Domain.Entities;
using Project3.Domain.Enums;

namespace Project3.Application.Features.Queries;

public sealed record GetAllAppointmentsQuery() : IRequest<Result<List<GetAppointmentQueryResponsed>>>;

public sealed record GetAppointmentQueryResponsed(
    Guid Id,
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

public class GetAllAppointmentsQueryHandler(IUnitOfWork _unitOfWork) 
    : IRequestHandler<GetAllAppointmentsQuery, Result<List<GetAppointmentQueryResponsed>>>
{
    public async Task<Result<List<GetAppointmentQueryResponsed>>> Handle(
        GetAllAppointmentsQuery request,
        CancellationToken cancellationToken)
    {
        var appointments = await _unitOfWork.Appointments.GetAllAsync();

        if (appointments is [] || !appointments.Any())
            return Result<List<GetAppointmentQueryResponsed>>.Failure("No appointments found.");

        var response = appointments.Select(a => new GetAppointmentQueryResponsed(
            Id: a.Id,
            ProviderId: a.ProviderId,
            ServiceProvider: a.ServiceProvider,
            CustomerName: a.CustomerName,
            CustomerEmail: a.CustomerEmail,
            StartDate: a.AppointmentDate.ToDateTime(TimeOnly.MinValue),
            StartTime: a.StartTime,
            EndTime: a.EndTime,
            Status: a.Status,
            CancellationReason: a.CancellationReason,
            IsRecurring: a.IsRecurring,
            RecurrenceRule: a.RecurrenceRule,
            ParentAppointmentId: a.ParentAppointmentId,
            CreatedAt: a.CreatedAt,
            UpdatedAt: a.UpdatedAt
        )).ToList();

        return Result<List<GetAppointmentQueryResponsed>>.Success(response);
    }
}
