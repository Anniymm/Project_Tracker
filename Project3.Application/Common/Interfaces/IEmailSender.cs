using Project3.Domain.Entities;

namespace Project3.Application.Common.Interfaces;

public interface IEmailSender
{
    Task<bool> SendAppointmentConfirmationAsync(Appointment appointment, CancellationToken ct);
    Task<bool> SendAppointmentReminderAsync(Appointment appointment, CancellationToken ct);
    Task<bool> SendAppointmentCancellationAsync(Appointment appointment, CancellationToken ct);
}
