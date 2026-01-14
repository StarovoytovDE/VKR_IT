using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Operations.DecisionTree;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Типовые узлы дерева решений для семейства ДФЗ.
/// </summary>
public static class DfzNodes
{
    /// <summary>
    /// Типовой каркас для ДФЗ:
    /// HasDFZ? -> DFZEnabled? -> (ветка whenEnabled) / null
    /// </summary>
    public static Node<LineOperationCriteria> HasAndEnabled(Node<LineOperationCriteria> whenEnabled)
        => Node<LineOperationCriteria>.Decision(
            predicate: c => c.HasDFZ,
            whenTrue: Node<LineOperationCriteria>.Decision(
                predicate: c => c.DFZEnabled,
                whenTrue: whenEnabled,
                whenFalse: Node<LineOperationCriteria>.Action(null)
            ),
            whenFalse: Node<LineOperationCriteria>.Action(null)
        );

    /// <summary>
    /// Типовой финал для ДФЗ: "вывести устройство" (если функция единственная)
    /// иначе "вывести функцию ДФЗ".
    /// </summary>
    public static Node<LineOperationCriteria> WithdrawByOnlyFunctionRule()
        => OperationNodes.WithdrawByOnlyFunctionRule(FunctionNames.DFZ);

    /// <summary>
    /// Общий ромб семейства ДФЗ: устройство подключено к линейному ТТ?
    /// Используется в нескольких DFZ-операциях, чтобы править условие централизованно.
    /// </summary>
    public static Node<LineOperationCriteria> IfDeviceConnectedToLineCT(
        Node<LineOperationCriteria> whenTrue,
        Node<LineOperationCriteria> whenFalse)
        => Node<LineOperationCriteria>.Decision(
            predicate: c => c.DeviceConnectedToLineCT,
            whenTrue: whenTrue,
            whenFalse: whenFalse
        );

    /// <summary>
    /// Перегрузка, если "иначе" означает "ничего не делать".
    /// </summary>
    public static Node<LineOperationCriteria> IfDeviceConnectedToLineCT(Node<LineOperationCriteria> whenTrue)
        => IfDeviceConnectedToLineCT(
            whenTrue,
            Node<LineOperationCriteria>.Action(null)
        );
}
