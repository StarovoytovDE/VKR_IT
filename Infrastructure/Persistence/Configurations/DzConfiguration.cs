using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// Конфигурация таблицы dz.
/// </summary>
public sealed class DzConfiguration : IEntityTypeConfiguration<Dz>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Dz> builder)
    {
        builder.ToTable("dz");

        builder.HasKey(x => x.DzId)
            .HasName("pk_dz");

        builder.Property(x => x.DzId)
            .UseIdentityByDefaultColumn();

        builder.Property(x => x.DeviceId)
            .IsRequired();

        builder.Property(x => x.Code)
            .IsRequired();

        builder.Property(x => x.Name)
            .IsRequired();

        builder.Property(x => x.HazDz)
            .HasColumnName("haz_dz")
            .IsRequired();

        builder.Property(x => x.State)
            .IsRequired();

        builder.HasOne(x => x.Device)
            .WithMany(x => x.Dzs)
            .HasForeignKey(x => x.DeviceId);
    }
}
