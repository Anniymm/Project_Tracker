using Project3.Domain.Entities;

namespace Project3.Application.Common.Interfaces;

public interface IBlockedTimesRepository
{
    Task<BlockedTimes?> GetByIdAsync(Guid id);
    
    // radgan marto vfetchav da add-s an removes ar viyeneb, jobia IEnumerable an IReadOnlyList(indexing tu minda) 
    Task<IEnumerable<BlockedTimes>> GetByProviderIdAsync(Guid providerId);
    
    Task<IEnumerable<BlockedTimes>> GetAllAsync();
    
    Task AddAsync(BlockedTimes blockedTimes);
    
    Task UpdateAsync(BlockedTimes blockedTimes);
    
    Task RemoveAsync(BlockedTimes blockedTimes);
    
}