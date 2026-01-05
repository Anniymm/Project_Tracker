using FluentValidation;
using MediatR;
using Project3.Application.Common.Interfaces;
using Project3.Domain.Common.Response;

namespace Project3.Application.Features.Queries;

public sealed record GetWorkingHoursByProviderQuery(Guid ProviderId)
    : IRequest<Result<List<WorkingHourResponse>>>;

public sealed record WorkingHourResponse(
    Guid Id,
    Guid ProviderId,
    int DayOfWeek,
    string DayName,
    TimeOnly StartTime,
    TimeOnly EndTime,
    bool IsActive
);

public sealed class GetWorkingHoursQueryValidator 
    : AbstractValidator<GetWorkingHoursByProviderQuery>
{
    public GetWorkingHoursQueryValidator()
    {
        RuleFor(x => x.ProviderId)
            .NotEmpty()
            .WithMessage("Provider id cannot be empty");
    }
}

public sealed class GetWorkingHoursQueryHandler
    : IRequestHandler<GetWorkingHoursByProviderQuery, Result<List<WorkingHourResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetWorkingHoursQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<WorkingHourResponse>>> Handle(
        GetWorkingHoursByProviderQuery request,
        CancellationToken cancellationToken)
    {
        var provider = await _unitOfWork.ServiceProviders
            .GetByIdAsync(request.ProviderId);

        if (provider is null)
            return Result<List<WorkingHourResponse>>.Failure("Provider not found");

        var workingHours = await _unitOfWork.WorkingHours
            .GetAllByProviderIdAsync(request.ProviderId);

        var response = workingHours.Select(wh => new WorkingHourResponse(
            wh.Id,
            wh.ProviderId,
            wh.DayOfWeek,
            GetDayName(wh.DayOfWeek),
            wh.StartTime,
            wh.EndTime,
            wh.IsActive
        )).OrderBy(x => x.DayOfWeek).ThenBy(x => x.StartTime).ToList();

        return Result<List<WorkingHourResponse>>.Success(
            response,
            "Working hours retrieved successfully");
    }

    private static string GetDayName(int dayOfWeek) => dayOfWeek switch
    {
        0 => "Sunday",
        1 => "Monday",
        2 => "Tuesday",
        3 => "Wednesday",
        4 => "Thursday",
        5 => "Friday",
        6 => "Saturday",
        _ => "Unknown"
    };
}