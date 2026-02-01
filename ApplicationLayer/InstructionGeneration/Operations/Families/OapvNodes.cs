using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Operations.DecisionTree;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Узлы дерева решений для ОАПВ.
/// </summary>
public static class OapvNodes
{
    /// <summary>
    /// Типовой каркас для ОАПВ по новой модели (без HasOAPV):
    /// OAPVEnabled? -> OAPVState? -> (ветка whenTrue) / null
    /// </summary>
    public static Node<LineOperationCriteria> EnabledAndState(Node<LineOperationCriteria> whenTrue)
        => Node<LineOperationCriteria>.Decision(
            predicate: c => c.OAPVEnabled,
            whenTrue: Node<LineOperationCriteria>.Decision(
                predicate: c => c.OAPVState,
                whenTrue: whenTrue,
                whenFalse: Node<LineOperationCriteria>.NoAction()
            ),
            whenFalse: Node<LineOperationCriteria>.NoAction() 
        );

    /// <summary>
    /// Проверка флага switch_off для ОАПВ:
    /// OAPVSwitchOff? -> (ветка whenTrue) / null
    /// </summary>
    public static Node<LineOperationCriteria> SwitchOff(Node<LineOperationCriteria> whenTrue)
        => Node<LineOperationCriteria>.Decision(
            predicate: c => c.OAPVSwitchOff,
            whenTrue: whenTrue,
            whenFalse: Node<LineOperationCriteria>.NoAction()
        );

    /// <summary>
    /// Текущая принятая формулировка вывода ОАПВ — "вывести функцию ОАПВ".
    /// </summary>
    public static Node<LineOperationCriteria> WithdrawFunction()
        => Node<LineOperationCriteria>.Action(InstructionTexts.WithdrawFunction(FunctionNames.OAPV));
}
