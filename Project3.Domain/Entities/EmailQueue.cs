using Project3.Domain.Enums;
namespace Project3.Domain.Entities;

public class EmailQueue
{
    public Guid Id { get; private set; }

    public Guid AppointmentId { get; private set; }
    public string ToEmail { get; private set; } = null!;
    public EmailNotificationType EmailNotificationType { get; private set; }

    public DateTimeOffset ScheduledAt { get; private set; }
    public EmailNotificationStatus Status { get; private set; }
    public string? FailureReason { get; private set; }
    public DateTimeOffset? SentAt { get; private set; }
    public int RetryCount { get; private set; }

    public virtual Appointment Appointment { get; private set; } = null!;

    private EmailQueue() { }

    public EmailQueue(
        Guid id,
        Guid appointmentId,
        string toEmail,
        EmailNotificationType notificationType,
        DateTimeOffset scheduledAt)
    {
        Id = id;
        AppointmentId = appointmentId;
        ToEmail = toEmail;
        EmailNotificationType = notificationType;
        ScheduledAt = scheduledAt;

        Status = EmailNotificationStatus.Pending;

        Validate();
    }

    public void MarkInProgress()
    {
        Status = EmailNotificationStatus.InProgress;
    }

    public void MarkSent()
    {
        Status = EmailNotificationStatus.Sent;
        SentAt = DateTimeOffset.UtcNow;
        FailureReason = null;
    }

    public void MarkFailed(string? reason)
    {
        Status = EmailNotificationStatus.Failed;
        FailureReason = reason;
    }

    private void Validate()
    {
        if (AppointmentId == Guid.Empty)
            throw new Exception("Appointment Id is required");
        
        //error middlewarebit unda errorebis ambebi
        
        // TODO: https://www.youtube.com/watch?v=rXdsm9R5TR0&t=156s
        if (string.IsNullOrEmpty(ToEmail))
            throw new Exception("To Email is required");
    }
}