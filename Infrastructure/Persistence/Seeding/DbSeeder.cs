using System.Security.Cryptography;
using System.Text;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Action = Domain.Entities.Action;
using Object = Domain.Entities.Object;

namespace Infrastructure.Persistence.Seeding;

public sealed class DbSeeder(VkrItDbContext db)
{
    private readonly VkrItDbContext _db = db;

    public async Task SeedMinimalAsync(CancellationToken ct = default)
    {
        await _db.Database.MigrateAsync(ct);

        _ = await UpsertRoleAsync("ADMIN", "Администратор", ct);
        var (dispatcherRole, isDispatcherRoleNew) = await UpsertRoleAsync("DISPATCHER", "Диспетчер", ct);

        var passwordHash = ComputeSha256("dispatcher1:password");
        var (dispatcherUser, isDispatcherUserNew) = await UpsertUserAsync(
            "dispatcher1",
            "Диспетчер 1",
            passwordHash,
            ct);

        await UpsertUserRoleAsync(dispatcherUser, dispatcherRole, isDispatcherUserNew, isDispatcherRoleNew, ct);

        var (lineType, _) = await UpsertObjectTypeAsync("LINE_500", "ВЛ 500 кВ", ct);
        await UpsertObjectAsync(lineType, "VL500_001", "ВЛ 500 кВ №1", true, ct);

        var (action, isActionNew) = await UpsertActionAsync(
            "FORM_INSTRUCTION",
            "Сформировать указание",
            "Тестовое действие для проверки пайплайна",
            ct);

        await UpsertActionVersionAsync(action, isActionNew, "v1", true, DateTimeOffset.UtcNow, ct);

        await _db.SaveChangesAsync(ct);
    }

    private async Task<(AppRole Role, bool IsNew)> UpsertRoleAsync(
        string code,
        string name,
        CancellationToken ct)
    {
        var role = await _db.AppRoles.FirstOrDefaultAsync(x => x.Code == code, ct);
        if (role is null)
        {
            role = new AppRole { Code = code, Name = name };
            _db.AppRoles.Add(role);
            return (role, true);
        }

        if (role.Name != name)
        {
            role.Name = name;
        }

        return (role, false);
    }

    private async Task<(AppUser User, bool IsNew)> UpsertUserAsync(
        string login,
        string name,
        string passwordHash,
        CancellationToken ct)
    {
        var user = await _db.AppUsers.FirstOrDefaultAsync(x => x.Login == login, ct);
        if (user is null)
        {
            user = new AppUser
            {
                Login = login,
                Name = name,
                PasswordHash = passwordHash
            };
            _db.AppUsers.Add(user);
            return (user, true);
        }

        if (user.Name != name)
        {
            user.Name = name;
        }

        if (user.PasswordHash != passwordHash)
        {
            user.PasswordHash = passwordHash;
        }

        return (user, false);
    }

    private async Task UpsertUserRoleAsync(
        AppUser user,
        AppRole role,
        bool isUserNew,
        bool isRoleNew,
        CancellationToken ct)
    {
        if (isUserNew || isRoleNew)
        {
            _db.UserRoles.Add(new UserRole
            {
                AppUser = user,
                AppRole = role
            });
            return;
        }

        var existing = await _db.UserRoles.FirstOrDefaultAsync(
            x => x.UserId == user.UserId && x.RoleId == role.RoleId,
            ct);

        if (existing is null)
        {
            _db.UserRoles.Add(new UserRole
            {
                UserId = user.UserId,
                RoleId = role.RoleId
            });
        }
    }

    private async Task<(ObjectType ObjectType, bool IsNew)> UpsertObjectTypeAsync(
        string code,
        string name,
        CancellationToken ct)
    {
        var objectType = await _db.ObjectTypes.FirstOrDefaultAsync(x => x.Code == code, ct);
        if (objectType is null)
        {
            objectType = new ObjectType { Code = code, Name = name };
            _db.ObjectTypes.Add(objectType);
            return (objectType, true);
        }

        if (objectType.Name != name)
        {
            objectType.Name = name;
        }

        return (objectType, false);
    }

    private async Task<Object> UpsertObjectAsync(
        ObjectType objectType,
        string uid,
        string dispatchName,
        bool isActive,
        CancellationToken ct)
    {
        var entity = await _db.Objects.FirstOrDefaultAsync(x => x.Uid == uid, ct);
        if (entity is null)
        {
            entity = new Object
            {
                Uid = uid,
                DispatchName = dispatchName,
                IsActive = isActive,
                ObjectType = objectType
            };
            _db.Objects.Add(entity);
            return entity;
        }

        if (entity.DispatchName != dispatchName)
        {
            entity.DispatchName = dispatchName;
        }

        if (entity.IsActive != isActive)
        {
            entity.IsActive = isActive;
        }

        if (entity.ObjectTypeId != objectType.ObjectTypeId)
        {
            entity.ObjectType = objectType;
        }

        return entity;
    }

    private async Task<(Action Action, bool IsNew)> UpsertActionAsync(
        string code,
        string name,
        string description,
        CancellationToken ct)
    {
        var action = await _db.Actions.FirstOrDefaultAsync(x => x.Code == code, ct);
        if (action is null)
        {
            action = new Action
            {
                Code = code,
                Name = name,
                Description = description
            };
            _db.Actions.Add(action);
            return (action, true);
        }

        if (action.Name != name)
        {
            action.Name = name;
        }

        if (action.Description != description)
        {
            action.Description = description;
        }

        return (action, false);
    }

    private async Task UpsertActionVersionAsync(
        Action action,
        bool isActionNew,
        string versionLabel,
        bool isActive,
        DateTimeOffset releasedAt,
        CancellationToken ct)
    {
        if (isActionNew)
        {
            _db.ActionVersions.Add(new ActionVersion
            {
                Action = action,
                VersionLabel = versionLabel,
                IsActive = isActive,
                ReleasedAt = releasedAt
            });
            return;
        }

        var version = await _db.ActionVersions.FirstOrDefaultAsync(
            x => x.ActionId == action.ActionId && x.VersionLabel == versionLabel,
            ct);

        if (version is null)
        {
            _db.ActionVersions.Add(new ActionVersion
            {
                Action = action,
                VersionLabel = versionLabel,
                IsActive = isActive,
                ReleasedAt = releasedAt
            });
            return;
        }

        if (version.IsActive != isActive)
        {
            version.IsActive = isActive;
        }

        if (version.ReleasedAt == default)
        {
            version.ReleasedAt = releasedAt;
        }
    }

    private static string ComputeSha256(string input)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        var hash = SHA256.HashData(bytes);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }
}