using Project3.Domain.Entities;

namespace Project3.Application.Common.Interfaces
{
    public interface INotificationLogsRepository
    {
        Task AddAsync(NotificationLogs log);
        Task<IEnumerable<NotificationLogs>> GetByAppointmentIdAsync(Guid appointmentId);
        Task<IEnumerable<NotificationLogs>> GetAllAsync();
        Task<NotificationLogs?> GetByIdAsync(Guid id);
    }
}