namespace Project3.Application.Common.DTOs;

public record BlockedTimeDto(
    Guid Id,
    Guid ProviderId,
    DateTime StartDateTime,
    DateTime EndDateTime,
    string Reason,
    DateTime CreatedAt
);