using Kairudev.Domain.Identity;
using Kairudev.Domain.Sprint;
using Kairudev.Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Kairudev.Infrastructure.Persistence.Configurations;

internal sealed class SprintSessionConfiguration : IEntityTypeConfiguration<SprintSession>
{
    public void Configure(EntityTypeBuilder<SprintSession> builder)
    {
        builder.ToTable("SprintSessions");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasConversion(
                id => id.Value,
                value => SprintSessionId.From(value))
            .HasColumnType("uniqueidentifier")
            .ValueGeneratedNever();

        builder.OwnsOne(s => s.Name, nameBuilder =>
        {
            nameBuilder.Property(n => n.Value)
                .HasColumnName("Name")
                .HasColumnType("nvarchar(200)")
                .HasMaxLength(200)
                .IsRequired();
        });

        builder.Property(s => s.OwnerId)
            .HasConversion(v => v.Value, v => UserId.From(v))
            .HasColumnType("uniqueidentifier")
            .IsRequired();

        builder.Property(s => s.StartedAt)
            .HasColumnType("datetimeoffset")
            .IsRequired();

        builder.Property(s => s.EndedAt)
            .HasColumnType("datetimeoffset")
            .IsRequired();

        builder.Property(s => s.Outcome)
            .HasConversion<string>()
            .HasColumnType("nvarchar(50)")
            .IsRequired();

        builder.Property(s => s.LinkedTaskId)
            .HasConversion(
                id => id == null ? (Guid?)null : id.Value,
                value => value == null ? null : TaskId.From(value.Value))
            .HasColumnType("uniqueidentifier")
            .IsRequired(false);

        // Computed property — not persisted
        builder.Ignore(s => s.Duration);
    }
}
