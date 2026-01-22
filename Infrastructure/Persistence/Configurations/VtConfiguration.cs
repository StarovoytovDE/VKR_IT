using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// Конфигурация таблицы vt.
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
            .UseIdentityByDefaultColumn();

        builder.Property(x => x.DeviceId)
            .IsRequired();

        builder.Property(x => x.Main)
            .IsRequired();

        builder.Property(x => x.Name)
            .IsRequired();

        builder.Property(x => x.Place)
            .IsRequired();

        builder.HasOne(x => x.Device)
            .WithMany(x => x.Vts)
            .HasForeignKey(x => x.DeviceId);
    }
}
