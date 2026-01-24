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

        builder.Property(x => x.PlaceCode)
            .IsRequired();

        builder.HasOne(x => x.Device)
            .WithMany(x => x.CtPlaces)
            .HasForeignKey(x => x.DeviceId);

        // Обычно ct_place 1:1 на устройство (как у тебя по модели чтения).
        builder.HasIndex(x => x.DeviceId)
            .IsUnique()
            .HasDatabaseName("uq_ct_place_device");
    }
}
