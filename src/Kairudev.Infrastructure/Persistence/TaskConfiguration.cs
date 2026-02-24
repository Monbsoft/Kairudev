using Kairudev.Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kairudev.Infrastructure.Persistence;

internal sealed class TaskConfiguration : IEntityTypeConfiguration<DeveloperTask>
{
    public void Configure(EntityTypeBuilder<DeveloperTask> builder)
    {
        builder.ToTable("Tasks");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasConversion(
                id => id.Value,
                value => TaskId.From(value))
            .ValueGeneratedNever();

        builder.Property(t => t.Title)
            .HasConversion(
                title => title.Value,
                value => TaskTitle.Create(value).Value)
            .HasMaxLength(TaskTitle.MaxLength)
            .IsRequired();

        builder.Property(t => t.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(t => t.CreatedAt).IsRequired();
        builder.Property(t => t.CompletedAt);
    }
}
