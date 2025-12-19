using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Presistance
{
    internal class VkrItDbContextFactory : IDesignTimeDbContextFactory<VkrItDbContext>
    {
        public VkrItDbContext CreateDbContext(string[] args)
        {
            // DOTNET_ENVIRONMENT можно выставлять в PowerShell при необходимости
            var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";

            // Когда вы запускаете dotnet ef с --startup-project WinForms,
            // конфиги лежат в output WinForms, поэтому базу берём из BaseDirectory.
            var basePath = AppContext.BaseDirectory;

            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json",
                                optional: false, reloadOnChange: false)
                .AddJsonFile("appsettings.local.json",
                                optional: true, reloadOnChange: false)
                .AddJsonFile($"appsettings.{environment}.json",
                                optional: true, reloadOnChange: false)
                .AddEnvironmentVariables()
                .Build();

            var cs = configuration.GetConnectionString("VkrItDb");
            if (string.IsNullOrWhiteSpace(cs))
                throw new InvalidOperationException(
                            "Connection string 'VkrItDb' not found. " +
                            "Check appsettings*.json in WinForms output.");

            var options = new DbContextOptionsBuilder<VkrItDbContext>()
                .UseNpgsql(cs)
                .Options;

            return new VkrItDbContext(options);

        }
    }
}
