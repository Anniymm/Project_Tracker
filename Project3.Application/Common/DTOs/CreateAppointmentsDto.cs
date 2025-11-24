namespace Project3.Application.Common.DTOs;

public record CreateAppointmentDto(
    Guid ProviderId,
    string CustomerName,
    string CustomerEmail,
    string CustomerPhone,
    DateOnly AppointmentDate,
    TimeOnly StartTime,
    TimeOnly EndTime,
    bool IsRecurring,
    string? RecurrenceRule
);