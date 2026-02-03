using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Models;
using ApplicationLayer.InstructionGeneration.Operations;
using ApplicationLayer.InstructionGeneration.Requests;
using Domain.Entities;
using Domain.ReferenceData;
using Infrastructure.InstructionGeneration.Services;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

internal static class Program
{
    /// <summary>
    /// Точка входа консольного теста генератора указаний.
    /// Демонстрация «правильного» пайплайна под сценарий «Вывод ВЛ с замыканием поля».
    ///
    /// Поддерживает пакетную генерацию:
    /// - по подстанции (переменная окружения VKR_IT_SUBSTATION = DispatchName ПС);
    /// - либо по списку deviceId (VKR_IT_DEVICE_IDS="1,2,3");
    /// - либо по умолчанию deviceId=1.
    ///
    /// Дополнительно:
    /// - VKR_IT_MAX_DEVICES=3 (ограничение количества устройств при выборе по ПС).
    /// - VKR_IT_CONSOLETEST_SEED=1 (явный сидинг тестовых данных).
    /// </summary>
    private static async Task Main()
    {
        Console.WriteLine("=== ConsoleTest: Batch instruction generation ===");

        var connectionString =
            "Host=localhost;Port=5432;Database=vkr_it;Username=vkr_it_app;Password=VKRitAPP12345671";

        var optionsBuilder = new DbContextOptionsBuilder<VkrItDbContext>()
            .UseNpgsql(connectionString, npgsql => npgsql.MigrationsAssembly("Infrastructure"))
            .UseSnakeCaseNamingConvention();

        // Контекст нужен для миграций/сидирования и для выборки списка устройств.
        await using var db = new VkrItDbContext(optionsBuilder.Options);

        // Фабрика нужна для «правильного» чтения snapshot (EfCoreDeviceParamsReader).
        IDbContextFactory<VkrItDbContext> dbFactory =
            new PooledDbContextFactory<VkrItDbContext>(optionsBuilder.Options);

        await db.Database.MigrateAsync();

        // ВАЖНО:
        // По умолчанию ConsoleTest НЕ должен сидировать и НЕ должен изменять БД,
        // иначе вы получаете «почему snapshot не подтягивает изменения» — изменения просто затираются сидом.
        //
        // Чтобы явно сидировать тестовые данные, установите переменную окружения:
        // VKR_IT_CONSOLETEST_SEED=1
        var shouldSeed = string.Equals(
            Environment.GetEnvironmentVariable("VKR_IT_CONSOLETEST_SEED"),
            "1",
            StringComparison.Ordinal);

        // ===== 1) Определяем список устройств для обработки =====

        // ЗАДАЁШЬ ПС ЗДЕСЬ:
        const string targetSubstationName = "ПС 500 кВ Восход";

        // Сколько устройств выводить (если нужно ограничить).
        const int maxDevices = 5;

        var deviceIds = await GetDeviceIdsBySubstationAsync(db, targetSubstationName, maxDevices);
        Console.WriteLine($"Mode: by substation '{targetSubstationName}', devices={deviceIds.Count}");

        if (deviceIds.Count == 0)
        {
            Console.WriteLine($"Устройства не найдены для ПС '{targetSubstationName}'. Проверь точное совпадение DispatchName в БД.");
            return;
        }

        if (deviceIds.Count == 0)
        {
            Console.WriteLine("Устройства не найдены. Проверьте VKR_IT_SUBSTATION / VKR_IT_DEVICE_IDS.");
            return;
        }

        // ===== 2) Инициализируем «пайплайн генерации» один раз =====

        // Reader: читает агрегированный снимок параметров устройства из БД.
        var reader = new EfCoreDeviceParamsReader(dbFactory);

        // Criteria builder: строит LineOperationCriteria из запроса диспетчера + snapshot.
        var criteriaBuilder = new LineOperationCriteriaBuilder();

        // Реестр операций.
        var operations = new IOperation[]
        {
            new DfzFieldClosingOperation(),
            new DzlFieldClosingOperation(),
            new DzFieldClosingOperation(),
            new OapvOperation(),
            new TapvOperation(),
            new UpaskReceiversWithdrawalOperation(),

            // Универсальная операция перевода цепей напряжения:
            new VtVoltageCircuitsTransferOperation(),

            new DisconnectLineCtFromDzoOperation(),
            new MtzoShinovkaAtoBOperation(),

            new DfzNoFieldClosingOperation(),
            new DzNoFieldClosingOperation(),
            new OapvNoFieldClosingOperation(),
            new TapvNoFieldClosingOperation(),

            new DfzSingleSideWithdrawalOperation(),
        };

        IActionOperationRegistry registry = new ActionOperationRegistry(operations);
        var generator = new InstructionGenerator(registry);

        // ===== 3) Генерируем для каждого устройства подряд =====
        Console.WriteLine();
        Console.WriteLine("=== Generate (batch) ===");

        foreach (var deviceId in deviceIds)
        {
            Console.WriteLine();
            Console.WriteLine(new string('=', 90));

            // 1) snapshot
            var snapshot = await reader.ReadAsync(deviceId, CancellationToken.None);

            // Попробуем корректно определить сторону A/B по LineEnd.SideCode (если есть).
            var side = await ResolveSideOfLineAsync(db, snapshot.LineEndId);

            Console.WriteLine($"Device: id={snapshot.DeviceId}, lineEndId={snapshot.LineEndId}, name='{snapshot.DeviceName}', side={side}");

            // 2) request (то, что задаёт диспетчер через UI)
            var request = new LineOperationRequest
            {
                LineCode = "VL-500-01",
                Side = side,
                ActionCode = ActionCode.LineWithdrawalWithFieldClosing,
                FunctionStates = new FunctionStatesRequest
                {
                    DfzEnabled = true,
                    DzlEnabled = true,
                    DzEnabled = true,
                    OapvEnabled = true,
                    TapvEnabled = true
                }
            };

            // 3) criteria
            var criteria = criteriaBuilder.Build(request, deviceObjectId: (int)deviceId, snapshot);

            // 4) generate
            var instructions = generator.Generate(criteria);

            // 5) print
            PrintResult(instructions);
        }

        Console.WriteLine();
        Console.WriteLine("=== End ===");
        Console.ReadKey();
    }

    /// <summary>
    /// Возвращает идентификаторы устройств, относящихся к подстанции (по DispatchName).
    /// Связь: device.line_end_id -> line_end.substation_id.
    /// </summary>
    private static async Task<IReadOnlyList<long>> GetDeviceIdsBySubstationAsync(
        VkrItDbContext db,
        string substationDispatchName,
        int maxDevices)
    {
        var substationId = await db.Substations
            .AsNoTracking()
            .Where(s => s.DispatchName == substationDispatchName)
            .Select(s => s.SubstationId)
            .FirstOrDefaultAsync();

        if (substationId == 0)
            return Array.Empty<long>();

        // join Devices -> LineEnds, фильтр по SubstationId
        var ids = await db.Devices
            .AsNoTracking()
            .Join(
                db.LineEnds.AsNoTracking(),
                d => d.LineEndId,
                le => le.LineEndId,
                (d, le) => new { d.DeviceId, le.SubstationId })
            .Where(x => x.SubstationId == substationId)
            .OrderBy(x => x.DeviceId)
            .Select(x => x.DeviceId)
            .Take(maxDevices)
            .ToListAsync();

        return ids;
    }

    /// <summary>
    /// Определяет сторону линии (A/B) по LineEnd.SideCode.
    /// Если определить не удалось — возвращает A.
    /// </summary>
    private static async Task<SideOfLine> ResolveSideOfLineAsync(VkrItDbContext db, long lineEndId)
    {
        var sideCode = await db.LineEnds
            .AsNoTracking()
            .Where(x => x.LineEndId == lineEndId)
            .Select(x => x.SideCode)
            .FirstOrDefaultAsync();

        if (string.Equals(sideCode, "B", StringComparison.OrdinalIgnoreCase))
            return SideOfLine.B;

        return SideOfLine.A;
    }

    /// <summary>
    /// Читает int из переменной окружения, иначе возвращает defaultValue.
    /// </summary>
    private static int ReadIntFromEnv(string envName, int defaultValue)
    {
        var raw = Environment.GetEnvironmentVariable(envName);
        if (string.IsNullOrWhiteSpace(raw))
            return defaultValue;

        return int.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value)
            ? value
            : defaultValue;
    }

    /// <summary>
    /// Читает список deviceId из переменной окружения VKR_IT_DEVICE_IDS формата "1,2,3".
    /// Некорректные значения игнорируются.
    /// </summary>
    private static IReadOnlyList<long> ReadDeviceIdsFromEnv(string envName)
    {
        var raw = Environment.GetEnvironmentVariable(envName);
        if (string.IsNullOrWhiteSpace(raw))
            return Array.Empty<long>();

        var ids = new List<long>();

        foreach (var token in raw.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            if (long.TryParse(token, NumberStyles.Integer, CultureInfo.InvariantCulture, out var id) && id > 0)
                ids.Add(id);
        }

        return ids;
    }

    /// <summary>
    /// Печатает список сформированных указаний в консоль.
    /// </summary>
    private static void PrintResult(IReadOnlyList<string> instructions)
    {
        if (instructions.Count == 0)
        {
            Console.WriteLine("Результат: операций не требуется");
            return;
        }

        Console.WriteLine("Результат:");
        foreach (var instruction in instructions)
            Console.WriteLine($" - {instruction}");
    }
}
