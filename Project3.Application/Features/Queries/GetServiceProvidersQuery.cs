using AutoMapper;
using FluentValidation;
using MediatR;
using Project3.Application.Common.DTOs;
using Project3.Application.Common.Interfaces;
using Project3.Domain.Common.Response;

namespace Project3.Application.Features.Queries;


public sealed record GetServiceProvidersQuery()
    : IRequest<Result<List<GetServiceProvidersDto>>>;


public sealed class GetServiceProvidersQueryHandler
    : IRequestHandler<GetServiceProvidersQuery, Result<List<GetServiceProvidersDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetServiceProvidersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<List<GetServiceProvidersDto>>> Handle(
        GetServiceProvidersQuery request,
        CancellationToken cancellationToken)
    {
        var providers = await _unitOfWork.ServiceProviders.GetAllAsync();

        if (providers.Count == 0)
            return Result<List<GetServiceProvidersDto>>
                .Success(new List<GetServiceProvidersDto>(), "No providers found");

        var dtos = _mapper.Map<List<GetServiceProvidersDto>>(providers);

        return Result<List<GetServiceProvidersDto>>
            .Success(dtos, "Service providers retrieved successfully");
    }
}