using Kairudev.Domain.Tasks;

namespace Kairudev.Application.Tests.Tasks;

internal sealed class FakeTaskRepository : ITaskRepository
{
    public List<DeveloperTask> Tasks { get; } = [];

    public Task AddAsync(DeveloperTask task, CancellationToken cancellationToken = default)
    {
        Tasks.Add(task);
        return Task.CompletedTask;
    }

    public Task<DeveloperTask?> GetByIdAsync(TaskId id, CancellationToken cancellationToken = default) =>
        Task.FromResult(Tasks.FirstOrDefault(t => t.Id == id));

    public Task<IReadOnlyList<DeveloperTask>> GetAllAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<DeveloperTask>>(Tasks.AsReadOnly());

    public Task UpdateAsync(DeveloperTask task, CancellationToken cancellationToken = default) =>
        Task.CompletedTask;

    public Task DeleteAsync(TaskId id, CancellationToken cancellationToken = default)
    {
        var task = Tasks.FirstOrDefault(t => t.Id == id);
        if (task is not null) Tasks.Remove(task);
        return Task.CompletedTask;
    }
}
