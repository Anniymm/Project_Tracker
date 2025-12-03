using MediatR;
using Project3.Application.Common.Interfaces;
using Project3.Domain.Common.Response;

namespace Project3.Application.Features.Queries;

public sealed record GetNotificationLogsByAppointmentQuery(Guid AppointmentId)
    : IRequest<Result<List<GetNotificationLogsQueryResponse>>>;

public class GetNotificationLogsByAppointmentQueryHandler
    (INotificationLogsRepository _repo)
    : IRequestHandler<GetNotificationLogsByAppointmentQuery, Result<List<GetNotificationLogsQueryResponse>>>
{
    public async Task<Result<List<GetNotificationLogsQueryResponse>>> Handle(
        GetNotificationLogsByAppointmentQuery request,
        CancellationToken cancellationToken)
    {
        var logs = await _repo.GetByAppointmentIdAsync(request.AppointmentId);

        if (logs is null || !logs.Any())
            return Result<List<GetNotificationLogsQueryResponse>>.Failure(
                "No notification logs found for this appointment");

        var response = logs
            .Select(log => new GetNotificationLogsQueryResponse(
                Id: log.Id,
                AppointmentId: log.AppointmentId,
                Type: log.Type,
                Status: log.Status,
                FailureReason: log.FailureReason,
                CreatedAt: log.CreatedAt
            ))
            .ToList();

        return Result<List<GetNotificationLogsQueryResponse>>.Success(response);
    }
}