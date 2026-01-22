using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// Конфигурация таблицы dzl.
/// </summary>
public sealed class DzlConfiguration : IEntityTypeConfiguration<Dzl>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Dzl> builder)
    {
        builder.ToTable("dzl");

        builder.HasKey(x => x.DzlId)
            .HasName("pk_dzl");

        builder.Property(x => x.DzlId)
            .UseIdentityByDefaultColumn();

        builder.Property(x => x.DeviceId)
            .IsRequired();

        builder.Property(x => x.Code)
            .IsRequired();

        builder.Property(x => x.Name)
            .IsRequired();

        builder.Property(x => x.HazDzl)
            .HasColumnName("haz_dzl")
            .IsRequired();

        builder.Property(x => x.State)
            .IsRequired();

        builder.HasOne(x => x.Device)
            .WithMany(x => x.Dzls)
            .HasForeignKey(x => x.DeviceId);
    }
}
