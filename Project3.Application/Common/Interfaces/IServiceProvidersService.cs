using Project3.Application.Common.DTOs;
using Project3.Domain.Common.Response;

namespace Project3.Application.Common.Interfaces;

public interface IServiceProviderService
{
    Task<Result<Guid>> CreateAsync(CreateServiceProviderDto dto, CancellationToken ct);

    Task<Result> UpdateAsync(UpdateServiceProviderDto dto, CancellationToken ct);

    Task<Result> DeleteAsync(Guid id, CancellationToken ct);

    Task<Result<GetServiceProvidersDto?>> GetByIdAsync(Guid id, CancellationToken ct);

    Task<Result<List<GetServiceProvidersDto>>> GetAllAsync(CancellationToken ct);
}