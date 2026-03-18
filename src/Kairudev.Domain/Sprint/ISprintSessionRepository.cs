using Kairudev.Domain.Identity;

namespace Kairudev.Domain.Sprint;

public interface ISprintSessionRepository
{
    Task AddAsync(SprintSession session, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SprintSession>> GetByDateAsync(DateOnly date, UserId ownerId, CancellationToken cancellationToken = default);
}
