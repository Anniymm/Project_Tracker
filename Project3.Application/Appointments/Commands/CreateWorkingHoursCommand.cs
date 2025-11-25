using FluentValidation;
using MediatR;
using Project3.Application.Common.DTOs;
using Project3.Application.Common.Interfaces;
using Project3.Domain.Common.Response;
using Project3.Domain.Entities;

namespace Project3.Application.Appointments.Commands;

public sealed record CreateWorkingHoursCommand(CreateWorkingHoursDto WorkingHours)
    : IRequest<Result<Guid>>;

public sealed class CreateWorkingHoursCommandValidator 
    : AbstractValidator<CreateWorkingHoursCommand>
{
    public CreateWorkingHoursCommandValidator()
    {
        RuleFor(x => x.WorkingHours.ProviderId)
            .NotEmpty()
            .WithMessage("Provider id cannot be empty");

        RuleFor(x => x.WorkingHours.DayOfWeek)
            .InclusiveBetween(0, 6)
            .WithMessage("DayOfWeek must be between 0 and 6");

        RuleFor(x => x.WorkingHours.StartTime)
            .NotEmpty()
            .WithMessage("Start time is required");

        RuleFor(x => x.WorkingHours.EndTime)
            .NotEmpty()
            .WithMessage("End time is required");

        RuleFor(x => x.WorkingHours.StartTime)
            .LessThan(x => x.WorkingHours.EndTime)
            .WithMessage("Start time must be before end time");
    }
}

public sealed class CreateWorkingHoursCommandHandler
    : IRequestHandler<CreateWorkingHoursCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateWorkingHoursCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(
        CreateWorkingHoursCommand request,
        CancellationToken cancellationToken)
    {
        var dto = request.WorkingHours;

        var provider = await _unitOfWork.ServiceProviders
            .GetByIdAsync(dto.ProviderId);

        if (provider is null)
            return Result<Guid>.Failure("Provider not found");

        var existingForDay = await _unitOfWork.WorkingHours
            .GetAllByProviderAndDayAsync(dto.ProviderId, dto.DayOfWeek);

        var overlaps = existingForDay.Any(x =>
            x.StartTime < dto.EndTime &&
            x.EndTime > dto.StartTime);

        if (overlaps)
            return Result<Guid>.Failure("Working hour overlaps an existing working hour");

        var workingHour = new WorkingHour(
            id: Guid.NewGuid(),
            providerId: dto.ProviderId,
            dayOfWeek: dto.DayOfWeek,
            startTime: dto.StartTime,
            endTime: dto.EndTime,
            isActive: dto.IsActive
        );

        await _unitOfWork.WorkingHours.AddAsync(workingHour);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(
            workingHour.Id,
            "Working hour created successfully"
        );
    }
}
