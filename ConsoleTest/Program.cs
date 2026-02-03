using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Models;
using ApplicationLayer.InstructionGeneration.Operations;
using ApplicationLayer.InstructionGeneration.Requests;
using Infrastructure.InstructionGeneration.Services;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

internal static class Program
{
    /// <summary>
    /// Точка входа консольного приложения для пакетной генерации указаний.
    /// Сценарий:
    /// 1) Запросить у пользователя ПС (dispatch_name из таблицы substation).
    /// 2) Запросить действие (4 варианта).
    /// 3) Найти все устройства device, относящиеся к данной ПС через связь:
    ///    device.line_end_id -> line_end.substation_id.
    /// 4) Для каждого device сформировать и вывести операции.
    /// 5) В конце ожидать нажатия клавиши, очистить консоль и повторить.
    /// </summary>
    private static async Task Main()
    {
        // Важно: строка подключения оставлена как в текущем ConsoleTest.
        // При необходимости вынеси в конфиг/ENV, но по задаче это не требуется.
        var connectionString =
            "Host=localhost;Port=5432;Database=vkr_it;Username=vkr_it_app;Password=VKRitAPP12345671";

        var options = new DbContextOptionsBuilder<VkrItDbContext>()
            .UseNpgsql(connectionString, npgsql => npgsql.MigrationsAssembly("Infrastructure"))
            .UseSnakeCaseNamingConvention()
            .Options;

        // Фабрика нужна для корректного чтения snapshot (EfCoreDeviceParamsReader создаёт новый DbContext на вызов).
        IDbContextFactory<VkrItDbContext> dbFactory = new PooledDbContextFactory<VkrItDbContext>(options);

        // DbContext для выборок (список устройств/сторона линии). Используем один на итерацию цикла.
        await using var db = new VkrItDbContext(options);

        // ===== Инициализация пайплайна генерации (один раз на процесс) =====

        var reader = new EfCoreDeviceParamsReader(dbFactory);
        var criteriaBuilder = new LineOperationCriteriaBuilder();

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

        // ===== Основной интерактивный цикл =====

        while (true)
        {
            Console.WriteLine("Введите ПС");
            var substationDispatchName = (Console.ReadLine() ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(substationDispatchName))
            {
                Console.WriteLine("ПС не введена. Повторите ввод.");
                Console.WriteLine();
                continue;
            }

            var actionCode = ReadActionFromUser();
            Console.WriteLine();

            var deviceIds = await GetDeviceIdsBySubstationAsync(
                db,
                substationDispatchName,
                maxDevices: int.MaxValue);

            if (deviceIds.Count == 0)
            {
                Console.WriteLine($"Устройства не найдены для ПС '{substationDispatchName}'. " +
                                  "Проверь точное совпадение dispatch_name в таблице substation.");
                Console.WriteLine();
                Console.WriteLine("Нажмите любую клавишу для сброса");
                Console.ReadKey(true);
                Console.Clear();
                continue;
            }

            Console.WriteLine($"ПС: '{substationDispatchName}'");
            Console.WriteLine($"Действие: {GetActionDisplayName(actionCode)}");
            Console.WriteLine($"Найдено устройств: {deviceIds.Count}");
            Console.WriteLine();

            foreach (var deviceId in deviceIds)
            {
                Console.WriteLine(new string('=', 90));

                // 1) snapshot
                var snapshot = await reader.ReadAsync(deviceId, CancellationToken.None);

                // 2) определить сторону A/B по LineEnd.SideCode (если определить не удалось — A)
                var side = await ResolveSideOfLineAsync(db, snapshot.LineEndId);

                Console.WriteLine($"Device: id={snapshot.DeviceId}, lineEndId={snapshot.LineEndId}, name='{snapshot.DeviceName}', side={side}");
                Console.WriteLine();

                // 3) request (то, что задаёт диспетчер)
                var request = new LineOperationRequest
                {
                    // Пока остаётся тестовый код линии (как в текущей реализации ConsoleTest).
                    // Если нужно — можем позже подтянуть LineCode из БД через line_end -> object.
                    LineCode = "VL-500-01",
                    Side = side,
                    ActionCode = actionCode,
                    FunctionStates = new FunctionStatesRequest
                    {
                        // В консольном сценарии без чекбоксов считаем, что функции "разрешены" к обработке.
                        DfzEnabled = true,
                        DzlEnabled = true,
                        DzEnabled = true,
                        OapvEnabled = true,
                        TapvEnabled = true
                    }
                };

                // 4) criteria
                var criteria = criteriaBuilder.Build(request, deviceObjectId: (int)deviceId, snapshot);

                // 5) generate
                var instructions = generator.Generate(criteria);

                // 6) print
                PrintResult(instructions);
                Console.WriteLine();
            }

            Console.WriteLine("Нажмите любую клавишу для сброса");
            Console.ReadKey(true);
            Console.Clear();
        }
    }

    /// <summary>
    /// Читает выбранное действие от пользователя и возвращает соответствующий ActionCode.
    /// </summary>
    private static ActionCode ReadActionFromUser()
    {
        while (true)
        {
            Console.WriteLine("Выбирите действие:");
            Console.WriteLine("1 - Вывод ВЛ с замыканием поля");
            Console.WriteLine("2 - Вывод ВЛ без замыкания поля");
            Console.WriteLine("3 - Односторонний вывод ВЛ");

            var raw = (Console.ReadLine() ?? string.Empty).Trim();

            if (raw == "1") return ActionCode.LineWithdrawalWithFieldClosing;
            if (raw == "2") return ActionCode.LineWithdrawalWithoutFieldClosing;
            if (raw == "3") return ActionCode.LineSingleSideWithdrawal;

            Console.WriteLine("Некорректный выбор. Введите число 1..4.");
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Возвращает человекочитаемое имя действия для вывода в консоль.
    /// </summary>
    private static string GetActionDisplayName(ActionCode action)
    {
        return action switch
        {
            ActionCode.LineWithdrawalWithFieldClosing => "Вывод ВЛ с замыканием поля",
            ActionCode.LineWithdrawalWithoutFieldClosing => "Вывод ВЛ без замыкания поля",
            ActionCode.LineSingleSideWithdrawal => "Односторонний вывод ВЛ",
            _ => action.ToString()
        };
    }

    /// <summary>
    /// Возвращает идентификаторы устройств, относящихся к подстанции (по dispatch_name).
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
