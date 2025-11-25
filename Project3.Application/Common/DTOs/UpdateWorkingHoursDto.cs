namespace Project3.Application.Common.DTOs;

public sealed record UpdateWorkingHoursDto(
    Guid Id,
    int? DayOfWeek,
    TimeOnly? StartTime,
    TimeOnly? EndTime,
    bool? IsActive);