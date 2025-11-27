using FluentValidation;
using MediatR;
using Project3.Application.Common.DTOs;
using Project3.Application.Common.Interfaces;
using Project3.Domain.Common.Response;
using Project3.Domain.Entities;

namespace Project3.Application.Features.Commands;

public sealed record CreateServiceProviderCommand(    
    string Name,
    string Email,
    string Specialty)
    : IRequest<Result>;

public sealed class CreateServiceProviderValidator
    : AbstractValidator<CreateServiceProviderCommand>
{
    public CreateServiceProviderValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Specialty).NotEmpty().MaximumLength(150);
    }
}

public sealed class CreateServiceProviderHandler
    : IRequestHandler<CreateServiceProviderCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateServiceProviderHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(CreateServiceProviderCommand request, CancellationToken cancellationToken)
    {

        var provider = new ServiceProvider(
            Guid.NewGuid(),
            request.Name,
            request.Email,
            request.Specialty,
            isActive: true,
            createdAt: DateTime.UtcNow
        );

        await _unitOfWork.ServiceProviders.AddAsync(provider);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Provider created successfully.");
    }
}