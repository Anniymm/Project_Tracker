using Project3.Domain.Entities;

namespace Project3.Application.Common.Interfaces;

public interface IWorkingHourService
{
    Task<Guid> CreateAsync(
        Guid providerId,
        int dayOfWeek,
        TimeOnly startTime,
        TimeOnly endTime,
        bool isActive,
        CancellationToken ct = default);

    Task UpdateAsync(
        Guid id,
        int? dayOfWeek,
        TimeOnly? startTime,
        TimeOnly? endTime,
        bool? isActive,
        CancellationToken ct = default);

    Task DeleteAsync(Guid id, CancellationToken ct = default);

    Task<List<WorkingHour>> GetAllAsync(CancellationToken ct = default);

    Task<WorkingHour?> GetByIdAsync(Guid id, CancellationToken ct = default);

    Task<List<WorkingHour>> GetByProviderAsync(Guid providerId, CancellationToken ct = default);

    Task<List<WorkingHour>> GetByDayOfWeekAsync(int dayOfWeek, CancellationToken ct = default);

    Task<List<WorkingHour>> GetByProviderAndDayAsync(
        Guid providerId,
        int dayOfWeek,
        CancellationToken ct = default);

    Task<List<WorkingHour>> GetActiveByProviderAsync(Guid providerId, CancellationToken ct = default);

    
}