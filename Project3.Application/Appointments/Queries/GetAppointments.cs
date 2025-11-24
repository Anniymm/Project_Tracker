using AutoMapper;
using FluentValidation;
using MediatR;
using Project3.Application.Common.DTOs;
using Project3.Application.Common.Interfaces;
using Project3.Domain.Common.Response;

namespace Project3.Application.Appointments.Queries;

public sealed record GetAppointmentsQuery(Guid ProviderId)
    : IRequest<Result<List<AppointmentDto>>>;


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
    : IRequestHandler<GetAppointmentsQuery, Result<List<AppointmentDto>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetAppointmentsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<List<AppointmentDto>>> Handle(
        GetAppointmentsQuery request,
        CancellationToken cancellationToken)
    {
        var provider = await _unitOfWork.Appointments.GetByProviderAsync(request.ProviderId);
        if (provider is null)
            return Result<List<AppointmentDto>>.Failure("Provider not found.");

        var appointments = await _unitOfWork.Appointments.GetByProviderAsync(request.ProviderId);

        var appointmentDtos = _mapper.Map<List<AppointmentDto>>(appointments);

        return Result<List<AppointmentDto>>.Success(
            appointmentDtos,
            "Appointments got successfully."
        );
    }
}