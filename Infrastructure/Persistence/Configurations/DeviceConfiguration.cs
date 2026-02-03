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

        builder.Property(x => x.LineEndId)
            .HasColumnName("line_end_id")
            .IsRequired();

        builder.Property(x => x.CtPlaceId)
            .HasColumnName("ct_place_id"); // nullable

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .IsRequired();

        builder.Property(x => x.VtSwitchTrue)
            .HasColumnName("vt_switch_true")
            .IsRequired();

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

        builder.Property(x => x.MainVtId)
            .HasColumnName("main_vt_id")
            .IsRequired();

        builder.Property(x => x.ReserveVtId)
            .HasColumnName("reserve_vt_id")
            .IsRequired();

        builder.HasOne(x => x.LineEnd)
            .WithMany(x => x.Devices)
            .HasForeignKey(x => x.LineEndId)
            .HasConstraintName("fk_device_line_end");

        // FK на справочник места подключения ТТ
        builder.HasOne(x => x.CtPlace)
            .WithMany()
            .HasForeignKey(x => x.CtPlaceId)
            .HasConstraintName("fk_device_ct_place");

        // ВАЖНО: два разных FK на одну и ту же таблицу vt.
        // Для избежания циклов каскадного удаления используем Restrict.
        builder.HasOne(x => x.MainVt)
            .WithMany()
            .HasForeignKey(x => x.MainVtId)
            .HasConstraintName("fk_device_main_vt");

        builder.HasOne(x => x.ReserveVt)
            .WithMany()
            .HasForeignKey(x => x.ReserveVtId)
            .HasConstraintName("fk_device_reserve_vt");
    }
}
