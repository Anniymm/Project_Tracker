using FluentValidation;
using MediatR;
using Project3.Application.Common.Interfaces;
using Project3.Domain.Common.Response;

namespace Project3.Application.Features.Commands;

public sealed record UpdateBlockedTimeCommand(
    Guid Id,
    Guid ProviderId,
    DateTime StartDateTime,
    DateTime EndDateTime,
    string Reason ) : IRequest<Result>;
public sealed class UpdateBlockedTimeCommandValidator 
    : AbstractValidator<UpdateBlockedTimeCommand>
{
    public UpdateBlockedTimeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");
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
public sealed class UpdateBlockedTimeCommandHandler
    : IRequestHandler<UpdateBlockedTimeCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    public UpdateBlockedTimeCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<Result> Handle(
        UpdateBlockedTimeCommand request,
        CancellationToken cancellationToken)
    {
        var blockedTime = await _unitOfWork.BlockedTimes.GetByIdAsync(request.Id);
        if (blockedTime is null)
            return Result.Failure("Blocked time not found.");
        blockedTime.Update(
            request.StartDateTime,
            request.EndDateTime,
            request.Reason );
        await _unitOfWork.BlockedTimes.UpdateAsync(blockedTime);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success("Blocked time updated successfully.");
    }
}