namespace Project3.Application.Common.DTOs;

public sealed record UpdateServiceProviderDto(
    Guid Id,
    string? Name,
    string? Email,
    string? Specialty,
    bool? IsActive);
