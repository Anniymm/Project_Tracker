using Microsoft.Extensions.Logging;
using Project3.Application.Common.Interfaces;
using Project3.Domain.Enums;

namespace Project3.Infrastructure.Services.Email;

public class EmailService : IEmailService
{
    private readonly IEmailQueueRepository _emailQueueRepository;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<EmailService> _logger;

    public EmailService(
        IEmailQueueRepository emailQueueRepository,
        IEmailSender emailSender,
        ILogger<EmailService> logger)
    {
        _emailQueueRepository = emailQueueRepository;
        _emailSender = emailSender;
        _logger = logger;
    }

    public async Task ProcessPendingEmailsAsync(CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;
        
        while (!cancellationToken.IsCancellationRequested)
        {
            var emailQueue = await _emailQueueRepository.GetNextPendingAsync(now, cancellationToken);
            
            if (emailQueue == null)
                break; 

            try
            {
                emailQueue.MarkInProgress();
                await _emailQueueRepository.UpdateAsync(emailQueue);

                // emailis gagzavna notificationis typis mixedvit
                bool success = emailQueue.EmailNotificationType switch
                {
                    EmailNotificationType.Confirmation => 
                        await _emailSender.SendAppointmentConfirmationAsync(emailQueue.Appointment, cancellationToken),
    
                    EmailNotificationType.Reminder => 
                        await _emailSender.SendAppointmentReminderAsync(emailQueue.Appointment, cancellationToken),
    
                    EmailNotificationType.Cancellation => 
                        await _emailSender.SendAppointmentCancellationAsync(emailQueue.Appointment, cancellationToken),
    
                    EmailNotificationType.Rescheduled => 
                        await _emailSender.SendAppointmentRescheduledAsync(emailQueue.Appointment, cancellationToken),
    
                    _ => throw new NotImplementedException($"Email type {emailQueue.EmailNotificationType} not implemented")
                };

                if (success)
                {
                    emailQueue.MarkSent();
                    _logger.LogInformation(
                        "Successfully sent email {EmailId} of type {Type} to {Email}",
                        emailQueue.Id,
                        emailQueue.EmailNotificationType,
                        emailQueue.ToEmail);
                }
                else
                {
                    throw new Exception("Email sending returned false");
                }

                await _emailQueueRepository.UpdateAsync(emailQueue);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, 
                    "Failed to send email {EmailId} to {Email}",
                    emailQueue.Id,
                    emailQueue.ToEmail);

                emailQueue.MarkFailed(ex.Message);
                await _emailQueueRepository.UpdateAsync(emailQueue);
            }
        }
    }
}