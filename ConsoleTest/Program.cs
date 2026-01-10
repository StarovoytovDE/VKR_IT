using System;
using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Models;
using ApplicationLayer.InstructionGeneration.Operations;

internal static class Program
{
    /// <summary>
    /// Точка входа консольного теста генератора указаний.
    /// </summary>
    private static void Main()
    {
        Console.WriteLine("=== ConsoleTest: InstructionGenerator ===");

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

        // 3) Генератор указаний: выбирает операции по ActionCode и выполняет их
        var generator = new InstructionGenerator(registry);

        // =====================================================================
        // ТЕСТ №1: LineWithdrawalWithFieldClosing
        // =====================================================================
        Console.WriteLine();
        Console.WriteLine("=== Test #1: LineWithdrawalWithFieldClosing ===");

        var criteria1 = new LineOperationCriteria(
            lineCode: "VL-500-01",
            side: SideOfLine.A,
            deviceObjectId: 1,
            actionCode: ActionCode.LineWithdrawalWithFieldClosing)
        {
            // ===== Параметры ДФЗ =====
            HasDFZ = true,
            DFZEnabled = true,
            DFZConnectedToLineVT = true,

            // ===== Параметры ДЗЛ =====
            HasDZL = true,
            DZLEnabled = true,

            // ===== Параметры ДЗ =====
            HasDZ = true,
            DZEnabled = true,
            DZConnectedToLineVT = true,

            // ===== Параметры ОАПВ =====
            HasOAPV = true,
            OAPVEnabled = true,

            // ===== Параметры ТАПВ =====
            HasTAPV = true,
            TAPVEnabled = true,

            // ===== Параметры УПАСК =====
            NeedDisableUpaskReceivers = true,

            // ===== Перевод цепей напряжения с линейного ТН на резервный =====
            // ВАЖНО: ты писал, что в некоторых операциях используешь NeedSwitchVTToReserve.
            // Поэтому, чтобы точно увидеть результат, выставляем оба флага.
            NeedSwitchFromLineVTToReserve = true,
            NeedSwitchVTToReserve = false,

            // ===== Отключение ТТ от ДЗО =====
            DeviceConnectedToLineCT = true,
            NeedDisconnectLineCTFromDZO = true,

            // ===== МТЗ ошиновки =====
            HasMtzoShinovka = true,
            CtRemainsEnergizedOnThisSide = true,

            // ===== Общие параметры =====
            IsOnlyFunctionInDevice = true
        };

        var instructions1 = generator.Generate(criteria1);
        PrintResult(instructions1);

        // =====================================================================
        // ТЕСТ №2: LineWithdrawalWithoutFieldClosing
        // =====================================================================
        Console.WriteLine();
        Console.WriteLine("=== Test #2: LineWithdrawalWithoutFieldClosing ===");

        var criteria2 = new LineOperationCriteria(
            lineCode: "VL-500-01",
            side: SideOfLine.A,
            deviceObjectId: 1,
            actionCode: ActionCode.LineWithdrawalWithoutFieldClosing)
        {
            // ===== ДФЗ без замыкания поля =====
            HasDFZ = true,
            DFZEnabled = true,


            // ===== ДЗ без замыкания поля =====
            HasDZ = true,
            DZEnabled = true,

            // ===== ОАПВ =====
            HasOAPV = true,
            OAPVEnabled = true,

            // ===== ТАПВ =====
            HasTAPV = true,
            TAPVEnabled = true,

            // ===== МТЗ ошиновки =====
            HasMtzoShinovka = true,
            CtRemainsEnergizedOnThisSide = true,

            // ===== Общие параметры =====
            IsOnlyFunctionInDevice = false,
            BothLineBreakerCTsOnSubstationSide = true,
        };

        var instructions2 = generator.Generate(criteria2);
        PrintResult(instructions2);

        // =====================================================================
        // ТЕСТ №3: LineWithdrawalWithBusSideDisconnector
        // =====================================================================
        Console.WriteLine();
        Console.WriteLine("=== Test #3: LineWithdrawalWithBusSideDisconnector ===");

        var criteria3 = new LineOperationCriteria(
            lineCode: "VL-500-01",
            side: SideOfLine.A,
            deviceObjectId: 1,
            actionCode: ActionCode.LineWithdrawalWithBusSideDisconnector)
        {
            // Для этого действия в ActionOperationRegistry подключена только операция:
            // OperationCodes.DisconnectLineCtFromDzo

            DeviceConnectedToLineCT = true,
            NeedDisconnectLineCTFromDZO = true,

            NeedSwitchFromBusVTToReserve = true,

            // ===== ОАПВ =====
            HasOAPV = true,
            OAPVEnabled = true,

            // ===== ТАПВ =====
            HasTAPV = true,
            TAPVEnabled = true,
        };

        var instructions3 = generator.Generate(criteria3);
        PrintResult(instructions3);

        // =====================================================================
        // ТЕСТ №4: LineSingleSideWithdrawal
        // =====================================================================
        Console.WriteLine();
        Console.WriteLine("=== Test #4: LineSingleSideWithdrawal ===");

        var criteria4 = new LineOperationCriteria(
            lineCode: "VL-500-01",
            side: SideOfLine.A,
            deviceObjectId: 1,
            actionCode: ActionCode.LineSingleSideWithdrawal)
        {
            HasDFZ = true,
            DFZEnabled = true,
            DeviceConnectedToLineCT = true,
            IsOnlyFunctionInDevice = false
        };
        
        var instructions4 = generator.Generate(criteria4);
        PrintResult(instructions4);

        Console.WriteLine();
        Console.WriteLine("=== End ===");
        Console.ReadKey();

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
