namespace Project3.Domain.Entities;

public class BlockedTimes
{
    public Guid Id { get; private set; }
    public Guid ProviderId { get; private set; }
    public DateTime StartDateTime { get; private set; }
    public DateTime EndDateTime { get; private set; }
    public string Reason { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public ServiceProvider? ServiceProvider { get; private set; }

    public BlockedTimes() { }

    public BlockedTimes(
        Guid id,
        Guid providerId,
        DateTime startDateTime,
        DateTime endDateTime,
        string reason,
        DateTime createdAt)
    {
        Id = id;
        ProviderId = providerId;
        StartDateTime = startDateTime;
        EndDateTime = endDateTime;
        Reason = reason;
        CreatedAt = createdAt;
    }

    public void Update(
        DateTime? startDateTime,
        DateTime? endDateTime,
        string? reason)
    {
        StartDateTime = startDateTime ?? StartDateTime;
        EndDateTime = endDateTime ?? EndDateTime;
        Reason = reason ?? Reason;
    }
}