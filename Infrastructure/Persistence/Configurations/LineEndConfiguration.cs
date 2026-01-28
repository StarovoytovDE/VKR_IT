using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// Конфигурация таблицы line_end.
/// </summary>
public sealed class LineEndConfiguration : IEntityTypeConfiguration<LineEnd>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<LineEnd> builder)
    {
        builder.ToTable("line_end");

        builder.HasKey(x => x.LineEndId)
            .HasName("pk_line_end");

        builder.Property(x => x.LineEndId)
            .UseIdentityByDefaultColumn();

        builder.Property(x => x.ObjectId)
            .HasColumnName("object_id")
            .IsRequired();

        builder.Property(x => x.SubstationId)
            .HasColumnName("substation_id")
            .IsRequired();

        builder.Property(x => x.SideCode)
            .HasColumnName("side_code")
            .IsRequired();

        builder.HasOne(x => x.Object)
            .WithMany(x => x.LineEnds)
            .HasForeignKey(x => x.ObjectId)
            .HasConstraintName("fk_line_end_object");

        builder.HasOne(x => x.Substation)
            .WithMany(x => x.LineEnds)
            .HasForeignKey(x => x.SubstationId)
            .HasConstraintName("fk_line_end_substation");

        builder.HasIndex(x => new { x.ObjectId, x.SideCode })
            .IsUnique()
            .HasDatabaseName("uq_line_end_object_side");

        builder.HasIndex(x => new { x.ObjectId, x.SubstationId })
            .IsUnique()
            .HasDatabaseName("uq_line_end_object_substation");
    }
}
