using FluentValidation;
using MediatR;
using Project3.Application.Common.DTOs;
using Project3.Application.Common.Interfaces;
using Project3.Domain.Common.Response;

namespace Project3.Application.Features.Commands;

public sealed record UpdateWorkingHoursCommand(UpdateWorkingHoursDto Dto)
    : IRequest<Result>;

public sealed class UpdateWorkingHoursCommandValidator 
    : AbstractValidator<UpdateWorkingHoursCommand>
{
    public UpdateWorkingHoursCommandValidator()
    {
        RuleFor(x => x.Dto.Id)
            .NotEmpty()
            .WithMessage("Working hour Id is required.");

        When(x => x.Dto.DayOfWeek.HasValue, () =>
        {
            RuleFor(x => x.Dto.DayOfWeek)
                .InclusiveBetween(0, 6)
                .WithMessage("DayOfWeek must be between 0 and 6.");
        });

        // validaciebi marto im dros gaaketos roca start da end timebi orive arsebobs
        When(x => x.Dto.StartTime.HasValue && x.Dto.EndTime.HasValue, () =>
        {
            RuleFor(x => x.Dto.StartTime)
                .LessThan(x => x.Dto.EndTime)
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
        var dto = request.Dto;

        var workingHour = await _unitOfWork.WorkingHours.GetByIdAsync(dto.Id);
        if (workingHour is null)
            return Result.Failure("Working hour not found.");

        var newDay = dto.DayOfWeek ?? workingHour.DayOfWeek;
        var newStart = dto.StartTime ?? workingHour.StartTime;
        var newEnd = dto.EndTime ?? workingHour.EndTime;

        var existingForDay = await _unitOfWork.WorkingHours
            .GetAllByProviderAndDayAsync(workingHour.ProviderId, newDay);

        var overlaps = existingForDay.Any(x =>
            x.Id != workingHour.Id && 
            x.StartTime < newEnd &&
            x.EndTime > newStart);

        if (overlaps)
            return Result.Failure("Updated working hour overlaps another working hour.");

        workingHour.Update(
            dayOfWeek: dto.DayOfWeek,
            startTime: dto.StartTime,
            endTime: dto.EndTime,
            isActive: dto.IsActive
        );

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Working hour updated successfully.");
    }
}
