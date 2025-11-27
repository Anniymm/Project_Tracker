using MediatR;
using Project3.Application.Common.Interfaces;
using Project3.Domain.Common.Response;

namespace Project3.Application.Features.Queries;

public sealed record GetServiceProviderByIdQuery(Guid Id) 
    : IRequest<Result<GetProvidersQueryResponse>>;

public sealed record GetProvidersQueryResponse(
    Guid Id,
    string Name,
    string Email,
    string Specialty,
    bool IsActive,
    DateTime CreatedAt);

public class GetServiceProviderByIdQueryHandler(IUnitOfWork _unitOfWork) 
    : IRequestHandler<GetServiceProviderByIdQuery, Result<GetProvidersQueryResponse>>
{
    public async Task<Result<GetProvidersQueryResponse>> Handle(GetServiceProviderByIdQuery request, CancellationToken cancellationToken)
    {
        var provider = await _unitOfWork.ServiceProviders.GetByIdAsync(request.Id);

        if (provider is null)
            return Result<GetProvidersQueryResponse>.Failure("Can't find provider");

        var response = new GetProvidersQueryResponse(
            Id: provider.Id,
            Name: provider.Name,
            Email: provider.Email,
            Specialty: provider.Specialty,
            IsActive: provider.IsActive,
            CreatedAt: provider.CreatedAt
        );

        return Result<GetProvidersQueryResponse>.Success(response);
    }
}