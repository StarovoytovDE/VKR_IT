using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core конфигурация таблицы vt.
/// 
/// Важно: таблица vt не содержит FK на устройство. Два FK находятся в таблице device:
/// main_vt_id и reserve_vt_id.
/// </summary>
public sealed class VtConfiguration : IEntityTypeConfiguration<Vt>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Vt> builder)
    {
        builder.ToTable("vt");

        builder.HasKey(x => x.VtId)
            .HasName("pk_vt");

        builder.Property(x => x.VtId)
            .HasColumnName("vt_id")
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
    }
}
