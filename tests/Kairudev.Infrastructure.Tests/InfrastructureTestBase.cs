using Kairudev.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Kairudev.Infrastructure.Tests;

/// <summary>
/// Base class for infrastructure integration tests.
/// Creates an isolated SQLite in-memory database per test.
/// </summary>
public abstract class InfrastructureTestBase : IDisposable
{
    private readonly SqliteConnection _connection;
    protected readonly KairudevDbContext Context;

    protected InfrastructureTestBase()
    {
        // Keep connection open so the in-memory database persists for the test lifetime
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        var options = new DbContextOptionsBuilder<KairudevDbContext>()
            .UseSqlite(_connection)
            .Options;

        Context = new KairudevDbContext(options);
        Context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        Context.Dispose();
        _connection.Dispose();
    }
}
