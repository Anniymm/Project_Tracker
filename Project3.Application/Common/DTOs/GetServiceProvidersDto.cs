namespace Project3.Application.Common.DTOs;

public sealed record ServiceProviderDto(
    Guid Id,
    string Name,
    string Email,
    string Specialty,
    bool IsActive,
    DateTime CreatedAt
);