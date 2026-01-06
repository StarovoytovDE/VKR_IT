using Infrastructure.Persistence;
using Infrastructure.Persistence.Seeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("VkrIt");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                                    "Connection string 'VkrIt' not found.");
        }

        services.AddDbContext<VkrItDbContext>(options =>
            options.UseNpgsql(connectionString, npgsql => 
                                npgsql.MigrationsAssembly("Infrastructure"))
                                .UseSnakeCaseNamingConvention());
        services.AddTransient<DbSeeder>();

        return services;
    }
}
