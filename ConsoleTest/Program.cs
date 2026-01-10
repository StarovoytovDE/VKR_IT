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
        //    Создаём и регистрируем ВСЕ операции, которые потенциально используются
        //    в конфигурации ActionOperationRegistry (ActionCode -> OperationCodes[]).
        //
        // Важно:
        // - если в реестре упомянут код операции, но сама операция не зарегистрирована здесь,
        //   реестр выбросит исключение при ValidateConfiguration().
        var operations = new IOperation[]
        {
            // ДФЗ: для сценария "вывод линии с замыканием поля"
            new DfzFieldClosingOperation(),

            // ДЗЛ: отдельные операции (коды различаются), даже если логика пока одинаковая
            new DzlFieldClosingOperation(),
        };

        // 2) Реестр соответствия ActionCode -> операции
        IActionOperationRegistry registry = new ActionOperationRegistry(operations);

        // 3) Генератор указаний: выбирает операции по ActionCode и выполняет их
        var generator = new InstructionGenerator(registry);

        // 4) Критерии для одного устройства и одной стороны линии.
        //    Сейчас критерии задаются вручную для тестирования.
        //
        // Важно:
        // - ActionCode определяет, какие операции будут запущены (через реестр).
        // - Остальные поля определяют, что вернут алгоритмы операций.
        var criteria = new LineOperationCriteria(
            lineCode: "VL-500-01",
            side: SideOfLine.A,
            deviceObjectId: 1,
            actionCode: ActionCode.LineWithdrawalWithFieldClosing)
        {
            // ===== Параметры ДФЗ =====
            HasDFZ = true,
            DFZEnabled = true,
            DFZConnectedToLineCT = true,
            DFZConnectedToLineVT = true,
            NeedSwitchLineVTToReserve = false,

            // ===== Параметры ДЗЛ =====
            HasDZL = true,
            DZLEnabled = true,

            IsOnlyFunctionInDevice = false
        };

        // 5) Генерация указаний
        var instructions = generator.Generate(criteria);

        // 6) Вывод результата
        if (instructions.Count == 0)
        {
            Console.WriteLine("Результат: операций не требуется");
        }
        else
        {
            Console.WriteLine("Результат:");
            foreach (var instruction in instructions)
            {
                Console.WriteLine($" - {instruction}");
            }
        }

        Console.WriteLine("=== End ===");
        Console.ReadKey();
    }
}
