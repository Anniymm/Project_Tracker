using Microsoft.EntityFrameworkCore;
using Project3.Application.Common.Interfaces;
using Project3.Domain.Entities;

namespace Project3.Infrastructure.Persistence.Repositories;

public class BlockedTimesRepository : IBlockedTimesRepository
{
    private readonly ApplicationDbContext _context;

    public BlockedTimesRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(BlockedTime blockedTime)
    {
        await _context.BlockedTimes.AddAsync(blockedTime);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(BlockedTime blockedTime)
    {
        _context.BlockedTimes.Update(blockedTime);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(BlockedTime blockedTime)
    {
        _context.BlockedTimes.Remove(blockedTime);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<BlockedTime>> GetAllAsync()
    {
        return await _context.BlockedTimes.ToListAsync();
    }

    public async Task<IEnumerable<BlockedTime>> GetByProviderIdAsync(Guid providerId)
    {
        return await _context.BlockedTimes
            .Where(b => b.ProviderId == providerId)
            .ToListAsync();
    }

    public async Task<BlockedTime?> GetByIdAsync(Guid id)
    {
        return await _context.BlockedTimes
            .FirstOrDefaultAsync(b => b.Id == id);
    }
}