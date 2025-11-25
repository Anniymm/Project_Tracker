namespace Project3.Application.Common.DTOs;

public record GetAppointmentDto(
    Guid Id,
    Guid ProviderId,
    string CustomerName,
    string CustomerEmail,
    string CustomerPhone,
    DateOnly AppointmentDate,
    TimeOnly StartTime,
    TimeOnly EndTime,
    string Status,
    string? CancellationReason,
    bool IsRecurring,
    string? RecurrenceRule,
    Guid? ParentAppointmentId
);