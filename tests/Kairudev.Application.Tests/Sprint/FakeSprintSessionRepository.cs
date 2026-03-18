using Kairudev.Domain.Identity;
using Kairudev.Domain.Sprint;

namespace Kairudev.Application.Tests.Sprint;

internal sealed class FakeSprintSessionRepository : ISprintSessionRepository
{
    public List<SprintSession> Sessions { get; } = [];

    public Task AddAsync(SprintSession session, CancellationToken cancellationToken = default)
    {
        Sessions.Add(session);
        return Task.CompletedTask;
    }

    public Task<IReadOnlyList<SprintSession>> GetByDateAsync(DateOnly date, UserId ownerId, CancellationToken cancellationToken = default)
    {
        IReadOnlyList<SprintSession> result = Sessions
            .Where(s => s.OwnerId == ownerId && DateOnly.FromDateTime(s.StartedAt.UtcDateTime) == date)
            .OrderBy(s => s.StartedAt)
            .ToList()
            .AsReadOnly();
        return Task.FromResult(result);
    }
}
