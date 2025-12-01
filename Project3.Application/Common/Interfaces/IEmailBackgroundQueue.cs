using Project3.Domain.Enums;

namespace Project3.Application.Common.Interfaces;

public interface IEmailBackgroundQueue
{
    void Queue(EmailNotificationJob job);
    ValueTask<EmailNotificationJob?> DequeueAsync(CancellationToken cancellationToken);
}
