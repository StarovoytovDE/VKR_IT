using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ApplicationLayer.InstructionGeneration.DeviceParams;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.InstructionGeneration.DeviceParams;

/// <summary>
/// EF Core-реализация читателя параметров устройства.
/// Делает отдельные запросы по 1:1 таблицам функций и отдельный запрос по VT,
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

        // CT place (ожидаем 1 запись на устройство в вашей текущей модели данных)
        var ctPlace = await _db.CtPlaces
            .AsNoTracking()
            .SingleAsync(x => x.DeviceId == deviceId, ct);

        // VT pair (ожидаем 2 записи: Main=true и Main=false)
        var vts = await _db.Vts
            .AsNoTracking()
            .Where(x => x.DeviceId == deviceId)
            .ToListAsync(ct);

        var vtPair = MapAndValidateVts(deviceId, vts);

        // Функции (1:1 таблицы)
        var dfz = await _db.Dfzs
            .AsNoTracking()
            .SingleAsync(x => x.DeviceId == deviceId, ct);

        var dzl = await _db.Dzls
            .AsNoTracking()
            .SingleAsync(x => x.DeviceId == deviceId, ct);

        var dz = await _db.Dzs
            .AsNoTracking()
            .SingleAsync(x => x.DeviceId == deviceId, ct);

        // Для ОАПВ/ТАПВ «наличие» выражено фактом записи (пока нет HazOapv/HazTapv).
        var oapv = await _db.Oapvs
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.DeviceId == deviceId, ct);

        var tapv = await _db.Tapvs
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.DeviceId == deviceId, ct);

        return new DeviceParamsSnapshot
        {
            DeviceId = device.DeviceId,
            ObjectId = device.ObjectId,
            DeviceName = device.Name,
            VtSwitchTrue = device.VtSwitchTrue,

            CtPlace = new CtPlaceSnapshot
            {
                Name = ctPlace.Name,
                Place = ctPlace.Place,
                PlaceCode = ctPlace.PlaceCode
            },

            Vts = vtPair,

            // ПАСПОРТ (наличие) + состояние в БД (State)
            Dfz = new FunctionStateSnapshot { Has = dfz.HazDfz, State = dfz.State },
            Dzl = new FunctionStateSnapshot { Has = dzl.HazDzl, State = dzl.State },
            Dz = new FunctionStateSnapshot { Has = dz.HazDz, State = dz.State },

            Oapv = new OapvStateSnapshot
            {
                State = new FunctionStateSnapshot
                {
                    Has = oapv is not null,
                    State = oapv?.State ?? false
                },
                SwitchOff = oapv?.SwitchOff ?? false
            },

            Tapv = new FunctionStateSnapshot
            {
                Has = tapv is not null,
                State = tapv?.State ?? false
            },

            // Технологические флаги (пока не читаем из БД — позже появится UI управления объектами)
            IsFieldClosingAllowed = false,
            NeedDisableUpaskReceivers = false,
            NeedDisconnectLineCTFromDzo = false,
            NeedMtzoShinovkaAtoB = false
        };
    }

    /// <summary>
    /// Преобразует список VT в пару (Main/Reserve) и валидирует бизнес-правило:
    /// ровно 2 VT, один основной (Main=true), второй резервный (Main=false).
    /// </summary>
    private static VtPairSnapshot MapAndValidateVts(long deviceId, List<Vt> vts)
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
            Main = new VtSnapshot
            {
                Name = main.Name,
                Place = main.Place,
                PlaceCode = main.PlaceCode
            },
            Reserve = new VtSnapshot
            {
                Name = reserve.Name,
                Place = reserve.Place,
                PlaceCode = reserve.PlaceCode
            }
        };
    }
}
