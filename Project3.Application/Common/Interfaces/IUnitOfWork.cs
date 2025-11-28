namespace Project3.Application.Common.Interfaces;

// radgan aaplication layerma ar icis ef core rom arsebobs, amitom datas shenaxvis kontrolostvis mwhirdeba es 
// magalitad - raghacebi an ertad sruldeba an ertad faildeba da amitom ert commitad unda gavides
// await _unitOfWork.Appointments.AddAsync(appointment);
// await _unitOfWork.NotificationLogs.AddAsync(log);
//
// // da mere ertxel vacommitebt 
// await _unitOfWork.SaveChangesAsync();
// UoW acts like a “database context of repositories.”

public interface IUnitOfWork
{
    IAppointmentRepository Appointments { get; }
    IServiceProviderRepository ServiceProviders { get; }
    IWorkingHourRepository WorkingHours { get; }
    IBlockedTimesRepository BlockedTimes { get; }
    

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
