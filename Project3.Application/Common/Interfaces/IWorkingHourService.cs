using Project3.Application.Common.DTOs;

namespace Project3.Application.Common.Interfaces;

public interface IWorkingHourService
{
    Task<Guid> CreateAsync(CreateWorkingHoursDto dto, CancellationToken ct);

    Task UpdateAsync(UpdateWorkingHoursDto dto, CancellationToken ct);

    Task DeleteAsync(Guid id, CancellationToken ct);

    Task<List<GetWorkingHoursDto>> GetAllAsync(CancellationToken ct);

    Task<GetWorkingHoursDto?> GetByIdAsync(Guid id, CancellationToken ct);

    Task<List<GetWorkingHoursDto>> GetByProviderAsync(Guid providerId, CancellationToken ct);

    Task<List<GetWorkingHoursDto>> GetByDayOfWeekAsync(int dayOfWeek, CancellationToken ct);

    Task<List<GetWorkingHoursDto>> GetByProviderAndDayAsync(Guid providerId, int dayOfWeek, CancellationToken ct);

    Task<List<GetWorkingHoursDto>> GetActiveByProviderAsync(Guid providerId, CancellationToken ct);
}