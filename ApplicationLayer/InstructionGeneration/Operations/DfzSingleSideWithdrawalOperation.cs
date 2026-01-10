using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Operations.DecisionTree;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Операция вывода ДФЗ при выводе ВЛ с одной стороны
/// (когда линия остается включенной с противоположной стороны).
/// </summary>
public sealed class DfzSingleSideWithdrawalOperation : DecisionTreeOperationBase
{
    /// <inheritdoc />
    public override string Code => OperationCodes.DfzSingleSideWithdrawal;

    /// <inheritdoc />
    protected override Node<LineOperationCriteria> BuildTree()
    {
        // Алгоритм:
        // 1) Есть ли функция ДФЗ?
        //    нет -> null
        //    да  -> 2
        //
        // 2) Функция введена?
        //    нет -> null
        //    да  -> 3
        //
        // 3) Устройство подключено к линейному ТТ?
        //    нет -> null
        //    да  -> 4
        //
        // 4) Это единственная функция в устройстве?
        //    да  -> "Вывести устройство"
        //    нет -> "Вывести функцию ДФЗ"

        var withdraw =
            OperationNodes.WithdrawByOnlyFunctionRule(FunctionNames.DFZ);

        return Node<LineOperationCriteria>.Decision(
            predicate: c => c.HasDFZ,
            whenTrue: Node<LineOperationCriteria>.Decision(
                predicate: c => c.DFZEnabled,
                whenTrue: Node<LineOperationCriteria>.Decision(
                    predicate: c => c.DeviceConnectedToLineCT,
                    whenTrue: withdraw,
                    whenFalse: Node<LineOperationCriteria>.Action(null)
                ),
                whenFalse: Node<LineOperationCriteria>.Action(null)
            ),
            whenFalse: Node<LineOperationCriteria>.Action(null)
        );
    }
}
