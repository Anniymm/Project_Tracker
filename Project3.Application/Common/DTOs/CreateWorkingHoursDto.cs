namespace Project3.Application.Common.DTOs;

public sealed record CreateWorkingHoursDto(
    Guid ProviderId,
    int DayOfWeek,
    TimeOnly StartTime,
    TimeOnly EndTime,
    bool IsActive);