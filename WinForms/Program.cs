using ApplicationLayer.InstructionGeneration.Demo;
using Infrastructure;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Seeding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WinForms;

internal static class Program
{
    [STAThread]
    private static async Task Main(string[] args)
    {
        //ApplicationConfiguration.Initialize();
        //
        //var builder = Host.CreateApplicationBuilder();
        //builder.Configuration
        //    .SetBasePath(AppContext.BaseDirectory)
        //    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
        //    .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: false)
        //    .AddEnvironmentVariables();
        //
        //builder.Services.AddInfrastructure(builder.Configuration);
        //builder.Services.AddTransient<Form1>();
        //
        //using var host = builder.Build();
        //using var scope = host.Services.CreateScope();
        //
        ////       if (args.Contains("--seed", StringComparer.OrdinalIgnoreCase))
        ////       {
        ////           var seeder = scope.ServiceProvider.GetRequiredService<DbSeeder>();
        ////           await seeder.SeedMinimalAsync();
        ////           Console.WriteLine("Seeding done");
        ////           return;
        ////       }
        //
        //if (args.Contains("--seed", StringComparer.OrdinalIgnoreCase))
        //{
        //    var logPath = Path.Combine(AppContext.BaseDirectory, "seed.log");
        //
        //    try
        //    {
        //        var cs = builder.Configuration.GetConnectionString("VkrIt");
        //
        //        File.WriteAllText(logPath,
        //            $"Seed START {DateTimeOffset.Now:O}{Environment.NewLine}" +
        //            $"BaseDir: {AppContext.BaseDirectory}{Environment.NewLine}" +
        //            $"Args: {string.Join(" ", args)}{Environment.NewLine}" +
        //            $"CS: {cs}{Environment.NewLine}");
        //
        //        var seeder = scope.ServiceProvider.GetRequiredService<DbSeeder>();
        //        await seeder.SeedMinimalAsync();
        //
        //        File.AppendAllText(logPath, $"Seed DONE {DateTimeOffset.Now:O}{Environment.NewLine}");
        //        return;
        //    }
        //    catch (Exception ex)
        //    {
        //        File.AppendAllText(logPath, $"Seed FAILED {DateTimeOffset.Now:O}{Environment.NewLine}{ex}{Environment.NewLine}");
        //        throw;
        //    }
        //}
        //
        //var dbContext = scope.ServiceProvider.GetRequiredService<VkrItDbContext>();
        //_ = dbContext.Database.CanConnect();
        //
        //Application.Run(scope.ServiceProvider.GetRequiredService<Form1>());

        // Временно: прогон демо-генератора (пишет в Debug + Console)
        InstructionDemo.Run();

        ApplicationConfiguration.Initialize();
        Application.Run(new Form1());
    }
}