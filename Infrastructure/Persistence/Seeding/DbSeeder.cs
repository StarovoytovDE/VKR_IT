using System.Security.Cryptography;
using System.Text;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Seeding;

/// <summary>
/// Первичное наполнение БД минимальными справочниками для проверки пайплайна.
/// </summary>
public sealed class DbSeeder
{
    private readonly VkrItDbContext _db;

    /// <summary>
    /// Инициализирует сидер.
    /// </summary>
    public DbSeeder(VkrItDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Применяет миграции и заполняет минимальные данные.
    /// </summary>
    public async Task SeedMinimalAsync(CancellationToken ct = default)
    {
        await _db.Database.MigrateAsync(ct);

        var (adminRole, _) = await UpsertRoleAsync("ADMIN", "Администратор", ct);
        var (dispatcherRole, _) = await UpsertRoleAsync("DISPATCHER", "Диспетчер", ct);

        var passwordHash = ComputeSha256("dispatcher1:password");
        var (dispatcherUser, _) = await UpsertUserAsync("dispatcher1", "Диспетчер 1", passwordHash, ct);

        await UpsertUserRoleAsync(dispatcherUser, adminRole, ct);
        await UpsertUserRoleAsync(dispatcherUser, dispatcherRole, ct);

        var (lineType, _) = await UpsertObjectTypeAsync("LINE", "Линия", ct);

        var substation = await UpsertSubstationAsync("ПС 500 кВ Тестовая", ct);

        var lineObject = await UpsertObjectAsync(
            objectType: lineType,
            uid: "VL500_001",
            dispatchName: "ВЛ 500 кВ №1",
            isActive: true,
            substationId: substation.SubstationId,
            ct);

        _ = await UpsertActionAsync("FORM_INSTRUCTION", "Сформировать указание", "Тестовое действие", ct);

        var device = await UpsertDeviceAsync(lineObject.ObjectId, "Устройство РЗА 1", vtSwitchTrue: false, ct);

        // ВНИМАНИЕ: методы ниже синхронные (они просто добавляют сущности в ChangeTracker),
        // поэтому здесь НЕЛЬЗЯ использовать await.
        _ = UpsertVt(device.DeviceId, main: true, name: "ТН1", place: "Линейный ТН");
        _ = UpsertVt(device.DeviceId, main: false, name: "ТН2", place: "Резервный шинный ТН");

        _ = UpsertCtPlace(device.DeviceId, name: "ТТ на сумму токов", place: "Сумма токов ТТ выключателей ВЛ");

        _ = UpsertDfz(device.DeviceId, code: "DFZ", name: "ДФЗ", haz: true, state: true);
        _ = UpsertDzl(device.DeviceId, code: "DZL", name: "ДЗЛ", haz: true, state: true);
        _ = UpsertDz(device.DeviceId, code: "DZ", name: "ДЗ", haz: true, state: true);
        _ = UpsertOapv(device.DeviceId, code: "OAPV", name: "ОАПВ", switchOff: false, state: true);
        _ = UpsertTapv(device.DeviceId, code: "TAPV", name: "ТАПВ", switchOff: false, state: true);
        _ = UpsertMtzBusbar(device.DeviceId, code: "MTZ_BUS", name: "МТЗ ошиновки", aToBTrue: true, state: true);


        await _db.SaveChangesAsync(ct);
    }

    private async Task<(AppRole Role, bool IsNew)> UpsertRoleAsync(string code, string name, CancellationToken ct)
    {
        var role = await _db.AppRoles.FirstOrDefaultAsync(x => x.Code == code, ct);
        if (role is null)
        {
            role = new AppRole { Code = code, Name = name };
            _db.AppRoles.Add(role);
            return (role, true);
        }

        role.Name = name;
        return (role, false);
    }

    private async Task<(AppUser User, bool IsNew)> UpsertUserAsync(string login, string name, string passwordHash, CancellationToken ct)
    {
        var user = await _db.AppUsers.FirstOrDefaultAsync(x => x.Login == login, ct);
        if (user is null)
        {
            user = new AppUser { Login = login, Name = name, PasswordHash = passwordHash };
            _db.AppUsers.Add(user);
            return (user, true);
        }

        user.Name = name;
        user.PasswordHash = passwordHash;
        return (user, false);
    }

    private async Task UpsertUserRoleAsync(AppUser user, AppRole role, CancellationToken ct)
    {
        var existing = await _db.UserRoles.FirstOrDefaultAsync(x => x.UserId == user.UserId && x.RoleId == role.RoleId, ct);
        if (existing is null)
        {
            _db.UserRoles.Add(new UserRole { UserId = user.UserId, RoleId = role.RoleId });
        }
    }

    private async Task<(ObjectType ObjectType, bool IsNew)> UpsertObjectTypeAsync(string code, string name, CancellationToken ct)
    {
        var objectType = await _db.ObjectTypes.FirstOrDefaultAsync(x => x.Code == code, ct);
        if (objectType is null)
        {
            objectType = new ObjectType { Code = code, Name = name };
            _db.ObjectTypes.Add(objectType);
            return (objectType, true);
        }

        objectType.Name = name;
        return (objectType, false);
    }

    private async Task<Substation> UpsertSubstationAsync(string dispatchName, CancellationToken ct)
    {
        var s = await _db.Substations.FirstOrDefaultAsync(x => x.DispatchName == dispatchName, ct);
        if (s is null)
        {
            s = new Substation { DispatchName = dispatchName };
            _db.Substations.Add(s);
        }

        return s;
    }

    private async Task<ObjectTable> UpsertObjectAsync(
        ObjectType objectType,
        string uid,
        string dispatchName,
        bool isActive,
        long substationId,
        CancellationToken ct)
    {
        var entity = await _db.Objects.FirstOrDefaultAsync(x => x.Uid == uid, ct);
        if (entity is null)
        {
            entity = new ObjectTable
            {
                Uid = uid,
                DispatchName = dispatchName,
                IsActive = isActive,
                ObjectType = objectType,
                SubstationId = substationId
            };
            _db.Objects.Add(entity);
            return entity;
        }

        entity.DispatchName = dispatchName;
        entity.IsActive = isActive;
        entity.ObjectTypeId = objectType.ObjectTypeId;
        entity.SubstationId = substationId;

        return entity;
    }

    private async Task<ActionTable> UpsertActionAsync(string code, string name, string description, CancellationToken ct)
    {
        var action = await _db.Actions.FirstOrDefaultAsync(x => x.Code == code, ct);
        if (action is null)
        {
            action = new ActionTable { Code = code, Name = name, Description = description };
            _db.Actions.Add(action);
            return action;
        }

        action.Name = name;
        action.Description = description;
        return action;
    }

    private async Task<Device> UpsertDeviceAsync(long objectId, string name, bool vtSwitchTrue, CancellationToken ct)
    {
        var d = await _db.Devices.FirstOrDefaultAsync(x => x.ObjectId == objectId && x.Name == name, ct);
        if (d is null)
        {
            d = new Device { ObjectId = objectId, Name = name, VtSwitchTrue = vtSwitchTrue };
            _db.Devices.Add(d);
        }
        else
        {
            d.VtSwitchTrue = vtSwitchTrue;
        }

        return d;
    }

    /// <summary>
    /// Добавляет запись VT (ТН) в контекст.
    /// </summary>
    private Vt UpsertVt(long deviceId, bool main, string name, string place)
    {
        var vt = new Vt { DeviceId = deviceId, Main = main, Name = name, Place = place };
        _db.Vts.Add(vt);
        return vt;
    }

    /// <summary>
    /// Добавляет запись места подключения ТТ в контекст.
    /// </summary>
    private CtPlace UpsertCtPlace(long deviceId, string name, string place)
    {
        var ctPlace = new CtPlace { DeviceId = deviceId, Name = name, Place = place };
        _db.CtPlaces.Add(ctPlace);
        return ctPlace;
    }

    /// <summary>
    /// Добавляет запись ДФЗ в контекст.
    /// </summary>
    private Dfz UpsertDfz(long deviceId, string code, string name, bool haz, bool state)
    {
        var x = new Dfz { DeviceId = deviceId, Code = code, Name = name, HazDfz = haz, State = state };
        _db.Dfzs.Add(x);
        return x;
    }

    /// <summary>
    /// Добавляет запись ДЗЛ в контекст.
    /// </summary>
    private Dzl UpsertDzl(long deviceId, string code, string name, bool haz, bool state)
    {
        var x = new Dzl { DeviceId = deviceId, Code = code, Name = name, HazDzl = haz, State = state };
        _db.Dzls.Add(x);
        return x;
    }

    /// <summary>
    /// Добавляет запись ДЗ в контекст.
    /// </summary>
    private Dz UpsertDz(long deviceId, string code, string name, bool haz, bool state)
    {
        var x = new Dz { DeviceId = deviceId, Code = code, Name = name, HazDz = haz, State = state };
        _db.Dzs.Add(x);
        return x;
    }

    /// <summary>
    /// Добавляет запись ОАПВ в контекст.
    /// </summary>
    private Oapv UpsertOapv(long deviceId, string code, string name, bool switchOff, bool state)
    {
        var x = new Oapv { DeviceId = deviceId, Code = code, Name = name, SwitchOff = switchOff, State = state };
        _db.Oapvs.Add(x);
        return x;
    }

    /// <summary>
    /// Добавляет запись ТАПВ в контекст.
    /// </summary>
    private Tapv UpsertTapv(long deviceId, string code, string name, bool switchOff, bool state)
    {
        var x = new Tapv { DeviceId = deviceId, Code = code, Name = name, SwitchOff = switchOff, State = state };
        _db.Tapvs.Add(x);
        return x;
    }

    /// <summary>
    /// Добавляет запись МТЗ ошиновки в контекст.
    /// </summary>
    private MtzBusbar UpsertMtzBusbar(long deviceId, string code, string name, bool aToBTrue, bool state)
    {
        var x = new MtzBusbar
        {
            DeviceId = deviceId,
            Code = code,
            Name = name,
            State = state
        };

        _db.MtzBusbars.Add(x);
        return x;
    }


    private static string ComputeSha256(string input)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        var hash = SHA256.HashData(bytes);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }
}
