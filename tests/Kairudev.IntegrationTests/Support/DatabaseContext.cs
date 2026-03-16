using Kairudev.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Kairudev.IntegrationTests.Support;

public class DatabaseContext : IDisposable
{
    private readonly DbContextOptions<KairudevDbContext> _options;
    public KairudevDbContext DbContext { get; private set; }

    public DatabaseContext()
    {
        // Use in-memory SQLite for testing
        _options = new DbContextOptionsBuilder<KairudevDbContext>()
            .UseSqlite("Data Source=:memory:")
            .Options;

        DbContext = new KairudevDbContext(_options);
        DbContext.Database.EnsureCreated();
    }

    public void Dispose()
    {
        DbContext?.Dispose();
    }

    public void Reset()
    {
        DbContext?.Dispose();
        DbContext = new KairudevDbContext(_options);
        DbContext.Database.EnsureCreated();
    }
}
