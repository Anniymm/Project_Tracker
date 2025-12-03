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
        // sxva schemashi unda chavwero - oghond igive databaseshi amitom 
        // connectionsstring igive rcheba
        modelBuilder.Entity<NotificationLogs>().ToTable("NotificationLogs", schema: "logging");

        base.OnModelCreating(modelBuilder);
    }
}

// EmailQueueProcessor
// ├─ Tries to send email
// ├─ if success → NotificationLogs.Success()
// ├─ if failure → NotificationLogs.Failed()
// └─ Save using INotificationLogsRepository
