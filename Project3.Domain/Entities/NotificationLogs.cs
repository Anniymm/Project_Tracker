using Project3.Domain.Enums;

namespace Project3.Domain.Entities;

public class NotificationLogs
{
    public Guid Id { get; private set; }
    public Guid AppointmentId { get; private set; }
    public EmailNotificationType Type { get; private set; }
    public DateTime SentAt { get; private set; }
    public EmailNotificationStatus Status { get; private set; }
    public Appointment Appointment { get; private set; }

    public NotificationLogs() { }

    public NotificationLogs(
        Guid id,
        Guid appointmentId,
        EmailNotificationType type,
        DateTime sentAt,
        EmailNotificationStatus status)
    {
        Id = id;
        AppointmentId = appointmentId;
        Type = type;
        SentAt = sentAt;
        Status = status;
    }

    public void Update(
        EmailNotificationType? type = null,
        EmailNotificationStatus? status = null,
        DateTime? sentAt = null)
    {
        Type = type ?? Type;
        Status = status ?? Status;
        SentAt = sentAt ?? SentAt;
    }
}