using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Kairudev.Infrastructure.Persistence;

internal sealed class KairudevDbContextFactory : IDesignTimeDbContextFactory<KairudevDbContext>
{
    public KairudevDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<KairudevDbContext>()
            .UseSqlite("Data Source=kairudev.db")
            .Options;

        return new KairudevDbContext(options);
    }
}
