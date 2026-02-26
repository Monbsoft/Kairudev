using Kairudev.Domain.Journal;
using Microsoft.EntityFrameworkCore;

namespace Kairudev.Infrastructure.Persistence.Repositories;

public sealed class SqliteJournalEntryRepository : IJournalEntryRepository
{
    private readonly KairudevDbContext _context;

    public SqliteJournalEntryRepository(KairudevDbContext context) => _context = context;

    public async Task AddAsync(JournalEntry entry, CancellationToken cancellationToken = default)
    {
        await _context.JournalEntries.AddAsync(entry, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<JournalEntry?> GetByIdAsync(JournalEntryId id, CancellationToken cancellationToken = default)
        => await _context.JournalEntries
            .Include("_comments")
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    public async Task<IReadOnlyList<JournalEntry>> GetTodayEntriesAsync(DateOnly today, CancellationToken cancellationToken = default)
    {
        var start = today.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
        var end   = today.ToDateTime(TimeOnly.MaxValue, DateTimeKind.Utc);
        return await _context.JournalEntries
            .Include("_comments")
            .Where(e => e.OccurredAt >= start && e.OccurredAt <= end)
            .OrderBy(e => e.OccurredAt)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(JournalEntry entry, CancellationToken cancellationToken = default)
    {
        _context.JournalEntries.Update(entry);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
