using Kairudev.Domain.Journal;
using Kairudev.Domain.Pomodoro;
using Kairudev.Domain.Tasks;
using Kairudev.Infrastructure.Persistence.Configurations;
using Kairudev.Infrastructure.Persistence.Internal;
using Microsoft.EntityFrameworkCore;

namespace Kairudev.Infrastructure.Persistence;

public sealed class KairudevDbContext : DbContext
{
    public KairudevDbContext(DbContextOptions<KairudevDbContext> options) : base(options) { }

    public DbSet<DeveloperTask> Tasks => Set<DeveloperTask>();
    public DbSet<PomodoroSession> PomodoroSessions => Set<PomodoroSession>();
    internal DbSet<PomodoroSettingsRow> PomodoroSettings => Set<PomodoroSettingsRow>();
    public DbSet<JournalEntry> JournalEntries => Set<JournalEntry>();
    public DbSet<JournalComment> JournalComments => Set<JournalComment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TaskConfiguration());
        modelBuilder.ApplyConfiguration(new PomodoroSessionConfiguration());
        modelBuilder.ApplyConfiguration(new PomodoroSettingsConfiguration());
        modelBuilder.ApplyConfiguration(new JournalEntryConfiguration());
        modelBuilder.ApplyConfiguration(new JournalCommentConfiguration());
    }
}
