namespace Project3.Application.Common.DTOs;

public sealed record RescheduleAppointmentDto(
    DateOnly NewDate,
    TimeOnly NewStartTime,
    TimeOnly NewEndTime
);