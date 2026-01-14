using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Operations.DecisionTree;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Типовые узлы дерева решений для семейства ТАПВ.
/// </summary>
public static class TapvNodes
{
    /// <summary>
    /// Типовой каркас для ТАПВ:
    /// HasTAPV? -> TAPVEnabled? -> (ветка whenEnabled) / null
    /// </summary>
    public static Node<LineOperationCriteria> HasAndEnabled(Node<LineOperationCriteria> whenEnabled)
        => Node<LineOperationCriteria>.Decision(
            predicate: c => c.HasTAPV,
            whenTrue: Node<LineOperationCriteria>.Decision(
                predicate: c => c.TAPVEnabled,
                whenTrue: whenEnabled,
                whenFalse: Node<LineOperationCriteria>.Action(null)
            ),
            whenFalse: Node<LineOperationCriteria>.Action(null)
        );

    /// <summary>
    /// Текущая принятая формулировка вывода ТАПВ — "вывести функцию ТАПВ".
    /// </summary>
    public static Node<LineOperationCriteria> WithdrawFunction()
        => Node<LineOperationCriteria>.Action(InstructionTexts.WithdrawFunction(FunctionNames.TAPV));

    /// <summary>
    /// На будущее (если решите применять правило "вывести устройство/функцию" и для ТАПВ).
    /// </summary>
    public static Node<LineOperationCriteria> WithdrawByOnlyFunctionRule()
        => OperationNodes.WithdrawByOnlyFunctionRule(FunctionNames.TAPV);
}
