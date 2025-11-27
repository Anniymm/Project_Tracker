using MediatR;
using Project3.Application.Common.Interfaces;
using Project3.Domain.Common.Response;

namespace Project3.Application.Features.Queries;

public sealed record GetAppointmentsByProviderQuery(Guid ProviderId) 
    : IRequest<Result<List<GetAppointmentQueryResponse>>>;

public class GetAppointmentsByProviderQueryHandler (IUnitOfWork _unitOfWork)
    : IRequestHandler<GetAppointmentsByProviderQuery, Result<List<GetAppointmentQueryResponse>>>
{
    public async Task<Result<List<GetAppointmentQueryResponse>>> Handle(GetAppointmentsByProviderQuery request, CancellationToken cancellationToken)
    {
        var appointments = await _unitOfWork.Appointments.GetByProviderAsync(request.ProviderId);

        if (appointments is null || !appointments.Any())
            return Result<List<GetAppointmentQueryResponse>>.Failure("No appointments found for this provider.");
        
        // autommaper ar maqvs aq //
        var response = appointments.Select(a => new GetAppointmentQueryResponse(
            id: a.Id,
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


        return Result<List<GetAppointmentQueryResponse>>.Success(response);
    }
}
