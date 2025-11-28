namespace Project3.Application.Common.DTOs;

public sealed record BlockedTimeCreateUpdateDto(
    Guid ProviderId,
    DateTime StartDateTime,
    DateTime EndDateTime,
    string Reason
);