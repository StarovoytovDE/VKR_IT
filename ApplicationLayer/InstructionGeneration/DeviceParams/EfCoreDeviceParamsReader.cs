using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationLayer.InstructionGeneration.DeviceParams;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.InstructionGeneration.DeviceParams;

/// <summary>
/// EF Core-реализация читателя параметров устройства.
/// Делает отдельные запросы по «1:1» таблицам и отдельный запрос по VT,
/// проверяя, что VT ровно два: один Main=true, второй Main=false.
/// </summary>
public sealed class EfCoreDeviceParamsReader : IDeviceParamsReader
{
    private readonly VkrItDbContext _db;

    /// <summary>
    /// Создаёт читателя параметров устройства на базе EF Core контекста.
    /// </summary>
    public EfCoreDeviceParamsReader(VkrItDbContext db)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
    }

    /// <inheritdoc />
    public async Task<DeviceParamsSnapshot> ReadAsync(long deviceId, CancellationToken ct)
    {
        var device = await _db.Devices
            .AsNoTracking()
            .SingleAsync(x => x.DeviceId == deviceId, ct);

        var ctPlace = await _db.CtPlaces
            .AsNoTracking()
            .SingleAsync(x => x.DeviceId == deviceId, ct);

        var vts = await _db.Vts
            .AsNoTracking()
            .Where(x => x.DeviceId == deviceId)
            .ToListAsync(ct);

        var vtPair = MapAndValidateVts(deviceId, vts);

        var dfz = await _db.Dfzs
            .AsNoTracking()
            .SingleAsync(x => x.DeviceId == deviceId, ct);

        var dzl = await _db.Dzls
            .AsNoTracking()
            .SingleAsync(x => x.DeviceId == deviceId, ct);

        var dz = await _db.Dzs
            .AsNoTracking()
            .SingleAsync(x => x.DeviceId == deviceId, ct);

        var oapv = await _db.Oapvs
            .AsNoTracking()
            .SingleAsync(x => x.DeviceId == deviceId, ct);

        var tapv = await _db.Tapvs
            .AsNoTracking()
            .SingleAsync(x => x.DeviceId == deviceId, ct);

        return new DeviceParamsSnapshot
        {
            DeviceId = device.DeviceId,
            ObjectId = device.ObjectId,
            DeviceName = device.Name,
            VtSwitchTrue = device.VtSwitchTrue,

            CtPlace = new CtPlaceSnapshot
            {
                Name = ctPlace.Name,
                Place = ctPlace.Place
            },

            Vts = vtPair,

            Dfz = new FunctionStateSnapshot { Has = dfz.HazDfz, Enabled = dfz.State },
            Dzl = new FunctionStateSnapshot { Has = dzl.HazDzl, Enabled = dzl.State },
            Dz = new FunctionStateSnapshot { Has = dz.HazDz, Enabled = dz.State },

            Oapv = new OapvStateSnapshot
            {
                Has = true, // у ОАПВ в твоей модели «наличие» обычно выражено фактом записи; если у тебя есть HazOapv — поменяй сюда
                Enabled = oapv.State,
                SwitchOff = oapv.SwitchOff
            },

            Tapv = new FunctionStateSnapshot
            {
                Has = true, // аналогично: если у Tapv есть HazTapv — используй его
                Enabled = tapv.State
            }
        };
    }

    /// <summary>
    /// Преобразует список VT в пару (Main/Reserve) и валидирует бизнес-правило:
    /// ровно 2 VT, один основной (Main=true), второй резервный (Main=false).
    /// </summary>
    private static VtPairSnapshot MapAndValidateVts(long deviceId, System.Collections.Generic.List<Domain.Entities.Vt> vts)
    {
        if (vts.Count != 2)
        {
            throw new InvalidOperationException(
                $"Для устройства deviceId={deviceId} ожидается ровно 2 записи VT (основной и резервный), фактически: {vts.Count}.");
        }

        var main = vts.SingleOrDefault(x => x.Main);
        var reserve = vts.SingleOrDefault(x => !x.Main);

        if (main is null || reserve is null)
        {
            throw new InvalidOperationException(
                $"Для устройства deviceId={deviceId} VT должны содержать одну запись Main=true и одну запись Main=false.");
        }

        return new VtPairSnapshot
        {
            Main = new VtSnapshot { Name = main.Name, Place = main.Place },
            Reserve = new VtSnapshot { Name = reserve.Name, Place = reserve.Place }
        };
    }
}
