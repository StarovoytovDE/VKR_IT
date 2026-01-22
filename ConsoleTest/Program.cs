using System;
using System.Linq;
using System.Threading.Tasks;
using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Models;
using ApplicationLayer.InstructionGeneration.Operations;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

internal static class Program
{
    /// <summary>
    /// Точка входа консольного теста генератора указаний.
    /// Демонстрация: читаем HazDFZ из БД (таблица dfz.haz_dfz) через EF Core и
    /// прокидываем в критерии LineOperationCriteria.HasDFZ.
    /// </summary>
    private static async Task Main()
    {
        Console.WriteLine("=== ConsoleTest: InstructionGenerator + EF Core ===");

        // ВАЖНО:
        // 1) Здесь используется реальная PostgreSQL БД из appsettings WinForms.
        // 2) Для простоты (чтобы не тащить сюда конфиги/HostBuilder) строку подключения задаём прямо тут.
        //    При желании позже вынесем в appsettings ConsoleTest и общий хост.
        var connectionString =
            "Host=localhost;Port=5432;Database=vkr_it;Username=vkr_it_app;Password=VKRitAPP12345671";

        var dbOptions = new DbContextOptionsBuilder<VkrItDbContext>()
            .UseNpgsql(connectionString, npgsql => npgsql.MigrationsAssembly("Infrastructure"))
            .UseSnakeCaseNamingConvention()
            .Options;

        await using var db = new VkrItDbContext(dbOptions);

        // Применяем миграции (БД должна быть создана/обновлена).
        await db.Database.MigrateAsync();

        // Минимальные данные (без полноценного сидера): ObjectType -> Substation -> Object -> Device -> Dfz.
        // Нужны, чтобы вставить одну запись dfz (иначе FK на device не даст).
        var deviceId = await EnsureMinimalDeviceWithSingleDfzAsync(db);

        // Читаем ДФЗ из БД (берём первую для устройства).
        // Здесь и есть "достаём параметр HazDFZ из БД через EF Core".
        var dfz = await db.Dfzs
            .AsNoTracking()
            .Where(x => x.DeviceId == deviceId)
            .OrderBy(x => x.DfzId)
            .FirstAsync();

        Console.WriteLine();
        Console.WriteLine($"DB: dfz row найден: DfzId={dfz.DfzId}, DeviceId={dfz.DeviceId}, HazDfz={dfz.HazDfz}, State={dfz.State}");

        // 1) "Простой функциональный DI" руками:
        //    ВАЖНО: здесь должны быть зарегистрированы ВСЕ операции,
        //    которые упоминаются в ActionOperationRegistry (в _actionToOperationCodes).
        var operations = new IOperation[]
        {
            // ===== Вывод ВЛ с замыканием поля =====
            new DfzFieldClosingOperation(),
            new DzlFieldClosingOperation(),
            new DzFieldClosingOperation(),
            new UpaskReceiversWithdrawalOperation(),
            new LineVtToReserveVoltageCircuitsTransferOperation(),
            new DisconnectLineCtFromDzoOperation(),
            new MtzoShinovkaAtoBOperation(),
            new OapvOperation(),
            new TapvOperation(),

            // ===== Вывод ВЛ без замыкания поля =====
            new DfzNoFieldClosingOperation(),
            new DzNoFieldClosingOperation(),
            new OapvNoFieldClosingOperation(),
            new TapvNoFieldClosingOperation(),

            // ===== LineWithdrawalWithBusSideDisconnector =====
            new BusbarVtToReserveBusVtVoltageCircuitsTransferOperation(),

            // ===== Односторонний вывод линии =====
            new DfzSingleSideWithdrawalOperation(),
        };

        // 2) Реестр соответствия ActionCode -> операции
        IActionOperationRegistry registry = new ActionOperationRegistry(operations);

        // 3) Генератор указаний
        var generator = new InstructionGenerator(registry);

        // =====================================================================
        // ТЕСТ: только один параметр HazDFZ из БД
        // =====================================================================
        Console.WriteLine();
        Console.WriteLine("=== Test: HazDFZ from DB => criteria.HasDFZ ===");

        // Минимально заполняем только то, что нужно дереву DFZ:
        // HasDFZ и DFZEnabled. Остальные параметры пока не трогаем и оставляем дефолтами (false).
        var criteria = new LineOperationCriteria(
            lineCode: "VL-500-01",
            side: SideOfLine.A,
            deviceObjectId: (int)deviceId,
            actionCode: ActionCode.LineSingleSideWithdrawal)
        {
            HasDFZ = dfz.HazDfz,
            DFZEnabled = dfz.State,

            // Для этой операции (DfzSingleSideWithdrawal) дальше проверяется DeviceConnectedToLineCT.
            // Это НЕ "параметр HazDFZ", но без него тест может вернуть null даже при HasDFZ=true.
            // Чтобы увидеть результат, выставим true.
            DeviceConnectedToLineCT = true,

            // Чтобы дерево вернуло "вывести функцию", а не "вывести устройство" — оставим false.
            IsOnlyFunctionInDevice = false
        };

        var instructions = generator.Generate(criteria);
        PrintResult(instructions);

        Console.WriteLine();
        Console.WriteLine("=== End ===");
        Console.ReadKey();
    }

    /// <summary>
    /// Гарантирует наличие минимального набора данных и одной записи dfz для демонстрации HazDFZ.
    /// Возвращает DeviceId, к которому привязана созданная/найденная Dfz.
    /// </summary>
    private static async Task<long> EnsureMinimalDeviceWithSingleDfzAsync(VkrItDbContext db)
    {
        // 1) object_type: LINE
        var lineType = await db.ObjectTypes.FirstOrDefaultAsync(x => x.Code == "LINE");
        if (lineType is null)
        {
            lineType = new ObjectType { Code = "LINE", Name = "Линия" };
            db.ObjectTypes.Add(lineType);
            await db.SaveChangesAsync();
        }

        // 2) substations: тестовая
        var substation = await db.Substations.FirstOrDefaultAsync(x => x.DispatchName == "ПС 500 кВ Тестовая");
        if (substation is null)
        {
            substation = new Substation { DispatchName = "ПС 500 кВ Тестовая" };
            db.Substations.Add(substation);
            await db.SaveChangesAsync();
        }

        // 3) object: VL500_001
        var obj = await db.Objects.FirstOrDefaultAsync(x => x.Uid == "VL500_001");
        if (obj is null)
        {
            obj = new ObjectTable
            {
                ObjectTypeId = lineType.ObjectTypeId,
                Uid = "VL500_001",
                DispatchName = "ВЛ 500 кВ №1",
                IsActive = true,
                SubstationId = substation.SubstationId
            };
            db.Objects.Add(obj);
            await db.SaveChangesAsync();
        }

        // 4) device: "Устройство РЗА 1"
        var device = await db.Devices.FirstOrDefaultAsync(x => x.ObjectId == obj.ObjectId && x.Name == "Устройство РЗА 1");
        if (device is null)
        {
            device = new Device
            {
                ObjectId = obj.ObjectId,
                Name = "Устройство РЗА 1",
                VtSwitchTrue = false
            };
            db.Devices.Add(device);
            await db.SaveChangesAsync();
        }

        // 5) dfz: одна запись
        var dfz = await db.Dfzs.FirstOrDefaultAsync(x => x.DeviceId == device.DeviceId && x.Code == "DFZ");
        if (dfz is null)
        {
            dfz = new Dfz
            {
                DeviceId = device.DeviceId,
                Code = "DFZ",
                Name = "ДФЗ",
                HazDfz = true,   // <-- это и есть HazDFZ, который читаем из БД
                State = true     // для демонстрации, что функция "введена"
            };
            db.Dfzs.Add(dfz);
            await db.SaveChangesAsync();
        }

        return device.DeviceId;
    }

    /// <summary>
    /// Печатает список сформированных указаний в консоль.
    /// </summary>
    private static void PrintResult(System.Collections.Generic.IReadOnlyList<string> instructions)
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
