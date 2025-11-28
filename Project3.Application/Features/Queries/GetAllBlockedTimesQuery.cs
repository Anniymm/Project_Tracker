using MediatR;
using Project3.Application.Common.Interfaces;
using Project3.Domain.Common.Response;

namespace Project3.Application.Features.Queries;

public sealed record GetAllBlockedTimesQuery()
    : IRequest<Result<IEnumerable<GetAllBlockedTimesQueryResponse>>>;

public sealed record GetAllBlockedTimesQueryResponse(
    Guid Id,
    Guid ProviderId,
    DateTime StartTime,
    DateTime EndTime,
    string Reason,
    DateTime CreatedAt
);

public sealed class GetAllBlockedTimesQueryHandler
    : IRequestHandler<GetAllBlockedTimesQuery, Result<IEnumerable<GetAllBlockedTimesQueryResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllBlockedTimesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IEnumerable<GetAllBlockedTimesQueryResponse>>> Handle(
        GetAllBlockedTimesQuery request,
        CancellationToken cancellationToken)
    {
        var blockedTimes = await _unitOfWork.BlockedTimes.GetAllAsync();

        if (!blockedTimes.Any())
            return Result<IEnumerable<GetAllBlockedTimesQueryResponse>>
                .Failure("No blocked times found");

        var response = blockedTimes.Select(bt =>
            new GetAllBlockedTimesQueryResponse(
                Id: bt.Id,
                ProviderId: bt.ProviderId,
                StartTime: bt.StartDateTime,
                EndTime: bt.EndDateTime,
                Reason: bt.Reason,
                CreatedAt: bt.CreatedAt
            )
        );

        return Result<IEnumerable<GetAllBlockedTimesQueryResponse>>.Success(response);
    }
}