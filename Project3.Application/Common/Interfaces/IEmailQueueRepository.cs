using Project3.Domain.Entities;

namespace Project3.Application.Common.Interfaces;

public interface IEmailQueueRepository
{
    Task AddAsync(EmailQueue email);
    Task UpdateAsync(EmailQueue email);
    
    // shemdegi scheduled + pending rom wamovighos erti cali - unda imushaos singleze workerma
    Task<EmailQueue?> GetNextPendingAsync(DateTimeOffset now, CancellationToken cancellationToken);
}
