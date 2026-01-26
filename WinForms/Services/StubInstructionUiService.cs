using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WinForms.Services
{
    /// <summary>
    /// Заглушка генератора указаний.
    /// Заменить на адаптер, который вызывает ваш ApplicationLayer:
    /// snapshot из БД → criteria → операции → список указаний.
    /// </summary>
    public sealed class StubInstructionUiService : IInstructionUiService
    {
        /// <inheritdoc />
        public Task<IReadOnlyList<DeviceInstructionResult>> GenerateAsync(GenerateInstructionsCommand command)
        {
            var results = new List<DeviceInstructionResult>();
            AppendForSide(results, command.SideA);
            AppendForSide(results, command.SideB);

            return Task.FromResult<IReadOnlyList<DeviceInstructionResult>>(results);
        }

        private static void AppendForSide(List<DeviceInstructionResult> results, SideDispatcherInput sideInput)
        {
            foreach (var d in sideInput.Devices)
            {
                var list = new List<string>
                {
                    $"(Действие) {UiActionNames.GetDisplayName(sideInput.Action)}"
                };

                if (d.dfzEnabled) list.Add("ДФЗ: введена (по вводу диспетчера)");
                if (d.dzlEnabled) list.Add("ДЗЛ: введена (по вводу диспетчера)");
                if (d.dzEnabled) list.Add("ДЗ: введена (по вводу диспетчера)");

                if (!d.dfzEnabled && !d.dzlEnabled && !d.dzEnabled)
                {
                    list.Add("Функции: не введены (по вводу диспетчера)");
                }

                results.Add(new DeviceInstructionResult(
                    sideInput.Side,
                    d.DeviceId,
                    d.DeviceName,
                    list));
            }
        }
    }
}
