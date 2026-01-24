using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// EF Core конфигурация таблицы vt.
/// </summary>
public sealed class VtConfiguration : IEntityTypeConfiguration<Vt>
{
    public void Configure(EntityTypeBuilder<Vt> builder)
    {
        builder.ToTable("vt");

        builder.HasKey(x => x.VtId);

        builder.Property(x => x.VtId)
            .HasColumnName("vt_id")
            .UseIdentityByDefaultColumn();

        builder.Property(x => x.DeviceId)
            .HasColumnName("device_id")
            .IsRequired();

        builder.Property(x => x.Main)
            .HasColumnName("main")
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

        builder.HasIndex(x => new { x.DeviceId, x.Main })
            .IsUnique();

        builder.HasOne(x => x.Device)
            .WithMany(d => d.Vts)
            .HasForeignKey(x => x.DeviceId);
    }
}
