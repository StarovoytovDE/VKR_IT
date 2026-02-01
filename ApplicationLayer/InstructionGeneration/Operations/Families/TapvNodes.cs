using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Operations.DecisionTree;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Узлы дерева решений для ТАПВ.
/// </summary>
public static class TapvNodes
{
    /// <summary>
    /// Типовой каркас для ТАПВ по новой модели (без HasTAPV):
    /// TAPVEnabled? -> TAPVState? -> (ветка whenTrue) / null
    /// </summary>
    public static Node<LineOperationCriteria> EnabledAndState(Node<LineOperationCriteria> whenTrue)
        => Node<LineOperationCriteria>.Decision(
            predicate: c => c.TAPVEnabled,
            whenTrue: Node<LineOperationCriteria>.Decision(
                predicate: c => c.TAPVState,
                whenTrue: whenTrue,
                whenFalse: Node<LineOperationCriteria>.Action(null)
            ),
            whenFalse: Node<LineOperationCriteria>.Action(null)
        );

    /// <summary>
    /// Проверка флага switch_off для ТАПВ:
    /// TAPVSwitchOff? -> (ветка whenTrue) / null
    /// </summary>
    public static Node<LineOperationCriteria> SwitchOff(Node<LineOperationCriteria> whenTrue)
        => Node<LineOperationCriteria>.Decision(
            predicate: c => c.TAPVSwitchOff,
            whenTrue: whenTrue,
            whenFalse: Node<LineOperationCriteria>.Action(null)
        );

    /// <summary>
    /// Текущая принятая формулировка вывода ТАПВ — "вывести функцию ТАПВ".
    /// </summary>
    public static Node<LineOperationCriteria> WithdrawFunction()
        => Node<LineOperationCriteria>.Action(InstructionTexts.WithdrawFunction(FunctionNames.TAPV));
}
