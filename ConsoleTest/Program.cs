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
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

internal static class Program
{
    /// <summary>
    /// Точка входа консольного приложения.
    /// Сценарий работы:
    /// 1) Выбор линии из БД (object.dispatch_name).
    /// 2) Выбор ПС по концам линии (line_end -> substation).
    /// 3) Выбор действия (ActionCode).
    /// 4) Вывод указаний по всем устройствам на выбранной ПС выбранной линии.
    /// После вывода: ожидание клавиши и возврат к выбору линии.
    ///
    /// Важно: DbContext создаётся через фабрику как НОВЫЙ экземпляр на каждую операцию.
    /// Это предотвращает ObjectDisposedException, возникающий при повторном использовании уже disposed-контекста.
    /// </summary>
    private static async Task Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // Подключение к БД — оставлено как в текущем ConsoleTest (при необходимости вынеси в конфиг).
        var connectionString =
            "Host=localhost;Port=5432;Database=vkr_it;Username=vkr_it_app;Password=VKRitAPP12345671";

        var options = new DbContextOptionsBuilder<VkrItDbContext>()
            .UseNpgsql(connectionString, npgsql => npgsql.MigrationsAssembly("Infrastructure"))
            .UseSnakeCaseNamingConvention()
            .Options;

        IDbContextFactory<VkrItDbContext> dbFactory = new NewDbContextFactory(options);

        // Reader для снимков устройства (DeviceParamsSnapshot).
        var reader = new EfCoreDeviceParamsReader(dbFactory);

        // Пайплайн генерации.
        var criteriaBuilder = new LineOperationCriteriaBuilder();
        var generator = BuildGenerator();

        while (true)
        {
            Console.Clear();

            // 1) Выбор линии
            var line = await SelectLineAsync(dbFactory);
            if (line is null)
            {
                Console.WriteLine("Линии в таблице object не найдены.");
                Console.WriteLine("Нажмите любую клавишу для выхода...");
                Console.ReadKey(true);
                return;
            }

            Console.Clear();

            // 2) Выбор ПС (из line_end по выбранной линии)
            var lineEnds = await GetLineEndsAsync(dbFactory, line.ObjectId);
            if (lineEnds.Count == 0)
            {
                Console.WriteLine($"Для линии '{line.DispatchName}' (object_id={line.ObjectId}) не найдены концы line_end.");
                Console.WriteLine("Нажмите любую клавишу, чтобы вернуться к выбору линии...");
                Console.ReadKey(true);
                continue;
            }

            var chosenEnd = SelectSubstation(line.DispatchName, lineEnds);
            if (chosenEnd is null)
                continue;

            Console.Clear();

            // 3) Выбор действия
            var action = SelectAction(line.DispatchName, chosenEnd.SubstationDispatchName);

            Console.Clear();

            // 4) Генерация по всем устройствам на выбранной ПС данной линии
            var deviceIds = await GetDeviceIdsByLineEndAsync(dbFactory, chosenEnd.LineEndId);

            Console.WriteLine($"Линия: {line.DispatchName} (object_id={line.ObjectId})");
            Console.WriteLine($"ПС: {chosenEnd.SubstationDispatchName} (substation_id={chosenEnd.SubstationId}), side={chosenEnd.Side}");
            Console.WriteLine($"Действие: {GetActionDisplayName(action)}");
            Console.WriteLine();

            if (deviceIds.Count == 0)
            {
                Console.WriteLine("Устройства для выбранной ПС на данной линии не найдены.");
            }
            else
            {
                foreach (var deviceId in deviceIds)
                {
                    Console.WriteLine(new string('=', 90));

                    // Важно: reader сам создаёт DbContext через фабрику и корректно его освобождает.
                    var snapshot = await reader.ReadAsync(deviceId, CancellationToken.None);

                    var request = new LineOperationRequest
                    {
                        LineCode = line.DispatchName,
                        Side = chosenEnd.Side,
                        ActionCode = action,
                        FunctionStates = new FunctionStatesRequest
                        {
                            // В консольном сценарии считаем, что диспетчер «ввёл всё включено».
                            // (Если позже понадобится ввод галочками — добавим.)
                            DfzEnabled = true,
                            DzlEnabled = true,
                            DzEnabled = true,
                            OapvEnabled = true,
                            TapvEnabled = true
                        }
                    };

                    var criteria = criteriaBuilder.Build(request, deviceObjectId: (int)deviceId, snapshot);
                    var instructions = generator.Generate(criteria);

                    Console.WriteLine($"Устройство: id={snapshot.DeviceId}, lineEndId={snapshot.LineEndId}, name='{snapshot.DeviceName}'");
                    PrintResult(instructions);
                    Console.WriteLine();
                }
            }

            Console.WriteLine();
            Console.WriteLine("Нажмите любую клавишу, чтобы вернуться к выбору линии...");
            Console.ReadKey(true);
        }
    }

    /// <summary>
    /// Создаёт генератор указаний со всеми операциями и реестром ActionCode → операции.
    /// </summary>
    private static InstructionGenerator BuildGenerator()
    {
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
        return new InstructionGenerator(registry);
    }

    /// <summary>
    /// Предлагает выбрать линию из таблицы object (поле dispatch_name).
    /// </summary>
    private static async Task<LineItem?> SelectLineAsync(IDbContextFactory<VkrItDbContext> dbFactory)
    {
        Console.WriteLine("Выберите линию:");

        await using var db = dbFactory.CreateDbContext();

        var lines = await db.Objects
            .AsNoTracking()
            .OrderBy(x => x.DispatchName)
            .Select(x => new LineItem(x.ObjectId, x.DispatchName))
            .ToListAsync();

        if (lines.Count == 0)
            return null;

        for (var i = 0; i < lines.Count; i++)
            Console.WriteLine($"{i + 1}) {lines[i].DispatchName}");

        var index = ReadMenuIndex(lines.Count);
        return lines[index];
    }

    /// <summary>
    /// Возвращает концы линии (line_end) с подстанциями (substation) для выбранной линии (object_id).
    /// </summary>
    private static async Task<IReadOnlyList<LineEndItem>> GetLineEndsAsync(
        IDbContextFactory<VkrItDbContext> dbFactory,
        long lineObjectId)
    {
        await using var db = dbFactory.CreateDbContext();

        // Берём line_end по object_id, подтягиваем substation, сортируем по side_code (A/B/...) для стабильного вывода.
        var ends = await db.LineEnds
            .AsNoTracking()
            .Where(x => x.ObjectId == lineObjectId)
            .Include(x => x.Substation)
            .OrderBy(x => x.SideCode)
            .Select(x => new
            {
                x.LineEndId,
                x.SubstationId,
                SubstationName = x.Substation.DispatchName,
                x.SideCode
            })
            .ToListAsync();

        // Маппинг side_code -> SideOfLine (A/B). Если в БД что-то отличное — считаем A по умолчанию.
        return ends
            .Select(x => new LineEndItem(
                LineEndId: x.LineEndId,
                SubstationId: x.SubstationId,
                SubstationDispatchName: x.SubstationName,
                Side: MapSide(x.SideCode)))
            .ToList();
    }

    /// <summary>
    /// Выводит список ПС для выбранной линии и возвращает выбранный конец линии (line_end).
    /// </summary>
    private static LineEndItem? SelectSubstation(string lineName, IReadOnlyList<LineEndItem> lineEnds)
    {
        Console.WriteLine($"Линия: {lineName}");
        Console.WriteLine();
        Console.WriteLine("Выберите ПС:");

        // На всякий случай: если в line_end несколько записей на одну ПС, показываем их как отдельные варианты (по LineEndId/side).
        for (var i = 0; i < lineEnds.Count; i++)
        {
            var e = lineEnds[i];
            Console.WriteLine($"{i + 1}) {e.SubstationDispatchName} (side={e.Side}, line_end_id={e.LineEndId})");
        }

        var index = ReadMenuIndex(lineEnds.Count);
        return lineEnds[index];
    }

    /// <summary>
    /// Выводит меню действий и возвращает выбранный ActionCode.
    /// </summary>
    private static ActionCode SelectAction(string lineName, string substationName)
    {
        Console.WriteLine($"Линия: {lineName}");
        Console.WriteLine($"ПС: {substationName}");
        Console.WriteLine();
        Console.WriteLine("Выберите действие:");

        var actions = new[]
        {
            ActionCode.LineWithdrawalWithFieldClosing,
            ActionCode.LineWithdrawalWithoutFieldClosing,
            ActionCode.LineSingleSideWithdrawal
        };

        for (var i = 0; i < actions.Length; i++)
            Console.WriteLine($"{i + 1}) {GetActionDisplayName(actions[i])}");

        var index = ReadMenuIndex(actions.Length);
        return actions[index];
    }

    /// <summary>
    /// Возвращает список device_id устройств, относящихся к выбранному концу линии (line_end_id).
    /// </summary>
    private static async Task<IReadOnlyList<long>> GetDeviceIdsByLineEndAsync(
        IDbContextFactory<VkrItDbContext> dbFactory,
        long lineEndId)
    {
        await using var db = dbFactory.CreateDbContext();

        return await db.Devices
            .AsNoTracking()
            .Where(d => d.LineEndId == lineEndId)
            .OrderBy(d => d.DeviceId)
            .Select(d => d.DeviceId)
            .ToListAsync();
    }

    /// <summary>
    /// Читает номер пункта меню (1..max) и возвращает индекс (0..max-1).
    /// </summary>
    private static int ReadMenuIndex(int max)
    {
        while (true)
        {
            Console.Write("Введите номер: ");
            var raw = Console.ReadLine();

            if (int.TryParse(raw, NumberStyles.Integer, CultureInfo.InvariantCulture, out var n) &&
                n >= 1 && n <= max)
            {
                return n - 1;
            }

            Console.WriteLine($"Некорректный ввод. Ожидается число от 1 до {max}.");
        }
    }

    /// <summary>
    /// Маппит строковый side_code (из line_end) в SideOfLine.
    /// </summary>
    private static SideOfLine MapSide(string? sideCode)
    {
        if (string.Equals(sideCode, "B", StringComparison.OrdinalIgnoreCase))
            return SideOfLine.B;

        return SideOfLine.A;
    }

    /// <summary>
    /// Возвращает человекочитаемое название действия (ActionCode) для вывода в консоль.
    /// </summary>
    private static string GetActionDisplayName(ActionCode code)
    {
        return code switch
        {
            ActionCode.LineWithdrawalWithFieldClosing => "Вывод ВЛ с замыканием поля",
            ActionCode.LineWithdrawalWithoutFieldClosing => "Вывод ВЛ без замыкания поля",
            ActionCode.LineSingleSideWithdrawal => "Односторонний вывод ВЛ",
            _ => code.ToString()
        };
    }

    /// <summary>
    /// Печатает сгенерированные указания (пропуская пустые строки).
    /// </summary>
    private static void PrintResult(IReadOnlyList<string> instructions)
    {
        if (instructions is null || instructions.Count == 0)
        {
            Console.WriteLine("(нет указаний)");
            return;
        }

        foreach (var s in instructions)
        {
            if (!string.IsNullOrWhiteSpace(s))
                Console.WriteLine($"- {s}");
        }
    }

    /// <summary>
    /// Фабрика DbContext, создающая НОВЫЙ экземпляр VkrItDbContext при каждом вызове CreateDbContext().
    /// Нужна, чтобы исключить повторное использование уже disposed-контекста.
    /// </summary>
    private sealed class NewDbContextFactory : IDbContextFactory<VkrItDbContext>
    {
        private readonly DbContextOptions<VkrItDbContext> _options;

        /// <summary>
        /// Создаёт фабрику контекста на основе заранее подготовленных DbContextOptions.
        /// </summary>
        public NewDbContextFactory(DbContextOptions<VkrItDbContext> options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary>
        /// Создаёт новый экземпляр VkrItDbContext.
        /// </summary>
        public VkrItDbContext CreateDbContext()
        {
            return new VkrItDbContext(_options);
        }
    }

    /// <summary>
    /// Элемент списка линий (object).
    /// </summary>
    private sealed record LineItem(long ObjectId, string DispatchName);

    /// <summary>
    /// Элемент выбора ПС на линии (line_end + substation + side).
    /// </summary>
    private sealed record LineEndItem(long LineEndId, long SubstationId, string SubstationDispatchName, SideOfLine Side);
}
