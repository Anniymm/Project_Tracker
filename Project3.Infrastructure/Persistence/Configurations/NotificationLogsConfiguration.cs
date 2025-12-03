using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project3.Domain.Entities;

namespace Project3.Infrastructure.Persistence.Configurations
{
    public class NotificationLogsConfiguration : IEntityTypeConfiguration<NotificationLogs>
    {
        public void Configure(EntityTypeBuilder<NotificationLogs> builder)
        {
            builder.ToTable("NotificationLogs", schema: "logging");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.AppointmentId)
                .IsRequired();

            builder.Property(x => x.Type)
                .IsRequired();

            builder.Property(x => x.Status)
                .IsRequired();

            builder.Property(x => x.FailureReason)
                .HasMaxLength(1000); 

            builder.Property(x => x.CreatedAt)
                .IsRequired();
            

            builder.HasOne(x => x.Appointment)
                .WithMany() 
                .HasForeignKey(x => x.AppointmentId)
                .OnDelete(DeleteBehavior.Restrict); 
        }
    }
}