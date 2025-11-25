using AutoMapper;
using FluentValidation;
using MediatR;
using Project3.Application.Common.DTOs;
using Project3.Application.Common.Interfaces;
using Project3.Domain.Common.Response;

namespace Project3.Application.Appointments.Queries;

public sealed record GetServiceProvidersQuery()
    : IRequest<Result<List<GetServiceProviderDto>>>;

public class GetServiceProvidersQueryValidator :
    AbstractValidator<GetServiceProvidersQuery>
{
    public GetServiceProvidersQueryValidator()
    {
        
    }
}

public sealed class GetServiceProvidersQueryHandler
    : IRequestHandler<GetServiceProvidersQuery, Result<List<GetServiceProviderDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public Task<Result<List<GetServiceProviderDto>>> Handle(GetServiceProvidersQuery request, CancellationToken cancellationToken)
    {
        //RuleFor da amis implementation
        throw new NotImplementedException();
    }
}