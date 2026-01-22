using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// Конфигурация таблицы dfz.
/// </summary>
public sealed class DfzConfiguration : IEntityTypeConfiguration<Dfz>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Dfz> builder)
    {
        builder.ToTable("dfz");

        builder.HasKey(x => x.DfzId)
            .HasName("pk_dfz");

        builder.Property(x => x.DfzId)
            .UseIdentityByDefaultColumn();

        builder.Property(x => x.DeviceId)
            .IsRequired();

        builder.Property(x => x.Code)
            .IsRequired();

        builder.Property(x => x.Name)
            .IsRequired();

        builder.Property(x => x.HazDfz)
            .HasColumnName("haz_dfz")
            .IsRequired();

        builder.Property(x => x.State)
            .IsRequired();

        builder.HasOne(x => x.Device)
            .WithMany(x => x.Dfzs)
            .HasForeignKey(x => x.DeviceId);
    }
}
