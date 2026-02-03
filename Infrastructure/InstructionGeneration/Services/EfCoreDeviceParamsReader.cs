using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationLayer.InstructionGeneration.DeviceParams;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.InstructionGeneration.Services;

/// <summary>
/// EF Core реализация чтения параметров устройства (DeviceParamsSnapshot).
/// Для WinForms используется IDbContextFactory, чтобы исключить конкурентное использование одного DbContext.
/// </summary>
public sealed class EfCoreDeviceParamsReader : IDeviceParamsReader
{
    private readonly IDbContextFactory<VkrItDbContext> _dbFactory;

    /// <summary>
    /// Создаёт reader.
    /// </summary>
    /// <param name="dbFactory">Фабрика DbContext (создаёт новый контекст на каждый вызов).</param>
    public EfCoreDeviceParamsReader(IDbContextFactory<VkrItDbContext> dbFactory)
    {
        _dbFactory = dbFactory ?? throw new ArgumentNullException(nameof(dbFactory));
    }

    /// <inheritdoc />
    public async Task<DeviceParamsSnapshot> ReadAsync(long deviceId, CancellationToken ct)
    {
        await using var db = await _dbFactory.CreateDbContextAsync(ct);

        // Device содержит два FK на vt: MainVtId и ReserveVtId, поэтому можно одним запросом подтянуть оба ТН.
        var device = await db.Devices
            .AsNoTracking()
            .Include(x => x.MainVt)
            .Include(x => x.ReserveVt)
            .SingleAsync(x => x.DeviceId == deviceId, ct);

        var ctPlace = await db.CtPlaces
            .AsNoTracking()
            .Where(x => x.DeviceId == deviceId)
            .OrderByDescending(x => x.CtPlaceId)
            .FirstOrDefaultAsync(ct);

        // Наличие функций определяется наличием записей в таблицах функций.
        var dfz = await db.Dfzs
            .AsNoTracking()
            .Where(x => x.DeviceId == deviceId)
            .ToListAsync(ct);

        var dzl = await db.Dzls
            .AsNoTracking()
            .Where(x => x.DeviceId == deviceId)
            .ToListAsync(ct);

        var dz = await db.Dzs
            .AsNoTracking()
            .Where(x => x.DeviceId == deviceId)
            .ToListAsync(ct);

        var oapv = await db.Oapvs
            .AsNoTracking()
            .Where(x => x.DeviceId == deviceId)
            .ToListAsync(ct);

        var tapv = await db.Tapvs
            .AsNoTracking()
            .Where(x => x.DeviceId == deviceId)
            .ToListAsync(ct);

        return new DeviceParamsSnapshot
        {
            DeviceId = device.DeviceId,
            LineEndId = device.LineEndId,
            DeviceName = device.Name,

            VtSwitchTrue = device.VtSwitchTrue,

            CtPlace = new CtPlaceSnapshot
            {
                Name = ctPlace?.Name ?? string.Empty,
                Place = ctPlace?.Place ?? string.Empty,
                PlaceCode = ctPlace?.PlaceCode ?? string.Empty
            },

            Vts = new VtPairSnapshot
            {
                Main = new VtSnapshot
                {
                    Name = device.MainVt?.Name ?? string.Empty,
                    Place = device.MainVt?.Place ?? string.Empty,
                    PlaceCode = device.MainVt?.PlaceCode ?? string.Empty
                },
                Reserve = new VtSnapshot
                {
                    Name = device.ReserveVt?.Name ?? string.Empty,
                    Place = device.ReserveVt?.Place ?? string.Empty,
                    PlaceCode = device.ReserveVt?.PlaceCode ?? string.Empty
                }
            },

            Dfz = new FunctionStateSnapshot
            {
                Has = dfz.Count > 0,
                State = dfz.Any(x => x.State)
            },
            Dzl = new FunctionStateSnapshot
            {
                Has = dzl.Count > 0,
                State = dzl.Any(x => x.State)
            },
            Dz = new FunctionStateSnapshot
            {
                Has = dz.Count > 0,
                State = dz.Any(x => x.State)
            },

            Oapv = new ApvStateSnapshot
            {
                State = new FunctionStateSnapshot
                {
                    Has = oapv.Count > 0,
                    State = oapv.Any(x => x.State)
                },
                SwitchOff = oapv.Any(x => x.SwitchOff)
            },

            Tapv = new ApvStateSnapshot
            {
                State = new FunctionStateSnapshot
                {
                    Has = tapv.Count > 0,
                    State = tapv.Any(x => x.State)
                },
                SwitchOff = tapv.Any(x => x.SwitchOff)
            },

            IsFieldClosingAllowed = device.FieldClosingAllowed,
            NeedDisableUpaskReceivers = device.UpaskSwitchTrue,
            NeedDisconnectLineCTFromDzo = device.DzoSwitchTrue,
            CtRemainsEnergizedOnThisSide = device.CtRemainsEnergized
        };
    }
}
