using Project3.Domain.Entities;

namespace Project3.Application.Common.Interfaces;
// Immediate emailebia - confirmation, cancellation 
// scheduled emailebi - reminder appointmentamde

public interface IAppointmentNotificationService
{
    Task SendConfirmationAsync(Appointment appointment, CancellationToken cancellationToken);
    Task SendReminderAsync(Appointment appointment, CancellationToken cancellationToken);
    Task SendCancellationAsync(Appointment appointment, CancellationToken cancellationToken);
}