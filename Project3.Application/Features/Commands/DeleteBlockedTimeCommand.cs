using FluentValidation;
using MediatR;
using Project3.Application.Common.Interfaces;
using Project3.Domain.Common.Response;

namespace Project3.Application.Features.Commands;

public sealed record DeleteBlockedTimeCommand(Guid Id) : IRequest<Result>;

public sealed class DeleteBlockedTimeCommandValidator 
    : AbstractValidator<DeleteBlockedTimeCommand>
{
    public DeleteBlockedTimeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id is required.");
    }
}

public sealed class DeleteBlockedTimeCommandHandler
    : IRequestHandler<DeleteBlockedTimeCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBlockedTimeCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        DeleteBlockedTimeCommand request,
        CancellationToken cancellationToken)
    {
        var blockedTime = await _unitOfWork.BlockedTimes.GetByIdAsync(request.Id);

        if (blockedTime is null)
            return Result.Failure("Blocked Time not found");

        await _unitOfWork.BlockedTimes.RemoveAsync(blockedTime);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Blocked time deleted successfully");
    }
}