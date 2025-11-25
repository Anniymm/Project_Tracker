namespace Project3.Application.Common.DTOs;

public record UpdateAppointmentDto(
    string CustomerName,
    string CustomerEmail,
    string CustomerPhone,
    DateOnly AppointmentDate,
    TimeOnly StartTime,
    TimeOnly EndTime,
    bool IsRecurring,
    string? RecurrenceRule);