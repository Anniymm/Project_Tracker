using Project3.Domain.Enums;

namespace Project3.Domain.Entities;

public class NotificationLogs
{
    public Guid Id { get; private set; }
    public Guid AppointmentId { get; private set; }
    public NotificationType Type { get; private set; }
    public DateTime SentAt { get; private set; }
    public NotificationStatus Status { get; private set; }

    public NotificationLogs() { }

    public NotificationLogs(
        Guid id,
        Guid appointmentId,
        NotificationType type,
        DateTime sentAt,
        NotificationStatus status)
    {
        Id = id;
        AppointmentId = appointmentId;
        Type = type;
        SentAt = sentAt;
        Status = status;
    }

    public void Update(
        NotificationType? type = null,
        NotificationStatus? status = null,
        DateTime? sentAt = null)
    {
        Type = type ?? Type;
        Status = status ?? Status;
        SentAt = sentAt ?? SentAt;
    }
}