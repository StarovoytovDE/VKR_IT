using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// Конфигурация таблицы action.
/// </summary>
public sealed class ActionConfiguration : IEntityTypeConfiguration<ActionTable>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<ActionTable> builder)
    {
        builder.ToTable("action");

        builder.HasKey(x => x.ActionId)
            .HasName("pk_action");

        builder.Property(x => x.ActionId)
            .UseIdentityByDefaultColumn();

        builder.Property(x => x.Code)
            .IsRequired();

        builder.Property(x => x.Name)
            .IsRequired();

        builder.Property(x => x.Description);
    }
}
