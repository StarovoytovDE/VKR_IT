using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

/// <summary>
/// Контекст EF Core для БД проекта ВКР ИТ.
/// </summary>
public sealed class VkrItDbContext : DbContext
{
    /// <summary>
    /// Инициализирует контекст.
    /// </summary>
    public VkrItDbContext(DbContextOptions<VkrItDbContext> options) : base(options)
    {
    }

    /// <summary>Пользователи.</summary>
    public DbSet<AppUser> AppUsers => Set<AppUser>();

    /// <summary>Роли.</summary>
    public DbSet<AppRole> AppRoles => Set<AppRole>();

    /// <summary>Связи пользователей и ролей.</summary>
    public DbSet<UserRole> UserRoles => Set<UserRole>();

    /// <summary>Типы объектов.</summary>
    public DbSet<ObjectType> ObjectTypes => Set<ObjectType>();

    /// <summary>Подстанции.</summary>
    public DbSet<Substation> Substations => Set<Substation>();

    /// <summary>Объекты (линии).</summary>
    public DbSet<ObjectTable> Objects => Set<ObjectTable>();

    /// <summary>Концы линий (привязка линии к ПС по сторонам A/B).</summary>
    public DbSet<LineEnd> LineEnds => Set<LineEnd>();

    /// <summary>Действия.</summary>
    public DbSet<ActionTable> Actions => Set<ActionTable>();

    /// <summary>Запросы на формирование указаний.</summary>
    public DbSet<InstructionRequest> InstructionRequests => Set<InstructionRequest>();

    /// <summary>Результаты формирования указаний.</summary>
    public DbSet<InstructionResult> InstructionResults => Set<InstructionResult>();

    /// <summary>Устройства.</summary>
    public DbSet<Device> Devices => Set<Device>();

    /// <summary>ТН (VT).</summary>
    public DbSet<Vt> Vts => Set<Vt>();

    /// <summary>Места подключения ТТ (CT place).</summary>
    public DbSet<CtPlace> CtPlaces => Set<CtPlace>();

    /// <summary>ДФЗ.</summary>
    public DbSet<Dfz> Dfzs => Set<Dfz>();

    /// <summary>ДЗЛ.</summary>
    public DbSet<Dzl> Dzls => Set<Dzl>();

    /// <summary>ДЗ.</summary>
    public DbSet<Dz> Dzs => Set<Dz>();

    /// <summary>ОАПВ.</summary>
    public DbSet<Oapv> Oapvs => Set<Oapv>();

    /// <summary>ТАПВ.</summary>
    public DbSet<Tapv> Tapvs => Set<Tapv>();

    /// <summary>МТЗ ошиновки.</summary>
    public DbSet<MtzBusbar> MtzBusbars => Set<MtzBusbar>();

    /// <summary>Привязки параметров запроса.</summary>
    public DbSet<RequestParamValue> RequestParamValues => Set<RequestParamValue>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(VkrItDbContext).Assembly);
    }
}
