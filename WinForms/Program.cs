using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows.Forms;
using WinForms.Services;
using WinForms.UI;

namespace WinForms
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа приложения.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            ApplicationConfiguration.Initialize();

            using IHost host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    // TODO: Заменить заглушки на реальные сервисы (Infrastructure/ApplicationLayer).
                    services.AddSingleton<ILineUiDataService, StubLineUiDataService>();
                    services.AddSingleton<IInstructionUiService, StubInstructionUiService>();

                    services.AddTransient<InstructionGenerationForm>();
                })
                .Build();

            using IServiceScope scope = host.Services.CreateScope();
            var form = scope.ServiceProvider.GetRequiredService<InstructionGenerationForm>();
            Application.Run(form);
        }
    }
}
