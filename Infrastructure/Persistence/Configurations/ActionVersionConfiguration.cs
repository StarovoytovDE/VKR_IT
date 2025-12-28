using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class ActionVersionConfiguration : IEntityTypeConfiguration<ActionVersion>
{
    public void Configure(EntityTypeBuilder<ActionVersion> builder)
    {
        builder.ToTable("action_version");

        builder.HasKey(x => x.ActionVersionId)
            .HasName("pk_action_version");

        builder.Property(x => x.ActionVersionId)
            .UseIdentityByDefaultColumn();

        builder.Property(x => x.ActionId)
            .IsRequired();

        builder.Property(x => x.VersionLabel)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .HasDefaultValue(false)
            .IsRequired();

        builder.Property(x => x.ReleasedAt)
            .HasColumnType("timestamptz")
            .IsRequired();

        builder.HasIndex(x => new { x.ActionId, x.VersionLabel })
            .IsUnique()
            .HasDatabaseName("uq_action_version_action_label");

        builder.HasIndex(x => x.ActionId)
            .IsUnique()
            .HasFilter("is_active = true")
            .HasDatabaseName("uq_action_version_action_active");

        builder.HasOne(x => x.Action)
            .WithMany(x => x.ActionVersions)
            .HasForeignKey(x => x.ActionId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_action_version_action");
    }
}
