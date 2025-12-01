using MediatR;
using Project3.Application.Common.Interfaces;
using Project3.Domain.Common.Response;
using Project3.Domain.Enums;

namespace Project3.Application.Features.Queries;

public sealed record GetAllNotificationLogsQuery()
    : IRequest<Result<IEnumerable<GetNotificationLogsQueryResponse>>>;

public sealed record GetNotificationLogsQueryResponse(
    Guid Id,
    Guid AppointmentId,
    NotificationType Type,
    DateTime SentAt,
    NotificationStatus Status
);

public class GetAllNotificationLogsQueryHandler(IUnitOfWork _unitOfWork)
    : IRequestHandler<GetAllNotificationLogsQuery, Result<IEnumerable<GetNotificationLogsQueryResponse>>>
{
    public async Task<Result<IEnumerable<GetNotificationLogsQueryResponse>>> Handle(
        GetAllNotificationLogsQuery request,
        CancellationToken cancellationToken)
    {
        var logs = await _unitOfWork.NotificationLogs.GetAllAsync();

        if (logs is null || !logs.Any())
            return Result<IEnumerable<GetNotificationLogsQueryResponse>>.Failure("No notification logs found");

        var response = logs.Select(
            l => new GetNotificationLogsQueryResponse(
            Id: l.Id,
            AppointmentId: l.AppointmentId,
            Type: l.Type,
            SentAt: l.SentAt,
            Status: l.Status
        )).ToList();

        return Result<IEnumerable<GetNotificationLogsQueryResponse>>.Success(response);
    }
}