using Microsoft.EntityFrameworkCore;
using Project3.Application.Common.Interfaces;
using Project3.Domain.Entities;

namespace Project3.Infrastructure.Persistence.Repositories
{
    public class NotificationLogsRepository : INotificationLogsRepository
    {
        private readonly ApplicationDbContext _context;

        public NotificationLogsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(NotificationLogs log)
        {
            await _context.NotificationLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<NotificationLogs>> GetAllAsync()
        {
            return await _context.NotificationLogs.ToListAsync();
        }

        public async Task<NotificationLogs?> GetByIdAsync(Guid id)
        {
            return await _context.NotificationLogs
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<NotificationLogs>> GetByAppointmentIdAsync(Guid appointmentId)
        {
            return await _context.NotificationLogs
                .Where(x => x.AppointmentId == appointmentId)
                .ToListAsync();
        }
    }
}