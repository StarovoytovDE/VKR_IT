using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// Конфигурация таблицы ct_place.
/// </summary>
public sealed class CtPlaceConfiguration : IEntityTypeConfiguration<CtPlace>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<CtPlace> builder)
    {
        builder.ToTable("ct_place");

        builder.HasKey(x => x.CtPlaceId)
            .HasName("pk_ct_place");

        builder.Property(x => x.CtPlaceId)
            .UseIdentityByDefaultColumn();

        builder.Property(x => x.DeviceId)
            .IsRequired();

        builder.Property(x => x.Name)
            .IsRequired();

        builder.Property(x => x.Place)
            .IsRequired();

        builder.HasOne(x => x.Device)
            .WithMany(x => x.CtPlaces)
            .HasForeignKey(x => x.DeviceId);
    }
}
