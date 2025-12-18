using FluentValidation;
using MediatR;
using Project3.Application.Common.DTOs;
using Project3.Application.Common.Interfaces;
using Project3.Domain.Common.Response;

namespace Project3.Application.Features.Commands;

public sealed record UpdateWorkingHoursCommand(    Guid Id,
    int? DayOfWeek,
    TimeOnly? StartTime,
    TimeOnly? EndTime,
    bool? IsActive)
    : IRequest<Result>;

public sealed class UpdateWorkingHoursCommandValidator 
    : AbstractValidator<UpdateWorkingHoursCommand>
{
    public UpdateWorkingHoursCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Working hour Id is required.");

        When(x => x.DayOfWeek.HasValue, () =>
        {
            RuleFor(x => x.DayOfWeek)
                .InclusiveBetween(0, 6)
                .WithMessage("DayOfWeek must be between 0 and 6.");
        });

        // validaciebi marto im dros gaaketos roca start da end timebi orive arsebobs
        When(x => x.StartTime.HasValue && x.EndTime.HasValue, () =>
        {
            RuleFor(x => x.StartTime)
                .LessThan(x => x.EndTime)
                .WithMessage("Start time must be before end time.");
        });
    }
}

public sealed class UpdateWorkingHoursCommandHandler
    : IRequestHandler<UpdateWorkingHoursCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateWorkingHoursCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        UpdateWorkingHoursCommand request,
        CancellationToken cancellationToken)
    {
        var workingHour = await _unitOfWork.WorkingHours.GetByIdAsync(request.Id);
        if (workingHour is null)
            return Result.Failure("Working hour not found.");

        var newDay = request.DayOfWeek ?? workingHour.DayOfWeek;
        var newStart = request.StartTime ?? workingHour.StartTime;
        var newEnd = request.EndTime ?? workingHour.EndTime;

        var existingForDay = await _unitOfWork.WorkingHours
            .GetAllByProviderAndDayAsync(workingHour.ProviderId, newDay);

        var overlaps = existingForDay.Any(x =>
            x.Id != workingHour.Id && 
            x.StartTime < newEnd &&
            x.EndTime > newStart);

        if (overlaps)
            return Result.Failure("Updated working hour overlaps another working hour.");

        workingHour.Update(
            dayOfWeek: request.DayOfWeek,
            startTime: request.StartTime,
            endTime: request.EndTime,
            isActive: request.IsActive
        );

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Working hour updated successfully.");
    }
}
