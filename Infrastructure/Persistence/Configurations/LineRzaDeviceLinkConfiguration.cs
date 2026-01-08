using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// Конфигурация сущности связи линии и устройств РЗА.
/// </summary>
public sealed class LineRzaDeviceLinkConfiguration : IEntityTypeConfiguration<LineRzaDeviceLink>
{
    /// <summary>
    /// Настраивает модель таблицы line_rza_device.
    /// </summary>
    /// <param name="builder">Построитель сущности.</param>
    public void Configure(EntityTypeBuilder<LineRzaDeviceLink> builder)
    {
        builder.ToTable("line_rza_device", t =>
        {
            t.HasCheckConstraint(
                "ck_line_rza_device_line_side",
                "line_side IN (1,2)");
        });

        builder.HasKey(x => new { x.LineObjectId, x.DeviceObjectId })
            .HasName("pk_line_rza_device");

        builder.Property(x => x.LineSide)
            .HasConversion<short>()
            .HasColumnType("smallint")
            .HasColumnName("line_side")
            .IsRequired();

        builder.HasIndex(x => x.LineObjectId)
            .HasDatabaseName("ix_line_rza_device_line");

        builder.HasIndex(x => x.DeviceObjectId)
            .HasDatabaseName("ix_line_rza_device_device");

        builder.HasOne(x => x.LineObject)
            .WithMany(x => x.LineDevices)
            .HasForeignKey(x => x.LineObjectId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_line_rza_device_line_object");

        builder.HasOne(x => x.DeviceObject)
            .WithMany(x => x.DeviceLines)
            .HasForeignKey(x => x.DeviceObjectId)
            .OnDelete(DeleteBehavior.Restrict)
            .HasConstraintName("fk_line_rza_device_device_object");
    }
}
