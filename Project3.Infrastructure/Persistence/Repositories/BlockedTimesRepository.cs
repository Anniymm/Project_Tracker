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

    public async Task AddAsync(BlockedTimes blockedTime)
    {
        await _context.BlockedTimes.AddAsync(blockedTime);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(BlockedTimes blockedTime)
    {
        _context.BlockedTimes.Update(blockedTime);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(BlockedTimes blockedTime)
    {
        _context.BlockedTimes.Remove(blockedTime);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<BlockedTimes>> GetAllAsync()
    {
        return await _context.BlockedTimes.ToListAsync();
    }

    public async Task<IEnumerable<BlockedTimes>> GetByProviderIdAsync(Guid providerId)
    {
        return await _context.BlockedTimes
            .Where(b => b.ProviderId == providerId)
            .ToListAsync();
    }

    public async Task<BlockedTimes?> GetByIdAsync(Guid id)
    {
        return await _context.BlockedTimes
            .FirstOrDefaultAsync(b => b.Id == id);
    }
}