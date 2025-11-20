using Project3.Domain.Enums;

namespace Project3.Domain.Entities;

public class Appointment
{
    public Guid Id { get; private set; }
    public Guid ProviderId { get; private set; }
    public ServiceProviders ServiceProvider { get; private set; }
    public string CustomerName { get; private set; }
    public string CustomerEmail { get; private set; }
    public string CustomerPhone { get; private set; }

    public DateOnly AppointmentDate { get; private set; }
    public TimeOnly StartTime { get; private set; }
    public TimeOnly EndTime { get; private set; }

    public AppointmentStatus Status { get; private set; }
 
    public string? CancellationReason { get; private set; }

    public bool IsRecurring { get; private set; }
    public string? RecurrenceRule { get; private set; }

    public Guid? ParentAppointmentId { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public Appointment() { }

    public Appointment(
        Guid id,
        Guid providerId,
        string customerName,
        string customerEmail,
        string customerPhone,
        DateOnly appointmentDate,
        TimeOnly startTime,
        TimeOnly endTime,
        AppointmentStatus status,
        string? cancellationReason,
        bool isRecurring,
        string? recurrenceRule,
        Guid? parentAppointmentId,
        DateTime createdAt,
        DateTime updatedAt)
    {
        Id = id;
        ProviderId = providerId;

        CustomerName = customerName;
        CustomerEmail = customerEmail;
        CustomerPhone = customerPhone;

        AppointmentDate = appointmentDate;
        StartTime = startTime;
        EndTime   = endTime;

        Status = status;
        CancellationReason = cancellationReason;

        IsRecurring = isRecurring;
        RecurrenceRule = recurrenceRule;

        ParentAppointmentId = parentAppointmentId;

        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public void Update(
        DateOnly? appointmentDate,
        TimeOnly? startTime,
        TimeOnly? endTime,
        AppointmentStatus? status,
        string? cancellationReason,
        bool? isRecurring,
        string? recurrenceRule)
    {
        AppointmentDate = appointmentDate ?? AppointmentDate;
        StartTime = startTime ?? StartTime;
        EndTime   = endTime   ?? EndTime;

        Status = status ?? Status;
        CancellationReason = cancellationReason ?? CancellationReason;

        IsRecurring = isRecurring ?? IsRecurring;
        RecurrenceRule = recurrenceRule ?? RecurrenceRule;

        UpdatedAt = DateTime.UtcNow;
    }
}
