using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// Конфигурация таблицы device.
/// </summary>
public sealed class DeviceConfiguration : IEntityTypeConfiguration<Device>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.ToTable("device");

        builder.HasKey(x => x.DeviceId)
            .HasName("pk_device");

        builder.Property(x => x.DeviceId)
            .UseIdentityByDefaultColumn();

        builder.Property(x => x.ObjectId)
            .IsRequired();

        builder.Property(x => x.Name)
            .IsRequired();

        builder.Property(x => x.VtSwitchTrue)
            .HasColumnName("vt_switch_true")
            .IsRequired();

        builder.HasOne(x => x.Object)
            .WithMany(x => x.Devices)
            .HasForeignKey(x => x.ObjectId);
    }
}
