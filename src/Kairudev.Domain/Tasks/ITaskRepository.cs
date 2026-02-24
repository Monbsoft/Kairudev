namespace Kairudev.Domain.Tasks;

public interface ITaskRepository
{
    Task AddAsync(DeveloperTask task, CancellationToken cancellationToken = default);
    Task<DeveloperTask?> GetByIdAsync(TaskId id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DeveloperTask>> GetAllAsync(CancellationToken cancellationToken = default);
    Task UpdateAsync(DeveloperTask task, CancellationToken cancellationToken = default);
    Task DeleteAsync(TaskId id, CancellationToken cancellationToken = default);
}
