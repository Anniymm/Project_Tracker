using FluentValidation;
using MediatR;
using Project3.Application.Common.Interfaces;
using Project3.Domain.Common.Response;
using Project3.Domain.Entities;

namespace Project3.Application.Features.Commands;

public sealed record CreateBlockedTimeCommand(
    Guid ProviderId,
    DateTime StartDateTime,
    DateTime EndDateTime,
    string Reason
) : IRequest<Result>;

public sealed class CreateBlockedTimeCommandValidator 
    : AbstractValidator<CreateBlockedTimeCommand>
{
    public CreateBlockedTimeCommandValidator()
    {
        RuleFor(x => x.ProviderId)
            .NotEmpty().WithMessage("ProviderId is required.");

        RuleFor(x => x.StartDateTime)
            .NotEmpty().WithMessage("StartDateTime is required.");

        RuleFor(x => x.EndDateTime)
            .NotEmpty().WithMessage("EndDateTime is required.")
            .GreaterThan(x => x.StartDateTime)
            .WithMessage("EndDateTime must be after StartDateTime.");

        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("Reason is required.")
            .MaximumLength(250).WithMessage("Reason cannot exceed 250 characters.");
    }
}

public sealed class CreateBlockedTimeCommandHandler
    : IRequestHandler<CreateBlockedTimeCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateBlockedTimeCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        CreateBlockedTimeCommand request,
        CancellationToken cancellationToken)
    {
        var blockedTime = new BlockedTime(
            Guid.NewGuid(),
            request.ProviderId,
            request.StartDateTime,
            request.EndDateTime,
            request.Reason,
            DateTime.UtcNow
        );

        await _unitOfWork.BlockedTimes.AddAsync(blockedTime);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Blocked time created successfully.");
    }
}