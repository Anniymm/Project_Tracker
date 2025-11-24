using Microsoft.EntityFrameworkCore;
using Project3.Application.Common.Interfaces.Appointments;
using Project3.Domain.Entities;

namespace Project3.Infrastructure.Persistence.Repositories.Appointments;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly ApplicationDbContext _context;

    public AppointmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Appointment appointment)
    {
        await _context.Appointment.AddAsync(appointment);
    }

    public async Task<List<Appointment>> GetAllAsync()
    {
        return await _context.Appointment.ToListAsync();
    }

    public async Task<Appointment?> GetByIdAsync(Guid id)
    {
        return await _context.Appointment.FindAsync(id);
    }

    public async Task<List<Appointment>> GetByProviderAsync(Guid providerId)
    {
        return await _context.Appointment
            .Where(a => a.ProviderId == providerId)
            .ToListAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.Appointment.FindAsync(id);
        if (entity is not null)
            _context.Appointment.Remove(entity);
    }
}