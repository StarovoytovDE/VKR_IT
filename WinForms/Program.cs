using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WinForms;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();

        var builder = Host.CreateApplicationBuilder();
        builder.Configuration
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: false)
            .AddEnvironmentVariables();

        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddTransient<Form1>();

        using var host = builder.Build();
        using var scope = host.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<VkrItDbContext>();
        _ = dbContext.Database.CanConnect();

        Application.Run(scope.ServiceProvider.GetRequiredService<Form1>());
    }
}