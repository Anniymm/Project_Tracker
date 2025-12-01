using Project3.Domain.Entities;

namespace Project3.Application.Common.Interfaces;


public interface IEmailQueueRepository
{
    Task AddAsync(EmailQueue email);
    Task UpdateAsync(EmailQueue email);
    // statusebi rom daabrunos da samomavlod tu davu,ateb cdebs ramdenjer chafailda magalitad 
    Task<IEnumerable<EmailQueue>> GetPendingDueAsync(DateTimeOffset now, CancellationToken cancellationToken); 
}
