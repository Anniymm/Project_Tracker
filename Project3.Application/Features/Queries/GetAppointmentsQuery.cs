using AutoMapper;
using FluentValidation;
using MediatR;
using Project3.Application.Common.DTOs;
using Project3.Application.Common.Interfaces;
using Project3.Domain.Common.Response;

namespace Project3.Application.Features.Queries;

public sealed record GetAppointmentsQuery(Guid ProviderId)
    : IRequest<Result<List<GetAppointmentDto>>>;


public sealed class GetAppointmentsQueryValidator 
    : AbstractValidator<GetAppointmentsQuery>
{
    public GetAppointmentsQueryValidator()
    {
        RuleFor(x => x.ProviderId)
            .NotEmpty().WithMessage("ProviderId is required.");
    }
}


public sealed class GetAppointmentsQueryHandler
    : IRequestHandler<GetAppointmentsQuery, Result<List<GetAppointmentDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAppointmentsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<List<GetAppointmentDto>>> Handle(
        GetAppointmentsQuery request,
        CancellationToken cancellationToken)
    {
        var provider = await _unitOfWork.Appointments.GetByProviderAsync(request.ProviderId);
        if (provider is null)
            return Result<List<GetAppointmentDto>>.Failure("Provider not found.");

        var appointments = await _unitOfWork.Appointments.GetByProviderAsync(request.ProviderId);

        var appointmentDtos = _mapper.Map<List<GetAppointmentDto>>(appointments);

        return Result<List<GetAppointmentDto>>.Success(
            appointmentDtos,
            "Appointments got successfully."
        );
    }
}