using MediatR;
using Project3.Application.Common.Interfaces;
using Project3.Domain.Common.Response;

namespace Project3.Application.Features.Queries;

public sealed record GetBlockedTimeByIdQuery(Guid Id)
    : IRequest<Result<GetBlockedTimeQueryResponse>>;

public sealed record GetBlockedTimeQueryResponse(
    Guid Id,
    Guid ProviderId,
    DateTime StartDateTime,
    DateTime EndDateTime,
    string Reason,
    DateTime CreatedAt
);

public class GetBlockedTimeByIdQueryHandler(IUnitOfWork _unitOfWork)
    : IRequestHandler<GetBlockedTimeByIdQuery, Result<GetBlockedTimeQueryResponse>>
{
    public async Task<Result<GetBlockedTimeQueryResponse>> Handle(
        GetBlockedTimeByIdQuery request,
        CancellationToken cancellationToken)
    {
        var blockedTime = await _unitOfWork.BlockedTimes.GetByIdAsync(request.Id);

        if (blockedTime is null)
            return Result<GetBlockedTimeQueryResponse>.Failure("Blocked time not found");

        var response = new GetBlockedTimeQueryResponse(
            Id: blockedTime.Id,
            ProviderId: blockedTime.ProviderId,
            StartDateTime: blockedTime.StartDateTime,
            EndDateTime: blockedTime.EndDateTime,
            Reason: blockedTime.Reason,
            CreatedAt: blockedTime.CreatedAt
        );

        return Result<GetBlockedTimeQueryResponse>.Success(response);
    }
}