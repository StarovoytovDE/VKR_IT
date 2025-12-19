using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public sealed class VkrItDbContext : DbContext
{
    public VkrItDbContext(DbContextOptions<VkrItDbContext> options)
        : base(options)
    {
    }

    // DbSet'ы добавите позже, когда начнёте заводить сущности.
    // public DbSet<YourEntity> YourEntities => Set<YourEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Если будете делать Fluent-конфигурации отдельными классами:
        // modelBuilder.ApplyConfigurationsFromAssembly(typeof(VkrItDbContext).Assembly);
    }
}

