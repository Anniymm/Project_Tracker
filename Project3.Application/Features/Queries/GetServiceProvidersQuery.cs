using MediatR;
using Project3.Application.Common.Interfaces;
using Project3.Domain.Common.Response;

namespace Project3.Application.Features.Queries;

public sealed record GetServiceProvidersQuery()
    : IRequest<Result<List<ServiceProviderResponse>>>;

public sealed record ServiceProviderResponse(
    Guid Id,
    string Name,
    string Email,
    string Specialty,
    bool IsActive,
    DateTime CreatedAt
);

public sealed class GetServiceProvidersQueryHandler
    : IRequestHandler<GetServiceProvidersQuery, Result<List<ServiceProviderResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetServiceProvidersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<ServiceProviderResponse>>> Handle(
        GetServiceProvidersQuery request,
        CancellationToken cancellationToken)
    {
        var providers = await _unitOfWork.ServiceProviders.GetAllAsync();

        if (providers.Count == 0)
            return Result<List<ServiceProviderResponse>>
                .Success(new List<ServiceProviderResponse>(), "No providers found");

        var response = providers.Select(p => new ServiceProviderResponse(
            p.Id,
            p.Name,
            p.Email,
            p.Specialty,
            p.IsActive,
            p.CreatedAt
        )).ToList();

        return Result<List<ServiceProviderResponse>>
            .Success(response, "Service providers retrieved successfully");
    }
}