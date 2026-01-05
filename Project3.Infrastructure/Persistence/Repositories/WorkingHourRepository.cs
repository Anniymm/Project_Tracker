using Microsoft.EntityFrameworkCore;
using System.Linq;
using Project3.Application.Common.Interfaces;
using Project3.Domain.Entities;

namespace Project3.Infrastructure.Persistence.Repositories;

public class WorkingHourRepository : IWorkingHourRepository
{
    private readonly ApplicationDbContext _context;

    public WorkingHourRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(WorkingHour workingHour)
    {
        await _context.WorkingHours.AddAsync(workingHour);
    }
    
    public async Task DeleteAsync(Guid id)
    {
        var workingHour = await _context.WorkingHours
            .FirstOrDefaultAsync(x => x.Id == id);

        if (workingHour != null)
            _context.WorkingHours.Remove(workingHour);
        // tu null aris mashin ra xdeba? rogor unda gavagebinot users eg information, pawuk?
    }

    public Task UpdateAsync(WorkingHour workingHour)
    {
        _context.WorkingHours.Update(workingHour);
        return Task.CompletedTask;
    }

    public Task<List<WorkingHour>> GetAllAsync()
        => _context.WorkingHours.ToListAsync();

    public Task<WorkingHour?> GetByIdAsync(Guid id)
        => _context.WorkingHours.FirstOrDefaultAsync(x => x.Id == id);

    public Task<List<WorkingHour>> GetAllByProviderIdAsync(Guid providerId)
        => _context.WorkingHours
            .Where(x => x.ProviderId == providerId)
            .ToListAsync();

    public Task<List<WorkingHour>> GetAllByDayOfWeekAsync(int dayOfWeek)
        => _context.WorkingHours
            .Where(x => x.DayOfWeek == dayOfWeek)
            .ToListAsync();

    public Task<List<WorkingHour>> GetAllByProviderAndDayAsync(Guid providerId, int dayOfWeek)
        => _context.WorkingHours
            .Where(x => x.ProviderId == providerId && x.DayOfWeek == dayOfWeek)
            .ToListAsync();
    
    public Task<List<WorkingHour>> GetActiveByProviderAsync(Guid providerId)
        => _context.WorkingHours
            .Where(x => x.ProviderId == providerId && x.IsActive)
            .ToListAsync();
}
