using Microsoft.Extensions.Hosting;
using Project3.Application.Common.Interfaces;
using Project3.Domain.Entities;
using Project3.Domain.Enums;

namespace Project3.Infrastructure.BackgroundServices;

public class EmailQueueWorker : BackgroundService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailSender _emailSender;
    private readonly TimeSpan _pollInterval = TimeSpan.FromMinutes(1);

    public EmailQueueWorker(
        IUnitOfWork unitOfWork,
        IEmailSender emailSender)
    {
        _unitOfWork = unitOfWork;
        _emailSender = emailSender;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await ProcessQueue(stoppingToken);
            await Task.Delay(_pollInterval, stoppingToken);
        }
    }

    private async Task ProcessQueue(CancellationToken ct)
    {
        var now = DateTimeOffset.UtcNow;

        var dueEmails = await _unitOfWork.EmailQueues
            .GetPendingDueAsync(now, ct);

        foreach (var job in dueEmails)
        {
            job.MarkInProgress();

            var appointment = await _unitOfWork.Appointments.GetByIdAsync(job.AppointmentId);
            if (appointment is null)
            {
                continue;
            }
            
            _ = job.EmailNotificationType switch
            {
                EmailNotificationType.Confirmation =>
                    await _emailSender.SendAppointmentConfirmationAsync(appointment, ct),

                EmailNotificationType.Reminder =>
                    await _emailSender.SendAppointmentReminderAsync(appointment, ct),

                EmailNotificationType.Cancellation =>
                    await _emailSender.SendAppointmentCancellationAsync(appointment, ct),

                _ => true 
            };

            job.MarkSent();

            var log = new NotificationLogs(
                id: Guid.NewGuid(),
                appointmentId: job.AppointmentId,
                type: job.EmailNotificationType,
                sentAt: DateTime.UtcNow,
                status: EmailNotificationStatus.Sent
            );

            await _unitOfWork.NotificationLogs.AddAsync(log);
            await _unitOfWork.EmailQueues.UpdateAsync(job);
            await _unitOfWork.SaveChangesAsync(ct);
        }
    }
}
