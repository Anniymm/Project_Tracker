using Microsoft.EntityFrameworkCore;
using Project3.Domain.Entities;

namespace Project3.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    
     public DbSet<Appointment> Appointment => Set<Appointment>();
     public DbSet<ServiceProvider> ServiceProvider => Set<ServiceProvider>();
     public DbSet<WorkingHour> WorkingHours => Set<WorkingHour>();
     public DbSet<BlockedTime> BlockedTimes => Set<BlockedTime>();
     public  DbSet<EmailQueue> EmailQueue => Set<EmailQueue>();

     protected override void OnModelCreating(ModelBuilder modelBuilder)
     {
         base.OnModelCreating(modelBuilder);
         modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
     }
     
}