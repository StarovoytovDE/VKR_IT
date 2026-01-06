using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ActionTable = Domain.Entities.ActionTable;

namespace Infrastructure.Persistence.Configurations;

public sealed class ActionConfiguration : IEntityTypeConfiguration<ActionTable>
{
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

        builder.HasIndex(x => x.Code)
            .IsUnique()
            .HasDatabaseName("uq_action_code");
    }
}
