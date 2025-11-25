using Project3.Domain.Entities;

namespace Project3.Application.Common.Interfaces;

public interface IWorkingHourRepository
{
    Task AddAsync(WorkingHour workingHour);
    Task DeleteAsync(Guid id);
    Task UpdateAsync(WorkingHour workingHour);
    Task<List<WorkingHour>> GetAllAsync();
    Task<WorkingHour?> GetByIdAsync(Guid id);
    Task<List<WorkingHour>> GetAllByProviderIdAsync(Guid providerId);
    Task<List<WorkingHour>> GetAllByDayOfWeekAsync(int dayOfWeek);
    Task<List<WorkingHour>> GetAllByProviderAndDayAsync(Guid providerId, int dayOfWeek);
    Task<List<WorkingHour>> GetActiveByProviderAsync(Guid providerId);
}
