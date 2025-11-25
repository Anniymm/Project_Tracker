namespace Project3.Application.Common.DTOs;

public record GetServiceProviderDto(
    Guid Id,
    string Name,
    string Email,
    string Specialty,
    bool IsActive,
    DateTime CreatedAt
);