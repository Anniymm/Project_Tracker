using AutoMapper;
using FluentValidation;
using MediatR;
using Project3.Application.Common.DTOs;
using Project3.Application.Common.Interfaces;
using Project3.Domain.Common.Response;

namespace Project3.Application.Appointments.Queries;


public sealed record GetServiceProvidersQuery()
    : IRequest<Result<List<GetServiceProviderDto>>>;


public sealed class GetServiceProvidersQueryValidator
    : AbstractValidator<GetServiceProvidersQuery>
{
  // parametrebi ar maqvs da es unda shevamowmebino // ??????????
}

public sealed class GetServiceProvidersQueryHandler
    : IRequestHandler<GetServiceProvidersQuery, Result<List<GetServiceProviderDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetServiceProvidersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<List<GetServiceProviderDto>>> Handle(
        GetServiceProvidersQuery request,
        CancellationToken cancellationToken)
    {
        var providers = await _unitOfWork.ServiceProviders.GetAllAsync();

        if (providers.Count == 0)
            return Result<List<GetServiceProviderDto>>
                .Success(new List<GetServiceProviderDto>(), "No providers found");

        var dtos = _mapper.Map<List<GetServiceProviderDto>>(providers);

        return Result<List<GetServiceProviderDto>>
            .Success(dtos, "Service providers retrieved successfully");
    }
}