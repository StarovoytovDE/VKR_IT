using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Operations.DecisionTree;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Набор типовых узлов дерева решений для переиспользования в операциях.
/// </summary>
public static class OperationNodes
{
    /// <summary>
    /// Типовая развилка:
    /// если функция единственная в устройстве — вывести устройство,
    /// иначе — вывести функцию.
    /// </summary>
    public static Node<LineOperationCriteria> WithdrawByOnlyFunctionRule(string functionName)
        => Node<LineOperationCriteria>.Decision(
            predicate: c => c.IsOnlyFunctionInDevice,
            whenTrue: Node<LineOperationCriteria>.Action(InstructionTexts.WithdrawDevice),
            whenFalse: Node<LineOperationCriteria>.Action(InstructionTexts.WithdrawFunction(functionName))
        );
}
