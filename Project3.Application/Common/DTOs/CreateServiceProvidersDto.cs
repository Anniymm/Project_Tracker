namespace Project3.Application.Common.DTOs;

public record CreateServiceProviderDto(
    string Name,
    string Email,
    string Specialty
);