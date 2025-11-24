using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project3.Domain.Entities;

namespace Project3.Infrastructure.Persistence.Configurations;

public class WorkingHourConfiguration : IEntityTypeConfiguration<WorkingHour>
{
    public void Configure(EntityTypeBuilder<WorkingHour> builder)
    {
        builder.ToTable("working_hours");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(x => x.ProviderId)
            .HasColumnName("provider_id")
            .IsRequired();
        
        builder.HasOne<ServiceProvider>()
            .WithMany()
            .HasForeignKey(x => x.ProviderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(x => x.DayOfWeek)
            .HasColumnName("day_of_week")
            .IsRequired();

        builder.Property(x => x.StartTime)
            .HasColumnName("start_time")
            .HasColumnType("time")
            .IsRequired();

        builder.Property(x => x.EndTime)
            .HasColumnName("end_time")
            .HasColumnType("time")
            .IsRequired();

        builder.Property(x => x.IsActive)
            .HasColumnName("is_active")
            .IsRequired();

        // es araswori range-stvis
        builder.HasCheckConstraint(
            "CK_working_hours_start_before_end",
            "\"start_time\" < \"end_time\""
        );
    }
}