using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core конфигурация таблицы ct_place.
/// </summary>
public sealed class CtPlaceConfiguration : IEntityTypeConfiguration<CtPlace>
{
    public void Configure(EntityTypeBuilder<CtPlace> builder)
    {
        builder.ToTable("ct_place");

        builder.HasKey(x => x.CtPlaceId);

        builder.Property(x => x.CtPlaceId)
            .HasColumnName("ct_place_id")
            .UseIdentityByDefaultColumn();

        builder.Property(x => x.DeviceId)
            .HasColumnName("device_id")
            .IsRequired();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .IsRequired();

        builder.Property(x => x.Place)
            .HasColumnName("place")
            .IsRequired();

        builder.Property(x => x.PlaceCode)
            .HasColumnName("place_code")
            .IsRequired();

        builder.HasIndex(x => x.DeviceId)
            .IsUnique();

        builder.HasOne(x => x.Device)
            .WithMany(d => d.CtPlaces)
            .HasForeignKey(x => x.DeviceId);
    }
}
