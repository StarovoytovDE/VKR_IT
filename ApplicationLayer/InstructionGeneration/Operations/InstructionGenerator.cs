using System;
using System.Collections.Generic;
using ApplicationLayer.InstructionGeneration.Criteria;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Генератор указаний: берёт ActionCode из criteria, выбирает набор операций,
/// прогоняет алгоритмы и возвращает итоговые указания.
/// </summary>
public sealed class InstructionGenerator(IActionOperationRegistry registry)
{
    private readonly IActionOperationRegistry _registry = registry ?? throw new ArgumentNullException(nameof(registry));

    public IReadOnlyList<string> Generate(LineOperationCriteria criteria)
    {
        ArgumentNullException.ThrowIfNull(criteria);

        var operations = _registry.GetOperations(criteria.ActionCode);

        var results = new List<string>();

        foreach (var op in operations)
        {
            var output = op.Evaluate(criteria);
            if (!string.IsNullOrWhiteSpace(output))
                results.Add(output);
        }

        return results;
    }
}
