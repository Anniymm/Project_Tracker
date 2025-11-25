using Project3.Application.Common.DTOs;
using Project3.Domain.Common.Response;
using Project3.Domain.Enums;

namespace Project3.Application.Common.Interfaces;

public interface IAppointmentService
{
    Task<Result<Guid>> CreateAsync(CreateAppointmentDto dto, CancellationToken cancellationToken);

    Task<Result> UpdateAsync(UpdateAppointmentDto dto, CancellationToken cancellationToken);

    Task<Result> RescheduleAsync(RescheduleAppointmentDto dto, CancellationToken cancellationToken);

    Task<Result> CancelAsync(Guid appointmentId, string reason, CancellationToken cancellationToken);

    Task<Result> DeleteAsync(Guid appointmentId, CancellationToken cancellationToken);

    Task<Result<GetAppointmentDto?>> GetByIdAsync(Guid appointmentId, CancellationToken cancellationToken);

    Task<Result<List<GetAppointmentDto>>> GetByProviderAsync(Guid providerId, CancellationToken cancellationToken);

    Task<Result<List<GetAppointmentDto>>> GetAllAsync(CancellationToken cancellationToken);
}