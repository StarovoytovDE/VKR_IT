using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
/// Конфигурация таблицы app_user.
/// </summary>
public sealed class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.ToTable("app_user");

        builder.HasKey(x => x.UserId)
            .HasName("pk_app_user");

        builder.Property(x => x.UserId)
            .UseIdentityByDefaultColumn();

        builder.Property(x => x.Name).IsRequired();
        builder.Property(x => x.Login).IsRequired();
        builder.Property(x => x.PasswordHash).IsRequired();
    }
}
