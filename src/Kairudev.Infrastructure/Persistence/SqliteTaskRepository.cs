using Kairudev.Domain.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Kairudev.Infrastructure.Persistence;

internal sealed class SqliteTaskRepository : ITaskRepository
{
    private readonly KairudevDbContext _context;

    public SqliteTaskRepository(KairudevDbContext context) => _context = context;

    public async Task AddAsync(DeveloperTask task, CancellationToken cancellationToken = default)
    {
        await _context.Tasks.AddAsync(task, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<DeveloperTask?> GetByIdAsync(TaskId id, CancellationToken cancellationToken = default) =>
        await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

    public async Task<IReadOnlyList<DeveloperTask>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _context.Tasks
            .OrderBy(t => t.CreatedAt)
            .ToListAsync(cancellationToken);

    public async Task UpdateAsync(DeveloperTask task, CancellationToken cancellationToken = default)
    {
        _context.Tasks.Update(task);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(TaskId id, CancellationToken cancellationToken = default)
    {
        var task = await GetByIdAsync(id, cancellationToken);
        if (task is not null)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
