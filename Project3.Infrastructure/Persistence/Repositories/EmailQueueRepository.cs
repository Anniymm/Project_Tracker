using Microsoft.EntityFrameworkCore;
using Project3.Application.Common.Interfaces;
using Project3.Domain.Entities;
using Project3.Domain.Enums;

namespace Project3.Infrastructure.Persistence.Repositories;

public class EmailQueueRepository : IEmailQueueRepository
{
    private readonly ApplicationDbContext _context;

    public EmailQueueRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(EmailQueue email)
    {
        await _context.EmailQueue.AddAsync(email);
    }

    public Task UpdateAsync(EmailQueue email)
    {
        _context.EmailQueue.Update(email);
        return Task.CompletedTask;
    }

    public async Task<IEnumerable<EmailQueue>> GetPendingDueAsync(
        DateTimeOffset now,
        CancellationToken cancellationToken)
    {
        return await _context.EmailQueue
            .Where(x =>
                x.ScheduledAt <= now &&
                (x.Status == EmailNotificationStatus.Pending ||
                 x.Status == EmailNotificationStatus.Failed))
            .ToListAsync(cancellationToken);
    }
}
