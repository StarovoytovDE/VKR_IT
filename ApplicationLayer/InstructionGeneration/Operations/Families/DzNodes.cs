using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Operations.DecisionTree;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Типовые узлы дерева решений для семейства ДЗ.
/// </summary>
public static class DzNodes
{
    /// <summary>
    /// Типовой каркас для ДЗ:
    /// HasDZ? -> DZEnabled? -> (ветка whenEnabled) / null
    /// </summary>
    public static Node<LineOperationCriteria> HasAndEnabled(Node<LineOperationCriteria> whenEnabled)
        => Node<LineOperationCriteria>.Decision(
            predicate: c => c.HasDZ,
            whenTrue: Node<LineOperationCriteria>.Decision(
                predicate: c => c.DZEnabled,
                whenTrue: whenEnabled,
                whenFalse: Node<LineOperationCriteria>.Action(null)
            ),
            whenFalse: Node<LineOperationCriteria>.Action(null)
        );

    /// <summary>
    /// Текущая принятая формулировка вывода ДЗ — всегда "вывести функцию ДЗ".
    /// (Если позже решите применять "вывести устройство", добавите аналог WithdrawByOnlyFunctionRule.)
    /// </summary>
    public static Node<LineOperationCriteria> WithdrawFunction()
        => Node<LineOperationCriteria>.Action(InstructionTexts.WithdrawFunction(FunctionNames.DZ));
}
