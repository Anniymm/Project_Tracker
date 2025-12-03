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
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(EmailQueue email)
    {
        _context.EmailQueue.Update(email);
        await _context.SaveChangesAsync();
    }

    public async Task<EmailQueue?> GetNextPendingAsync(DateTimeOffset now, CancellationToken cancellationToken)
    {
        // ert pending gmailze rom imushaos
        return await _context.EmailQueue
            .Where(e => e.Status == EmailNotificationStatus.Pending &&
                        e.ScheduledAt <= now)
            .OrderBy(e => e.ScheduledAt)
            .FirstOrDefaultAsync(cancellationToken);
    }
}