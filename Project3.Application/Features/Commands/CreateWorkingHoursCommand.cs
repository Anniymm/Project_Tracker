using FluentValidation;
using MediatR;
using Project3.Application.Common.Interfaces;
using Project3.Domain.Common.Response;
using Project3.Domain.Entities;

namespace Project3.Application.Features.Commands;

public sealed record CreateWorkingHoursCommand(
    Guid ProviderId,
    int DayOfWeek,
    TimeOnly StartTime,
    TimeOnly EndTime,
    bool IsActive = true
) : IRequest<Result>;

public sealed class CreateWorkingHoursCommandValidator 
    : AbstractValidator<CreateWorkingHoursCommand>
{
    public CreateWorkingHoursCommandValidator()
    {
        RuleFor(x => x.ProviderId)
            .NotEmpty()
            .WithMessage("Provider ID is required");

        RuleFor(x => x.DayOfWeek)
            .InclusiveBetween(0, 6)
            .WithMessage("Day of week must be between 0 (Sunday) and 6 (Saturday)");

        RuleFor(x => x.StartTime)
            .NotEmpty()
            .WithMessage("Start time is required");

        RuleFor(x => x.EndTime)
            .NotEmpty()
            .WithMessage("End time is required.")
            .GreaterThan(x => x.StartTime)
            .WithMessage("End time must be after start time");
    }
}

public sealed class CreateWorkingHoursCommandHandler 
    : IRequestHandler<CreateWorkingHoursCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateWorkingHoursCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        CreateWorkingHoursCommand request, 
        CancellationToken cancellationToken)
    {

        var provider = await _unitOfWork.ServiceProviders.GetByIdAsync(request.ProviderId);
        if (provider == null)
            return Result.Failure("Service provider not found");

        // droebis overlapis shemowmeba
        var existingHours = await _unitOfWork.WorkingHours
            .GetAllByProviderAndDayAsync(request.ProviderId, request.DayOfWeek);

        foreach (var existing in existingHours.Where(x => x.IsActive))
        {
            bool overlaps = request.StartTime < existing.EndTime && 
                          request.EndTime > existing.StartTime;
            
            if (overlaps)
                return Result.Failure("Working hours overlap with existing hours for this day");
        }

        var workingHour = new WorkingHour(
            id: Guid.NewGuid(),
            providerId: request.ProviderId,
            dayOfWeek: request.DayOfWeek,
            startTime: request.StartTime,
            endTime: request.EndTime,
            isActive: request.IsActive
        );

        await _unitOfWork.WorkingHours.AddAsync(workingHour);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success("Working hour created successfully");
    }
}