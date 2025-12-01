using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project3.Domain.Entities;

namespace Project3.Infrastructure.Persistence.Configurations;

public class EmailQueueConfiguration : IEntityTypeConfiguration<EmailQueue>
{
    public void Configure(EntityTypeBuilder<EmailQueue> builder)
    {
        builder.ToTable("email_queue");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ToEmail)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.EmailNotificationType)
            .IsRequired();

        builder.Property(x => x.Status)
            .IsRequired();

        builder.Property(x => x.ScheduledAt)
            .IsRequired();

        builder.Property(x => x.RetryCount)
            .IsRequired();

        builder.HasOne(x => x.Appointment)
            .WithMany()
            .HasForeignKey(x => x.AppointmentId);
    }
}