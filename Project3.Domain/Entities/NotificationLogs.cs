using Project3.Domain.Enums;

namespace Project3.Domain.Entities;
public class NotificationLogs
{
    public Guid Id { get; private set; }              
    public Guid AppointmentId { get; private set; }    
    public EmailNotificationType Type { get; private set; }
    public EmailNotificationStatus Status { get; private set; }
    public string? FailureReason { get; private set; } 
    public DateTime CreatedAt { get; private set; }   
    public Appointment? Appointment { get; private set; }
    private NotificationLogs() { } 

  
    public static NotificationLogs Success(Guid appointmentId, EmailNotificationType type)
    {
        return new NotificationLogs
        {
            Id = Guid.NewGuid(),
            AppointmentId = appointmentId,
            Type = type,
            Status = EmailNotificationStatus.Sent,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static NotificationLogs Failed(Guid appointmentId, EmailNotificationType type, string? failureReason)
    {
        return new NotificationLogs
        {
            Id = Guid.NewGuid(),
            AppointmentId = appointmentId,
            Type = type,
            Status = EmailNotificationStatus.Failed,
            FailureReason = failureReason,
            CreatedAt = DateTime.UtcNow
        };
    }
}