using FluentValidation;
using MediatR;
using Project3.Application.Common.DTOs;
using Project3.Application.Common.Interfaces;
using Project3.Domain.Common.Response;

namespace Project3.Application.Features.Commands;


public sealed record UpdateServiceProviderCommand(UpdateServiceProviderDto Dto)
    : IRequest<Result>;

public sealed class UpdateServiceProviderCommandValidator
    : AbstractValidator<UpdateServiceProviderCommand>
{
    public UpdateServiceProviderCommandValidator()
    {
        RuleFor(x => x.Dto.Id)
            .NotEmpty()
            .WithMessage("Service provider Id is required.");

        When(x => x.Dto.Name is not null, () =>
        {
            RuleFor(x => x.Dto.Name)
                .NotEmpty()
                .WithMessage("Name cannot be empty");
        });

        When(x => x.Dto.Email is not null, () =>
        {
            RuleFor(x => x.Dto.Email)
                .EmailAddress()
                .WithMessage("Email must be a valid");
        });

        When(x => x.Dto.Specialty is not null, () =>
        {
            RuleFor(x => x.Dto.Specialty)
                .NotEmpty()
                .WithMessage("Specialty cannot be empty");
        });
    }
}


public sealed class UpdateServiceProviderCommandHandler
    : IRequestHandler<UpdateServiceProviderCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateServiceProviderCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        UpdateServiceProviderCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.Dto;

        var provider = await _unitOfWork.ServiceProviders.GetByIdAsync(dto.Id);

        if (provider is null)
            return Result.Failure("Service provider not found.");

        provider.Update(
            name: dto.Name,
            email: dto.Email,
            specialty: dto.Specialty,
            isActive: dto.IsActive
        );

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Service provider updated successfully.");
    }
}
