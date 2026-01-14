using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Operations.DecisionTree;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Типовые узлы дерева решений для семейства ОАПВ.
/// </summary>
public static class OapvNodes
{
    /// <summary>
    /// Типовой каркас для ОАПВ:
    /// HasOAPV? -> OAPVEnabled? -> (ветка whenEnabled) / null
    /// </summary>
    public static Node<LineOperationCriteria> HasAndEnabled(Node<LineOperationCriteria> whenEnabled)
        => Node<LineOperationCriteria>.Decision(
            predicate: c => c.HasOAPV,
            whenTrue: Node<LineOperationCriteria>.Decision(
                predicate: c => c.OAPVEnabled,
                whenTrue: whenEnabled,
                whenFalse: Node<LineOperationCriteria>.Action(null)
            ),
            whenFalse: Node<LineOperationCriteria>.Action(null)
        );

    /// <summary>
    /// Текущая принятая формулировка вывода ОАПВ — "вывести функцию ОАПВ".
    /// </summary>
    public static Node<LineOperationCriteria> WithdrawFunction()
        => Node<LineOperationCriteria>.Action(InstructionTexts.WithdrawFunction(FunctionNames.OAPV));

    /// <summary>
    /// На будущее (если решите применять правило "вывести устройство/функцию" и для ОАПВ).
    /// Сейчас не используется в текущих деревьях, но готово для расширения.
    /// </summary>
    public static Node<LineOperationCriteria> WithdrawByOnlyFunctionRule()
        => OperationNodes.WithdrawByOnlyFunctionRule(FunctionNames.OAPV);
}
