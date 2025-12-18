using Project3.Domain.Entities;

namespace Project3.Application.Common.Interfaces;

public interface IAppointmentRepository
{
    Task AddAsync(Appointment appointment);
    Task<List<Appointment>> GetAllAsync();
    Task<Appointment?> GetByIdAsync(Guid id);
    Task<List<Appointment>> GetByProviderAsync(Guid providerId);
    Task DeleteAsync(Guid id);
}