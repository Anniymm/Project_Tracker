using FluentValidation;
using MediatR;
using Project3.Application.Common.Interfaces;
using Project3.Domain.Common.Response;

namespace Project3.Application.Features.Queries;

public sealed record GetBlockedTimesByProviderQuery(Guid ProviderId)
    : IRequest<Result<IEnumerable<GetBlockedTimesByProviderQueryResponse>>>;

public sealed record GetBlockedTimesByProviderQueryResponse(
    Guid Id,
    Guid ProviderId,
    DateTime StartTime,
    DateTime EndTime,
    string Reason,
    DateTime CreatedAt
);

public sealed class GetBlockedTimesByProviderQueryValidator
    : AbstractValidator<GetBlockedTimesByProviderQuery>
{
    public GetBlockedTimesByProviderQueryValidator()
    {
        RuleFor(x => x.ProviderId)
            .NotEmpty().WithMessage("ProviderId is required.");
    }
}

public sealed class GetBlockedTimesByProviderQueryHandler
    : IRequestHandler<GetBlockedTimesByProviderQuery, Result<IEnumerable<GetBlockedTimesByProviderQueryResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetBlockedTimesByProviderQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IEnumerable<GetBlockedTimesByProviderQueryResponse>>> Handle(
        GetBlockedTimesByProviderQuery request,
        CancellationToken cancellationToken)
    {
        var blockedTimes = await _unitOfWork.BlockedTimes.GetByProviderIdAsync(request.ProviderId);

        if (!blockedTimes.Any())
            return Result<IEnumerable<GetBlockedTimesByProviderQueryResponse>>
                .Failure("No blocked times found for this provider");

        var response = blockedTimes.Select(bt =>
            new GetBlockedTimesByProviderQueryResponse(
                Id: bt.Id,
                ProviderId: bt.ProviderId,
                StartTime: bt.StartDateTime,
                EndTime: bt.EndDateTime,
                Reason: bt.Reason,
                CreatedAt: bt.CreatedAt
            )
        );

        return Result<IEnumerable<GetBlockedTimesByProviderQueryResponse>>.Success(response);
    }
}
