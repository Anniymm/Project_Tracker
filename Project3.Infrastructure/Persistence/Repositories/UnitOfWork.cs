using Project3.Application.Common.Interfaces;
using Project3.Infrastructure.Persistence.Repositories.Appointments;

namespace Project3.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Appointments = new AppointmentRepository(context);
        ServiceProviders = new ServiceProviderRepository(context);
        WorkingHours = new WorkingHourRepository(context);
    }

    public IAppointmentRepository Appointments { get; }
    public IServiceProviderRepository ServiceProviders { get; }
    
    public IWorkingHourRepository WorkingHours { get; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        => _context.SaveChangesAsync(cancellationToken);
}
