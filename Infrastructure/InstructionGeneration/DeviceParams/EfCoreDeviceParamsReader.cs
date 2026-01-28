using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationLayer.InstructionGeneration.DeviceParams;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.InstructionGeneration.DeviceParams;

/// <summary>
/// EF Core реализация чтения параметров устройства (DeviceParamsSnapshot).
/// </summary>
public sealed class EfCoreDeviceParamsReader : IDeviceParamsReader
{
    private readonly VkrItDbContext _db;

    /// <summary>
    /// Создаёт reader.
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
            .Where(x => x.DeviceId == deviceId)
            .OrderByDescending(x => x.CtPlaceId)
            .FirstOrDefaultAsync(ct);

        var vts = await _db.Vts
            .AsNoTracking()
            .Where(x => x.DeviceId == deviceId)
            .ToListAsync(ct);

        var mainVt = vts.SingleOrDefault(x => x.Main);
        var reserveVt = vts.SingleOrDefault(x => !x.Main);

        var dfz = await _db.Dfzs.AsNoTracking().Where(x => x.DeviceId == deviceId).ToListAsync(ct);
        var dzl = await _db.Dzls.AsNoTracking().Where(x => x.DeviceId == deviceId).ToListAsync(ct);
        var dz = await _db.Dzs.AsNoTracking().Where(x => x.DeviceId == deviceId).ToListAsync(ct);
        var oapv = await _db.Oapvs.AsNoTracking().Where(x => x.DeviceId == deviceId).ToListAsync(ct);
        var tapv = await _db.Tapvs.AsNoTracking().Where(x => x.DeviceId == deviceId).ToListAsync(ct);

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
                    Name = mainVt?.Name ?? string.Empty,
                    Place = mainVt?.Place ?? string.Empty,
                    PlaceCode = mainVt?.PlaceCode ?? string.Empty
                },
                Reserve = new VtSnapshot
                {
                    Name = reserveVt?.Name ?? string.Empty,
                    Place = reserveVt?.Place ?? string.Empty,
                    PlaceCode = reserveVt?.PlaceCode ?? string.Empty
                }
            },

            Dfz = new FunctionStateSnapshot
            {
                Has = dfz.Any(x => x.HazDfz),
                State = dfz.Any(x => x.State)
            },
            Dzl = new FunctionStateSnapshot
            {
                Has = dzl.Any(x => x.HazDzl),
                State = dzl.Any(x => x.State)
            },
            Dz = new FunctionStateSnapshot
            {
                Has = dz.Any(x => x.HazDz),
                State = dz.Any(x => x.State)
            },

            Oapv = new ApvStateSnapshot
            {
                State = new FunctionStateSnapshot
                {
                    State = oapv.Any(x => x.State)
                },
                SwitchOff = oapv.Any(x => x.SwitchOff)
            },

            Tapv = new ApvStateSnapshot
            {
                State = new FunctionStateSnapshot
                {
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
