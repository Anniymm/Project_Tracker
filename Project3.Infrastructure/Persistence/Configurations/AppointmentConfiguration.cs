using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project3.Domain.Entities;

namespace Project3.Infrastructure.Persistence.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.ToTable("appointment");
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Id)
            .ValueGeneratedNever();
        
        
        builder.Property(x => x.ProviderId)
            .IsRequired();
            
        builder.HasOne<ServiceProvider>() // ert providers 
            .WithMany()  // aqvs bevri appointment 
            .HasForeignKey(x => x.ProviderId);
            
        builder.Property(x => x.ParentAppointmentId)
            .IsRequired(false);
        
        builder.Property(x => x.CustomerName)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(x => x.CustomerEmail)
            .IsRequired()
            .HasMaxLength(100);
            
        builder.Property(x => x.CustomerPhone)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(x => x.AppointmentDate)
            .IsRequired();

        builder.Property(x => x.StartTime)
            .IsRequired();
        
        builder.Property(x => x.EndTime)
            .IsRequired();


        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<string>();
        
        builder.Property(x => x.CancellationReason)
            .IsRequired(false) 
            .HasMaxLength(250); 
        
        builder.Property(x => x.IsRecurring)
            .IsRequired();
            
        builder.Property(x => x.RecurrenceRule)
            .IsRequired(false)
            .HasMaxLength(150);
        
        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .IsRequired();
    }
}