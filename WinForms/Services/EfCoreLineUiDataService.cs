using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WinForms.Services
{
    /// <summary>
    /// Реальная реализация ILineUiDataService на EF Core.
    /// Для WinForms используется IDbContextFactory, чтобы исключить конкурентное использование одного DbContext.
    /// </summary>
    public sealed class EfCoreLineUiDataService : ILineUiDataService
    {
        private readonly IDbContextFactory<VkrItDbContext> _dbFactory;

        /// <summary>
        /// Создаёт сервис.
        /// </summary>
        /// <param name="dbFactory">Фабрика DbContext (создаёт новый контекст на каждый вызов).</param>
        public EfCoreLineUiDataService(IDbContextFactory<VkrItDbContext> dbFactory)
        {
            _dbFactory = dbFactory ?? throw new ArgumentNullException(nameof(dbFactory));
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<LineListItem>> GetLinesAsync()
        {
            await using var db = await _dbFactory.CreateDbContextAsync();

            // В текущей модели "Objects" — это линии.
            // При необходимости позже можно отфильтровать по ObjectType (LINE).
            var list = await db.Objects
                .AsNoTracking()
                .OrderBy(x => x.DispatchName)
                .Select(x => new LineListItem((int)x.ObjectId, x.DispatchName))
                .ToListAsync();

            return list;
        }

        /// <inheritdoc />
        public async Task<LineUiContext> GetLineContextAsync(int lineId)
        {
            await using var db = await _dbFactory.CreateDbContextAsync();

            var ends = await db.LineEnds
                .AsNoTracking()
                .Where(x => x.ObjectId == lineId)
                .Include(x => x.Substation)
                .Include(x => x.Devices)
                .ToListAsync();

            var sideA = ends.FirstOrDefault(x => string.Equals(x.SideCode, "A", StringComparison.OrdinalIgnoreCase));
            var sideB = ends.FirstOrDefault(x => string.Equals(x.SideCode, "B", StringComparison.OrdinalIgnoreCase));

            var ctxA = new LineSideContext(
                SubstationName: sideA?.Substation.DispatchName ?? "(ПС не задана)",
                Devices: MapDevices(sideA));

            var ctxB = new LineSideContext(
                SubstationName: sideB?.Substation.DispatchName ?? "(ПС не задана)",
                Devices: MapDevices(sideB));

            return new LineUiContext(ctxA, ctxB);
        }

        private static IReadOnlyList<LineSideDeviceItem> MapDevices(Domain.Entities.LineEnd? lineEnd)
        {
            if (lineEnd?.Devices == null || lineEnd.Devices.Count == 0)
                return Array.Empty<LineSideDeviceItem>();

            return lineEnd.Devices
                .OrderBy(d => d.Name)
                .Select(d => new LineSideDeviceItem((int)d.DeviceId, d.Name))
                .ToList();
        }
    }
}
