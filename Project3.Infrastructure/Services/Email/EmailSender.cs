using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;
using Project3.Application.Common.Interfaces;
using Project3.Domain.Entities;

namespace Project3.Infrastructure.Services.Email;

public class EmailSender : IEmailSender
{
    private readonly EmailSettings _emailSettings;
    private readonly ILogger<EmailSender> _logger;

    public EmailSender(EmailSettings emailSettings, ILogger<EmailSender> logger)
    {
        _emailSettings = emailSettings;
        _logger = logger;
    }

    public async Task<bool> SendAppointmentConfirmationAsync(Appointment appointment, CancellationToken ct)
    {
        try
        {
            var subject = "Appointment Confirmation";
            var body = GetAppointmentConfirmationTemplate(appointment);
            
            await SendEmailAsync(appointment.CustomerEmail, subject, body, ct);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send appointment confirmation email for appointment {AppointmentId}", appointment.Id);
            return false;
        }
    }

    public async Task<bool> SendAppointmentReminderAsync(Appointment appointment, CancellationToken ct)
    {
        try
        {
            var subject = "Appointment Reminder";
            var body = GetAppointmentReminderTemplate(appointment);
            
            await SendEmailAsync(appointment.CustomerEmail, subject, body, ct);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send appointment reminder email for appointment {AppointmentId}", appointment.Id);
            return false;
        }
    }

    public async Task<bool> SendAppointmentCancellationAsync(Appointment appointment, CancellationToken ct)
    {
        try
        {
            var subject = "Appointment Cancelled";
            var body = GetAppointmentCancellationTemplate(appointment);
            
            await SendEmailAsync(appointment.CustomerEmail, subject, body, ct);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send appointment cancellation email for appointment {AppointmentId}", appointment.Id);
            return false;
        }
    }

    private async Task SendEmailAsync(string toEmail, string subject, string body, CancellationToken ct)
    {
        using var smtpClient = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort)
        {
            Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password),
            EnableSsl = _emailSettings.EnableSsl
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_emailSettings.FromEmail, _emailSettings.FromName),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        mailMessage.To.Add(toEmail);

        await smtpClient.SendMailAsync(mailMessage, ct);
    }
    
    public async Task<bool> SendAppointmentRescheduledAsync(Appointment appointment, CancellationToken ct)
    {
        try
        {
            var subject = "Appointment Rescheduled";
            var body = GetAppointmentRescheduledTemplate(appointment);
        
            await SendEmailAsync(appointment.CustomerEmail, subject, body, ct);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send appointment rescheduled email for appointment {AppointmentId}", appointment.Id);
            return false;
        }
    }
    private string GetAppointmentConfirmationTemplate(Appointment appointment)
    {
        return $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background-color: #4CAF50; color: white; padding: 20px; text-align: center; }}
        .content {{ background-color: #f9f9f9; padding: 20px; }}
        .details {{ background-color: white; padding: 15px; margin: 15px 0; border-left: 4px solid #4CAF50; }}
        .footer {{ text-align: center; padding: 20px; color: #777; font-size: 12px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Appointment Confirmed!</h1>
        </div>
        <div class='content'>
            <p>Dear {appointment.CustomerName},</p>
            <p>Your appointment has been successfully confirmed.</p>
            
            <div class='details'>
                <h3>Appointment Details:</h3>
                <p><strong>Date & Time:</strong> {appointment.AppointmentDate:MMMM dd, yyyy 'at' hh:mm tt}</p>
                <p><strong>Service:</strong> {appointment.ServiceProvider}</p>
                <p><strong>Appointment ID:</strong> {appointment.Id}</p>
            </div>
            
            <p>If you need to make any changes, please contact us as soon as possible.</p>
        </div>
        <div class='footer'>
            <p>This is an automated message, please do not reply to this email.</p>
        </div>
    </div>
</body>
</html>";
    }

    private string GetAppointmentReminderTemplate(Appointment appointment)
    {
        return $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background-color: #FF9800; color: white; padding: 20px; text-align: center; }}
        .content {{ background-color: #f9f9f9; padding: 20px; }}
        .details {{ background-color: white; padding: 15px; margin: 15px 0; border-left: 4px solid #FF9800; }}
        .footer {{ text-align: center; padding: 20px; color: #777; font-size: 12px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Appointment Reminder</h1>
        </div>
        <div class='content'>
            <p>Dear {appointment.CustomerName},</p>
            <p>This is a friendly reminder about your upcoming appointment.</p>
            
            <div class='details'>
                <h3>Appointment Details:</h3>
                <p><strong>Date & Time:</strong> {appointment.AppointmentDate:MMMM dd, yyyy 'at' hh:mm tt}</p>
                <p><strong>Service:</strong> {appointment.ServiceProvider}</p>
            </div>
            
            <p>We look forward to seeing you!</p>
        </div>
        <div class='footer'>
            <p>This is an automated message, please do not reply to this email.</p>
        </div>
    </div>
</body>
</html>";
    }

    private string GetAppointmentCancellationTemplate(Appointment appointment)
    {
        return $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background-color: #f44336; color: white; padding: 20px; text-align: center; }}
        .content {{ background-color: #f9f9f9; padding: 20px; }}
        .details {{ background-color: white; padding: 15px; margin: 15px 0; border-left: 4px solid #f44336; }}
        .footer {{ text-align: center; padding: 20px; color: #777; font-size: 12px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Appointment Cancelled</h1>
        </div>
        <div class='content'>
            <p>Dear {appointment.CustomerName},</p>
            <p>Your appointment has been cancelled.</p>
            
            <div class='details'>
                <h3>Cancelled Appointment Details:</h3>
                <p><strong>Date & Time:</strong> {appointment.AppointmentDate:MMMM dd, yyyy 'at' hh:mm tt}</p>
                <p><strong>Service:</strong> {appointment.ServiceProvider}</p>
            </div>
            
            <p>If you would like to reschedule, please contact us.</p>
        </div>
        <div class='footer'>
            <p>This is an automated message, please do not reply to this email.</p>
        </div>
    </div>
</body>
</html>";
    }
    
    private string GetAppointmentRescheduledTemplate(Appointment appointment)
    {
        return $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background-color: #2196F3; color: white; padding: 20px; text-align: center; }}
        .content {{ background-color: #f9f9f9; padding: 20px; }}
        .details {{ background-color: white; padding: 15px; margin: 15px 0; border-left: 4px solid #2196F3; }}
        .footer {{ text-align: center; padding: 20px; color: #777; font-size: 12px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>Appointment Rescheduled</h1>
        </div>
        <div class='content'>
            <p>Dear {appointment.CustomerName},</p>
            <p>Your appointment has been successfully rescheduled.</p>
            
            <div class='details'>
                <h3>New Appointment Details:</h3>
                <p><strong>Date & Time:</strong> {appointment.AppointmentDate:MMMM dd, yyyy} at {appointment.StartTime:hh:mm tt}</p>
                <p><strong>Duration:</strong> {appointment.StartTime} - {appointment.EndTime}</p>
                <p><strong>Appointment ID:</strong> {appointment.Id}</p>
            </div>
            
            <p>If you need to make any further changes, please contact us.</p>
        </div>
        <div class='footer'>
            <p>This is an automated message, please do not reply to this email.</p>
        </div>
    </div>
</body>
</html>";
    }
}