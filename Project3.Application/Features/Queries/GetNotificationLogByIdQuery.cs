using MediatR;
using Project3.Application.Common.Interfaces;
using Project3.Domain.Common.Response;
using Project3.Domain.Entities;
using Project3.Domain.Enums;

namespace Project3.Application.Features.Queries;

public sealed record GetNotificationLogByIdQuery(Guid Id)
    : IRequest<Result<GetNotificationLogsQueryResponse>>;

public class GetNotificationLogByIdQueryHandler(IUnitOfWork _unitOfWork)
    : IRequestHandler<GetNotificationLogByIdQuery, Result<GetNotificationLogsQueryResponse>>
{
    public async Task<Result<GetNotificationLogsQueryResponse>> Handle(
        GetNotificationLogByIdQuery request,
        CancellationToken cancellationToken)
    {
        var log = await _unitOfWork.NotificationLogs.GetByIdAsync(request.Id);

        if (log is null)
            return Result<GetNotificationLogsQueryResponse>.Failure("Notification log not found.");

        var response = new GetNotificationLogsQueryResponse(
            Id: log.Id,
            AppointmentId: log.AppointmentId,
            Type: log.Type,
            SentAt: log.SentAt,
            Status: log.Status
        );

        return Result<GetNotificationLogsQueryResponse>.Success(response);
    }
}