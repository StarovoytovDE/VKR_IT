using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// Конфигурация таблицы oapv.
/// </summary>
public sealed class OapvConfiguration : IEntityTypeConfiguration<Oapv>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Oapv> builder)
    {
        builder.ToTable("oapv");

        builder.HasKey(x => x.OapvId)
            .HasName("pk_oapv");

        builder.Property(x => x.OapvId)
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
            .WithMany(x => x.Oapvs)
            .HasForeignKey(x => x.DeviceId);
    }
}
