using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project3.Domain.Entities;

namespace Project3.Infrastructure.Persistence.Configurations
{
    public class BlockedTimesConfiguration : IEntityTypeConfiguration<BlockedTime>
    {
        public void Configure(EntityTypeBuilder<BlockedTime> builder)
        {
            builder.ToTable("blocked_times");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedNever();

            builder.Property(x => x.ProviderId)
                .IsRequired();

            builder.Property(x => x.StartDateTime)
                .IsRequired();

            builder.Property(x => x.EndDateTime)
                .IsRequired();

            builder.Property(x => x.Reason)
                .HasMaxLength(500);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.HasOne(x => x.ServiceProvider)
                .WithMany()
                .HasForeignKey(x => x.ProviderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}