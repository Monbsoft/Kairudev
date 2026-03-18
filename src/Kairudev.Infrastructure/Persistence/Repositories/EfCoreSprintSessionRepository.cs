using Kairudev.Domain.Identity;
using Kairudev.Domain.Sprint;
using Microsoft.EntityFrameworkCore;

namespace Kairudev.Infrastructure.Persistence.Repositories;

internal sealed class EfCoreSprintSessionRepository : ISprintSessionRepository
{
    private readonly KairudevDbContext _context;

    public EfCoreSprintSessionRepository(KairudevDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(SprintSession session, CancellationToken cancellationToken = default)
    {
        await _context.SprintSessions.AddAsync(session, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<SprintSession>> GetByDateAsync(
        DateOnly date,
        UserId ownerId,
        CancellationToken cancellationToken = default)
    {
        var startOfDay = new DateTimeOffset(date.Year, date.Month, date.Day, 0, 0, 0, TimeSpan.Zero);
        var endOfDay   = startOfDay.AddDays(1);

        return await _context.SprintSessions
            .Where(s => s.OwnerId == ownerId
                        && s.StartedAt >= startOfDay
                        && s.StartedAt < endOfDay)
            .OrderBy(s => s.StartedAt)
            .ToListAsync(cancellationToken);
    }
}
