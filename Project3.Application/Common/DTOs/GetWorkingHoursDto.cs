namespace Project3.Application.Common.DTOs;

public record GetWorkingHoursDto
(
    Guid Id,
    Guid ProviderId,
    int DayOfWeek,
    TimeOnly StartTime,
    TimeOnly EndTime,
    bool IsActive
);