using FluentValidation;
using MediatR;
using Project3.Application.Common.DTOs;
using Project3.Application.Common.Interfaces;
using Project3.Domain.Common.Response;
using Project3.Domain.Entities;

namespace Project3.Application.Features.Commands;

public sealed record CreateWorkingHoursCommand(Guid ProviderId,
    int DayOfWeek,
    TimeOnly StartTime,
    TimeOnly EndTime,
    bool IsActive)
    : IRequest<Result<Guid>>;

public sealed class CreateWorkingHoursCommandValidator 
    : AbstractValidator<CreateWorkingHoursCommand>
{
    public CreateWorkingHoursCommandValidator()
    {
        RuleFor(x => x.ProviderId)
            .NotEmpty()
            .WithMessage("Provider id cannot be empty");

        RuleFor(x => x.DayOfWeek)
            .InclusiveBetween(0, 6)
            .WithMessage("DayOfWeek must be between 0 and 6");

        RuleFor(x => x.StartTime)
            .NotEmpty()
            .WithMessage("Start time is required");

        RuleFor(x => x.EndTime)
            .NotEmpty()
            .WithMessage("End time is required");

        RuleFor(x => x.StartTime)
            .LessThan(x => x.EndTime)
            .WithMessage("Start time must be before end time");
    }
}

// primary consturctori gamoiyene chemo lamazo zangushka
public sealed class CreateWorkingHoursCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<CreateWorkingHoursCommand, Result<Guid>>
{
    // private readonly IUnitOfWork _unitOfWork;
    //
    // public CreateWorkingHoursCommandHandler(IUnitOfWork unitOfWork)
    // {
    //     _unitOfWork = unitOfWork;
    // }

    public async Task<Result<Guid>> Handle(
        CreateWorkingHoursCommand request,
        CancellationToken cancellationToken)
    {
        
        var provider = await unitOfWork.ServiceProviders
            .GetByIdAsync(request.ProviderId);

        if (provider is null)
            return Result<Guid>.Failure("Provider not found");

        var existingForDay = await unitOfWork.WorkingHours
            .GetAllByProviderAndDayAsync(request.ProviderId, request.DayOfWeek);

        var overlaps = existingForDay.Any(x =>
            x.StartTime < request.EndTime &&
            x.EndTime > request.StartTime);

        if (overlaps)
            return Result<Guid>.Failure("Working hour overlaps an existing working hour");

        var workingHour = new WorkingHour(
            id: Guid.NewGuid(),
            providerId: request.ProviderId,
            dayOfWeek: request.DayOfWeek,
            startTime: request.StartTime,
            endTime: request.EndTime,
            isActive: request.IsActive
        );

        await unitOfWork.WorkingHours.AddAsync(workingHour);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(
            workingHour.Id,
            "Working hour created successfully"
        );
    }
}
