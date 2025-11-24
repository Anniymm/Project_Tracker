namespace Project3.Domain.Entities;

public class WorkingHour
{
    public Guid Id { get; private set; }
    public Guid ProviderId { get; private set; }
    public int DayOfWeek { get; private set; }
    public TimeOnly StartTime { get; private set; }
    public TimeOnly EndTime { get; private set; }
    public bool IsActive { get; private set; }

    public WorkingHour() { }

    public WorkingHour(
        Guid id,
        Guid providerId,
        int dayOfWeek,
        TimeOnly startTime,
        TimeOnly endTime,
        bool isActive)
    {
        Id = id;
        ProviderId = providerId;
        DayOfWeek = dayOfWeek;
        StartTime = startTime;
        EndTime = endTime;
        IsActive = isActive;
    }

    public void Update(
        int? dayOfWeek,
        TimeOnly? startTime,
        TimeOnly? endTime,
        bool? isActive)
    {
        DayOfWeek = dayOfWeek ?? DayOfWeek;
        StartTime = startTime ?? StartTime;
        EndTime   = endTime   ?? EndTime;
        IsActive  = isActive  ?? IsActive;
    }
}