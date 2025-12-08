namespace Project3.Application.Common.Interfaces;

public interface IEmailService
{
    Task ProcessPendingEmailsAsync(CancellationToken cancellationToken);
}