using Project3.Application.Common.Interfaces;
using Project3.Application.Common.Interfaces.Appointments;
using Project3.Infrastructure.Persistence.Repositories.Appointments;

namespace Project3.Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Appointments = new AppointmentRepository(context);
    }

    public IAppointmentRepository Appointments { get; }
    public IServiceProviderRepository ServiceProviders { get; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        => _context.SaveChangesAsync(cancellationToken);
    
    
}
