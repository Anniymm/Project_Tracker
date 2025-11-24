namespace Project3.Application.Common.DTOs;

public sealed record CreateServiceProviderDto(
    string Name,
    string Email,
    string Specialty
);