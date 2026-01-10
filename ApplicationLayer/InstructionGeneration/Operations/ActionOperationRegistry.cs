using System;
using System.Collections.Generic;
using System.Linq;
using ApplicationLayer.InstructionGeneration.Models;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Реестр соответствия: ActionCode → набор операций.
/// Операции индексируются по IOperation.Code, а конфигурация хранит массивы кодов.
/// </summary>
public sealed class ActionOperationRegistry : IActionOperationRegistry
{
    private readonly IReadOnlyDictionary<string, IOperation> _operationByCode;
    private readonly IReadOnlyDictionary<ActionCode, string[]> _actionToOperationCodes;

    /// <summary>
    /// Инициализирует реестр.
    /// </summary>
    /// <param name="operations">Зарегистрированные операции.</param>
    public ActionOperationRegistry(IEnumerable<IOperation> operations)
    {
        ArgumentNullException.ThrowIfNull(operations);

        _operationByCode = operations.ToDictionary(
            op => op.Code,
            op => op,
            StringComparer.Ordinal);

        _actionToOperationCodes = new Dictionary<ActionCode, string[]>
        {
            [ActionCode.LineWithdrawalWithFieldClosing] = new[]
            {
                OperationCodes.DfzFieldClosing,
                OperationCodes.DzlFieldClosing
            },

            //[ActionCode.LineWithdrawalWithoutFieldClosing] = new[]
            //{
            //    // Здесь позже добавишь DfzNoFieldClosingOperation, когда реализуешь
            //    OperationCodes.DzlNoFieldClosing
            //},

            // Заготовки под прочие сценарии:
            [ActionCode.LineWithdrawalWithBusSideDisconnector] = Array.Empty<string>(),
            [ActionCode.LineSingleSideWithdrawal] = Array.Empty<string>()
        };

        ValidateConfiguration();
    }

    /// <inheritdoc />
    public IReadOnlyList<IOperation> GetOperations(ActionCode actionCode)
    {
        if (!_actionToOperationCodes.TryGetValue(actionCode, out var codes) || codes.Length == 0)
            return Array.Empty<IOperation>();

        var result = new List<IOperation>(codes.Length);

        foreach (var code in codes)
            result.Add(_operationByCode[code]);

        return result;
    }

    private void ValidateConfiguration()
    {
        foreach (var (action, opCodes) in _actionToOperationCodes)
        {
            foreach (var code in opCodes)
            {
                if (!_operationByCode.ContainsKey(code))
                {
                    throw new InvalidOperationException(
                        $"Операция с Code='{code}' не зарегистрирована, но используется в ActionCode='{action}'.");
                }
            }
        }
    }
}
