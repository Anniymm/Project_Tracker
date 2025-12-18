using FluentValidation;
using MediatR;
using Project3.Application.Common.DTOs;
using Project3.Application.Common.Interfaces;
using Project3.Domain.Common.Response;

namespace Project3.Application.Features.Commands;


public sealed record UpdateServiceProviderCommand(    Guid Id,
    string? Name,
    string? Email,
    string? Specialty,
    bool? IsActive)
    : IRequest<Result>;

public sealed class UpdateServiceProviderCommandValidator
    : AbstractValidator<UpdateServiceProviderCommand>
{
    public UpdateServiceProviderCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Service provider Id is required.");

        When(x => x.Name is not null, () =>
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name cannot be empty");
        });

        When(x => x.Email is not null, () =>
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("Email must be a valid");
        });

        When(x => x.Specialty is not null, () =>
        {
            RuleFor(x => x.Specialty)
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
        var provider = await _unitOfWork.ServiceProviders.GetByIdAsync(request.Id);

        if (provider is null)
            return Result.Failure("Service provider not found.");

        provider.Update(
            name: request.Name,
            email: request.Email,
            specialty: request.Specialty,
            isActive: request.IsActive
        );

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Service provider updated successfully.");
    }
}
