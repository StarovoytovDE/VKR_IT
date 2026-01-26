using System.Collections.Generic;
using System.Threading.Tasks;

namespace WinForms.Services
{
    /// <summary>
    /// Заглушка сервиса UI-данных: линии/устройства.
    /// Заменить на реализацию, читающую данные из PostgreSQL через Infrastructure.
    /// </summary>
    public sealed class StubLineUiDataService : ILineUiDataService
    {
        /// <inheritdoc />
        public Task<IReadOnlyList<LineListItem>> GetLinesAsync()
        {
            IReadOnlyList<LineListItem> lines = new List<LineListItem>
            {
                new LineListItem(1, "ВЛ-500-01 (ПС A — ПС B)"),
                new LineListItem(2, "ВЛ-500-02 (ПС C — ПС D)")
            };

            return Task.FromResult(lines);
        }

        /// <inheritdoc />
        public Task<LineUiContext> GetLineContextAsync(int lineId)
        {
            var sideADevices = new List<LineSideDeviceItem>
            {
                new LineSideDeviceItem(101, "Устройство РЗА A-1"),
                new LineSideDeviceItem(102, "Устройство РЗА A-2")
            };

            var sideBDevices = new List<LineSideDeviceItem>
            {
                new LineSideDeviceItem(201, "Устройство РЗА B-1"),
                new LineSideDeviceItem(202, "Устройство РЗА B-2"),
                new LineSideDeviceItem(203, "Устройство РЗА B-3")
            };

            var ctx = new LineUiContext(
                SideA: new LineSideContext($"ПС (A) для линии {lineId}", sideADevices),
                SideB: new LineSideContext($"ПС (B) для линии {lineId}", sideBDevices));

            return Task.FromResult(ctx);
        }
    }
}
