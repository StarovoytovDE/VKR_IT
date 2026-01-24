using ApplicationLayer.InstructionGeneration.DeviceParams;
using Infrastructure.InstructionGeneration.DeviceParams;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Seeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

/// <summary>
/// Регистрация зависимостей слоя Infrastructure.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Добавляет сервисы инфраструктурного слоя (EF Core, сидинг, reader'ы параметров и т.п.).
    /// </summary>
    /// <param name="services">Коллекция сервисов DI.</param>
    /// <param name="configuration">Конфигурация приложения.</param>
    /// <returns>Коллекция сервисов DI.</returns>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("VkrIt");
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Connection string 'VkrIt' not found.");
        }

        services.AddDbContext<VkrItDbContext>(options =>
            options.UseNpgsql(connectionString, npgsql =>
                    npgsql.MigrationsAssembly("Infrastructure"))
                .UseSnakeCaseNamingConvention());

        services.AddTransient<DbSeeder>();

        // Reader агрегированных параметров устройства (снимок DeviceParamsSnapshot).
        services.AddScoped<IDeviceParamsReader, EfCoreDeviceParamsReader>();

        return services;
    }
}
