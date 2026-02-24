using Kairudev.Domain.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Kairudev.Infrastructure.Persistence;

public sealed class KairudevDbContext : DbContext
{
    public KairudevDbContext(DbContextOptions<KairudevDbContext> options) : base(options) { }

    public DbSet<DeveloperTask> Tasks => Set<DeveloperTask>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TaskConfiguration());
    }
}
