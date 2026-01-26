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

        builder.Property(x => x.DzoSwitchTrue)
            .HasColumnName("dzo_switch_true")
            .IsRequired();

        builder.Property(x => x.UpaskSwitchTrue)
            .HasColumnName("upask_switch_true")
            .IsRequired();

        builder.Property(x => x.FieldClosingAllowed)
            .HasColumnName("field_closing_allowed")
            .IsRequired();

    builder.Property(x => x.CtRemainsEnergized)
            .HasColumnName("ct_remains_energized")
            .IsRequired();
    }
}
