using MediatR;
using Project3.Application.Common.Interfaces;
using Project3.Domain.Common.Response;

namespace Project3.Application.Features.Queries;

public sealed record GetNotificationLogByIdQuery(Guid Id)
    : IRequest<Result<GetNotificationLogsQueryResponse>>;

public class GetNotificationLogByIdQueryHandler(INotificationLogsRepository _repo)
    : IRequestHandler<GetNotificationLogByIdQuery, Result<GetNotificationLogsQueryResponse>>
{
    public async Task<Result<GetNotificationLogsQueryResponse>> Handle(
        GetNotificationLogByIdQuery request,
        CancellationToken cancellationToken)
    {
        var log = await _repo.GetByIdAsync(request.Id);

        if (log is null)
            return Result<GetNotificationLogsQueryResponse>.Failure("Notification log not found.");

        var response = new GetNotificationLogsQueryResponse(
            Id: log.Id,
            AppointmentId: log.AppointmentId,
            Type: log.Type,
            Status: log.Status,
            FailureReason: log.FailureReason,
            CreatedAt: log.CreatedAt
        );

        return Result<GetNotificationLogsQueryResponse>.Success(response);
    }
}