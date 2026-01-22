using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// Конфигурация таблицы substations.
/// </summary>
public sealed class SubstationConfiguration : IEntityTypeConfiguration<Substation>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Substation> builder)
    {
        builder.ToTable("substations");

        builder.HasKey(x => x.SubstationId)
            .HasName("pk_substations");

        builder.Property(x => x.SubstationId)
            .UseIdentityByDefaultColumn();

        builder.Property(x => x.DispatchName)
            .IsRequired();
    }
}
