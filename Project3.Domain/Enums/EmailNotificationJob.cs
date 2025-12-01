namespace Project3.Domain.Enums;

public sealed record EmailNotificationJob(
    Guid AppointmentId,
    EmailNotificationType Type
);


// queue mwhirdeba rom email jobebi background workers gadasces, 
// dauyovnebliv rom ar gaushvas 