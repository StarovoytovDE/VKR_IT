using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Operations;
using Infrastructure;
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
        /// Точка входа WinForms-приложения.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            ApplicationConfiguration.Initialize();

            using IHost host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // Infrastructure: DbContext + IDeviceParamsReader.
                    services.AddInfrastructure(context.Configuration);

                    // ApplicationLayer: операции, реестр, сборщик критериев и генератор.
                    services.AddSingleton<IOperation, DfzFieldClosingOperation>();
                    services.AddSingleton<IOperation, DzlFieldClosingOperation>();
                    services.AddSingleton<IOperation, DzFieldClosingOperation>();
                    services.AddSingleton<IOperation, OapvOperation>();
                    services.AddSingleton<IOperation, TapvOperation>();
                    services.AddSingleton<IOperation, UpaskReceiversWithdrawalOperation>();
                    services.AddSingleton<IOperation, LineVtToReserveVoltageCircuitsTransferOperation>();
                    services.AddSingleton<IOperation, DisconnectLineCtFromDzoOperation>();
                    services.AddSingleton<IOperation, MtzoShinovkaAtoBOperation>();

                    services.AddSingleton<IOperation, DfzNoFieldClosingOperation>();
                    services.AddSingleton<IOperation, DzNoFieldClosingOperation>();
                    services.AddSingleton<IOperation, OapvNoFieldClosingOperation>();
                    services.AddSingleton<IOperation, TapvNoFieldClosingOperation>();

                    services.AddSingleton<IOperation, BusbarVtToReserveBusVtVoltageCircuitsTransferOperation>();
                    services.AddSingleton<IOperation, DfzSingleSideWithdrawalOperation>();

                    services.AddSingleton<IActionOperationRegistry, ActionOperationRegistry>();
                    services.AddSingleton<InstructionGenerator>();
                    services.AddSingleton<LineOperationCriteriaBuilder>();

                    // WinForms: реальные сервисы UI вместо заглушек.
                    services.AddScoped<ILineUiDataService, EfCoreLineUiDataService>();
                    services.AddScoped<IInstructionUiService, EfCoreInstructionUiService>();

                    services.AddTransient<InstructionGenerationForm>();
                })
                .Build();

            using IServiceScope scope = host.Services.CreateScope();
            var form = scope.ServiceProvider.GetRequiredService<InstructionGenerationForm>();
            Application.Run(form);
        }
    }
}
