namespace Project3.Application.Common.DTOs;

public sealed record RescheduleAppointmentDto(
    Guid AppointmentId,
    DateOnly NewDate,
    TimeOnly NewStartTime,
    TimeOnly NewEndTime
);