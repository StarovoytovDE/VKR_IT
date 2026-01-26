using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// Конфигурация таблицы mtz_busbar.
/// </summary>
public sealed class MtzBusbarConfiguration : IEntityTypeConfiguration<MtzBusbar>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<MtzBusbar> builder)
    {
        builder.ToTable("mtz_busbar");

        builder.HasKey(x => x.MtzBusbarId)
            .HasName("pk_mtz_busbar");

        builder.Property(x => x.MtzBusbarId)
            .UseIdentityByDefaultColumn();

        builder.Property(x => x.DeviceId)
            .IsRequired();

        builder.Property(x => x.Code)
            .IsRequired();

        builder.Property(x => x.Name)
            .IsRequired();

        builder.Property(x => x.State)
            .IsRequired();

        builder.HasOne(x => x.Device)
            .WithMany(x => x.MtzBusbars)
            .HasForeignKey(x => x.DeviceId);
    }
}
