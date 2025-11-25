namespace Project3.Application.Common.DTOs;

public record GetServiceProvidersDto(
    Guid Id,
    string Name,
    string Email,
    string Specialty,
    bool IsActive,
    DateTime CreatedAt
);