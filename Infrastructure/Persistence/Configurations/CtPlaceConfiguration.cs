using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core конфигурация таблицы ct_place.
/// </summary>
public sealed class CtPlaceConfiguration : IEntityTypeConfiguration<CtPlace>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<CtPlace> builder)
    {
        builder.ToTable("ct_place");

        builder.HasKey(x => x.CtPlaceId);

        builder.Property(x => x.CtPlaceId)
            .HasColumnName("ct_place_id")
            .UseIdentityByDefaultColumn();

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .IsRequired();

        builder.Property(x => x.Place)
            .HasColumnName("place")
            .IsRequired();

        builder.Property(x => x.PlaceCode)
            .HasColumnName("place_code")
            .IsRequired();

        // Рекомендуемо: PlaceCode как логически уникальный справочник.
        builder.HasIndex(x => x.PlaceCode)
            .IsUnique();
    }
}
