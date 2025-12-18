using Project3.Domain.Entities;

namespace Project3.Application.Common.Interfaces;

public interface IBlockedTimesRepository
{
    Task<BlockedTime?> GetByIdAsync(Guid id);
    
    // radgan marto vfetchav da add-s an removes ar viyeneb, jobia IEnumerable an IReadOnlyList(indexing tu minda) 
    Task<IEnumerable<BlockedTime>> GetByProviderIdAsync(Guid providerId);
    Task<IEnumerable<BlockedTime>> GetAllAsync();
    Task AddAsync(BlockedTime blockedTime);
    Task UpdateAsync(BlockedTime blockedTime);
    Task RemoveAsync(BlockedTime blockedTime);
}