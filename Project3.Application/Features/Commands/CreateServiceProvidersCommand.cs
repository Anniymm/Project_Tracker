using FluentValidation;
using MediatR;
using Project3.Application.Common.DTOs;
using Project3.Application.Common.Interfaces;
using Project3.Domain.Common.Response;
using Project3.Domain.Entities;

namespace Project3.Application.Features.Commands;

public sealed record CreateServiceProviderCommand(CreateServiceProviderDto Provider)
    : IRequest<Result>;

public sealed class CreateServiceProviderValidator
    : AbstractValidator<CreateServiceProviderCommand>
{
    public CreateServiceProviderValidator()
    {
        RuleFor(x => x.Provider.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Provider.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Provider.Specialty).NotEmpty().MaximumLength(150);
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
        var dto = request.Provider;

        var provider = new ServiceProvider(
            Guid.NewGuid(),
            dto.Name,
            dto.Email,
            dto.Specialty,
            isActive: true,
            createdAt: DateTime.UtcNow
        );

        await _unitOfWork.ServiceProviders.AddAsync(provider);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Provider created successfully.");
    }
}