using Microsoft.EntityFrameworkCore;
using Project3.Application.Common.Interfaces;
using Project3.Domain.Entities;

namespace Project3.Infrastructure.Persistence.Repositories;

public class ServiceProviderRepository : IServiceProviderRepository
{
    private readonly ApplicationDbContext _context;
    
    public ServiceProviderRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ServiceProvider provider)
    {
        await _context.ServiceProvider.AddAsync(provider);
    }

    public async Task<List<ServiceProvider>> GetAllAsync()
    {
        var result = await _context.ServiceProvider.ToListAsync();
        return result;
    }

    public async Task<ServiceProvider?> GetByIdAsync(Guid id)
    {
        return await _context.ServiceProvider.FindAsync(id);
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.ServiceProvider.FindAsync(id);
        if(entity != null) 
            _context.ServiceProvider.Remove(entity);
    }
    
    
}