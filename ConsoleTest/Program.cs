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

        if (shouldSeed)
        {
            _ = await EnsureMinimalDeviceWithFunctionsAsync(db);

            // После сидирования/нормализации очищаем ChangeTracker,
            // чтобы дальнейшее чтение не вернуло «старую» tracked-сущность.
            db.ChangeTracker.Clear();
        }

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
    /// Гарантирует наличие минимального набора данных, достаточного для чтения DeviceParamsSnapshot.
    /// ВНИМАНИЕ: метод используется ТОЛЬКО при VKR_IT_CONSOLETEST_SEED=1.
    /// При повторном запуске нормализует place/place_code к каноническому виду,
    /// чтобы place всегда оставался русским (UI/вывод), а place_code — кодом (алгоритмы).
    /// </summary>
    private static async Task<long> EnsureMinimalDeviceWithFunctionsAsync(VkrItDbContext db)
    {
        var lineType = await db.ObjectTypes.FirstOrDefaultAsync(x => x.Code == "LINE");
        if (lineType is null)
        {
            lineType = new ObjectType { Code = "LINE", Name = "Линия" };
            db.ObjectTypes.Add(lineType);
            await db.SaveChangesAsync();
        }

        var substation = await db.Substations.FirstOrDefaultAsync(x => x.DispatchName == "ПС 500 кВ Тестовая");
        if (substation is null)
        {
            substation = new Substation { DispatchName = "ПС 500 кВ Тестовая" };
            db.Substations.Add(substation);
            await db.SaveChangesAsync();
        }

        var obj = await db.Objects.FirstOrDefaultAsync(x => x.Uid == "VL500_001");
        if (obj is null)
        {
            obj = new ObjectTable
            {
                ObjectTypeId = lineType.ObjectTypeId,
                Uid = "VL500_001",
                DispatchName = "ВЛ 500 кВ №1",
                IsActive = true,
            };
            db.Objects.Add(obj);
            await db.SaveChangesAsync();
        }

        // Актуальная БД: device.line_end_id -> line_end.line_end_id
        // Поэтому сначала гарантируем наличие LineEnd.
        var lineEnd = await db.LineEnds.FirstOrDefaultAsync(x =>
            x.ObjectId == obj.ObjectId &&
            x.SubstationId == substation.SubstationId &&
            x.SideCode == "A");

        if (lineEnd is null)
        {
            lineEnd = new LineEnd
            {
                ObjectId = obj.ObjectId,
                SubstationId = substation.SubstationId,
                SideCode = "A",
            };

            db.LineEnds.Add(lineEnd);
            await db.SaveChangesAsync();
        }

        // Пытаемся найти устройство уже по корректному line_end_id
        var device = await db.Devices.FirstOrDefaultAsync(x =>
            x.LineEndId == lineEnd.LineEndId &&
            x.Name == "Устройство РЗА 1");

        // Если ранее ConsoleTest создавал устройство с LineEndId=obj.ObjectId (устаревшая схема),
        // то аккуратно «переедем» на актуальный line_end_id.
        if (device is null)
        {
            var legacyDevice = await db.Devices.FirstOrDefaultAsync(x =>
                x.LineEndId == obj.ObjectId &&
                x.Name == "Устройство РЗА 1");

            if (legacyDevice is not null)
            {
                legacyDevice.LineEndId = lineEnd.LineEndId;
                await db.SaveChangesAsync();
                device = legacyDevice;
            }
        }

        if (device is null)
        {
            device = new Device
            {
                LineEndId = lineEnd.LineEndId,
                Name = "Устройство РЗА 1",

                // Технологические параметры устройства (как в целевой архитектуре).
                // Здесь задаём значения только для первичного наполнения.
                VtSwitchTrue = true,
                DzoSwitchTrue = true,
                UpaskSwitchTrue = true,
                FieldClosingAllowed = true,

                // Новый флаг — задаём при создании (не трогаем существующие значения при повторных запусках).
                CtRemainsEnergized = false
            };

            db.Devices.Add(device);
            await db.SaveChangesAsync();
        }

        // CT place: "Сумма токов выключателей линии" -> CT_SUM_BREAKERS
        var ctPlace = await db.CtPlaces.FirstOrDefaultAsync(x => x.DeviceId == device.DeviceId);
        if (ctPlace is null)
        {
            ctPlace = new CtPlace
            {
                DeviceId = device.DeviceId,
                Name = "ТТ (место подключения)",
                Place = "Сумма токов выключателей линии",
                PlaceCode = PlaceCodes.Ct.SumBreakers
            };
            db.CtPlaces.Add(ctPlace);
            await db.SaveChangesAsync();
        }
        else
        {
            var changed = false;

            if (!string.Equals(ctPlace.Place, "Сумма токов выключателей линии", StringComparison.Ordinal))
            {
                ctPlace.Place = "Сумма токов выключателей линии";
                changed = true;
            }

            if (!string.Equals(ctPlace.PlaceCode, PlaceCodes.Ct.SumBreakers, StringComparison.Ordinal))
            {
                ctPlace.PlaceCode = PlaceCodes.Ct.SumBreakers;
                changed = true;
            }

            if (changed)
                await db.SaveChangesAsync();
        }

        // VT: основной = линейный, резервный = шинный
        var vtMain = await db.Vts.FirstOrDefaultAsync(x => x.DeviceId == device.DeviceId && x.Main);
        var vtReserve = await db.Vts.FirstOrDefaultAsync(x => x.DeviceId == device.DeviceId && !x.Main);

        if (vtMain is null)
        {
            vtMain = new Vt
            {
                DeviceId = device.DeviceId,
                Main = true,
                Name = "Main1",
                Place = "Линейный ТН",
                PlaceCode = PlaceCodes.Vt.Line
            };
            db.Vts.Add(vtMain);
            await db.SaveChangesAsync();
        }

        if (vtReserve is null)
        {
            vtReserve = new Vt
            {
                DeviceId = device.DeviceId,
                Main = false,
                Name = "NotMain1",
                Place = "Шинный ТН",
                PlaceCode = PlaceCodes.Vt.Bus
            };
            db.Vts.Add(vtReserve);
            await db.SaveChangesAsync();
        }

        // ДФЗ/ДЗЛ/ДЗ (минимально)
        var dfz = await db.Dfzs.FirstOrDefaultAsync(x => x.DeviceId == device.DeviceId && x.Code == "DFZ");
        if (dfz is null)
        {
            dfz = new Dfz { DeviceId = device.DeviceId, Code = "DFZ", Name = "ДФЗ", HazDfz = true, State = true };
            db.Dfzs.Add(dfz);
            await db.SaveChangesAsync();
        }

        var dzl = await db.Dzls.FirstOrDefaultAsync(x => x.DeviceId == device.DeviceId && x.Code == "DZL");
        if (dzl is null)
        {
            dzl = new Dzl { DeviceId = device.DeviceId, Code = "DZL", Name = "ДЗЛ", HazDzl = true, State = true };
            db.Dzls.Add(dzl);
            await db.SaveChangesAsync();
        }

        var dz = await db.Dzs.FirstOrDefaultAsync(x => x.DeviceId == device.DeviceId && x.Code == "DZ");
        if (dz is null)
        {
            dz = new Dz { DeviceId = device.DeviceId, Code = "DZ", Name = "ДЗ", HazDz = true, State = true };
            db.Dzs.Add(dz);
            await db.SaveChangesAsync();
        }

        var oapv = await db.Oapvs.FirstOrDefaultAsync(x => x.DeviceId == device.DeviceId && x.Code == "OAPV");
        if (oapv is null)
        {
            oapv = new Oapv { DeviceId = device.DeviceId, Code = "OAPV", Name = "ОАПВ", SwitchOff = false, State = true };
            db.Oapvs.Add(oapv);
            await db.SaveChangesAsync();
        }

        var tapv = await db.Tapvs.FirstOrDefaultAsync(x => x.DeviceId == device.DeviceId && x.Code == "TAPV");
        if (tapv is null)
        {
            tapv = new Tapv { DeviceId = device.DeviceId, Code = "TAPV", Name = "ТАПВ", SwitchOff = false, State = true };
            db.Tapvs.Add(tapv);
            await db.SaveChangesAsync();
        }

        // МТЗ ошиновки (опционально)
        var mtzBusbar = await db.MtzBusbars.FirstOrDefaultAsync(x => x.DeviceId == device.DeviceId);
        if (mtzBusbar is null)
        {
            mtzBusbar = new MtzBusbar { DeviceId = device.DeviceId, Code = "MTZ_BUS", Name = "МТЗ ошиновки", State = true };
            db.MtzBusbars.Add(mtzBusbar);
            await db.SaveChangesAsync();
        }

        return device.DeviceId;
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
