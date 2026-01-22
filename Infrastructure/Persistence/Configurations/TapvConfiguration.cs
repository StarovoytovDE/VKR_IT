using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// Конфигурация таблицы tapv.
/// </summary>
public sealed class TapvConfiguration : IEntityTypeConfiguration<Tapv>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Tapv> builder)
    {
        builder.ToTable("tapv");

        builder.HasKey(x => x.TapvId)
            .HasName("pk_tapv");

        builder.Property(x => x.TapvId)
            .UseIdentityByDefaultColumn();

        builder.Property(x => x.DeviceId)
            .IsRequired();

        builder.Property(x => x.Code)
            .IsRequired();

        builder.Property(x => x.Name)
            .IsRequired();

        builder.Property(x => x.SwitchOff)
            .HasColumnName("switch_off")
            .IsRequired();

        builder.Property(x => x.State)
            .IsRequired();

        builder.HasOne(x => x.Device)
            .WithMany(x => x.Tapvs)
            .HasForeignKey(x => x.DeviceId);
    }
}
