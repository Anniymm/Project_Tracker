using AutoMapper;
using FluentValidation;
using MediatR;
using Project3.Application.Common.DTOs;
using Project3.Application.Common.Interfaces;
using Project3.Domain.Common.Response;

namespace Project3.Application.Features.Queries;

public sealed record GetWorkingHoursQuery(Guid ProviderId)
    : IRequest<Result<List<GetWorkingHoursDto>>>;

public sealed class GetWorkingHoursQueryValidator 
    : AbstractValidator<GetWorkingHoursQuery>
{
    public GetWorkingHoursQueryValidator()
    {
        RuleFor(x => x.ProviderId)
            .NotEmpty()
            .WithMessage("Provider id cannot be empty");
    }
}

public sealed class GetWorkingHoursQueryHandler
    : IRequestHandler<GetWorkingHoursQuery, Result<List<GetWorkingHoursDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetWorkingHoursQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<List<GetWorkingHoursDto>>> Handle(
        GetWorkingHoursQuery request,
        CancellationToken cancellationToken)
    {
        var provider = await _unitOfWork.ServiceProviders
            .GetByIdAsync(request.ProviderId);

        if (provider is null)
            return Result<List<GetWorkingHoursDto>>.Failure("Provider not found");

        var workingHours = await _unitOfWork.WorkingHours
            .GetAllByProviderIdAsync(request.ProviderId);

        var workingHourDtos = _mapper.Map<List<GetWorkingHoursDto>>(workingHours);

        return Result<List<GetWorkingHoursDto>>.Success(
            workingHourDtos,
            "Working hours retrieved successfully");
    }
}