using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public sealed class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
{
    public void Configure(EntityTypeBuilder<AppRole> builder)
    {
        builder.ToTable("app_role");

        builder.HasKey(x => x.RoleId)
            .HasName("pk_app_role");

        builder.Property(x => x.RoleId)
            .UseIdentityByDefaultColumn();

        builder.Property(x => x.Name)
            .IsRequired();

        builder.Property(x => x.Code)
            .IsRequired();

        builder.HasIndex(x => x.Code)
            .IsUnique()
            .HasDatabaseName("uq_app_role_code");
    }
}