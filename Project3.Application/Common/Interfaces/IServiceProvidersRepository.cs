using Project3.Domain.Entities;

namespace Project3.Application.Common.Interfaces;

public interface IServiceProviderRepository
{
    Task AddAsync(ServiceProvider provider);
    Task<ServiceProvider?> GetByIdAsync(Guid id);
    Task<List<ServiceProvider>> GetAllAsync();
    Task DeleteAsync(Guid id);
}