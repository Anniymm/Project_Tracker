using Microsoft.EntityFrameworkCore;
using Project3.Domain.Entities;

namespace Project3.Infrastructure.Persistence;

public class LoggingDbContext : DbContext
{
    public LoggingDbContext(DbContextOptions<LoggingDbContext> options)
        : base(options)
    {
    }

    public DbSet<NotificationLogs> NotificationLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("logging");

        modelBuilder.Entity<NotificationLogs>(entity =>
        {
            entity.ToTable("NotificationLogs", "logging");
            
            entity.HasKey(e => e.Id);

            entity.Property(e => e.AppointmentId).IsRequired();
            entity.Property(e => e.Type).IsRequired();
            entity.Property(e => e.Status).IsRequired();
            entity.Property(e => e.FailureReason).HasMaxLength(1000);
            entity.Property(e => e.CreatedAt).IsRequired();

            
            // index minda rom appointment table tavidan ki ar sheqmnas, ubralod unda miutitos arsebulze 
            entity.HasIndex(e => e.AppointmentId);
        });
    }
}
// EmailQueueProcessor
// ├─ Tries to send email
// ├─ if success → NotificationLogs.Success()
// ├─ if failure → NotificationLogs.Failed()
// └─ Save using INotificationLogsRepository
