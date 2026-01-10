using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Operations.DecisionTree;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Операция: перевод цепей напряжения устройств РЗ и СА, нормально подключенных к ТН ошиновки,
/// с ТН ошиновки на резервный шинный ТН.
/// Применяется для действия: LineWithdrawalWithBusSideDisconnector.
/// </summary>
public sealed class BusbarVtToReserveBusVtVoltageCircuitsTransferOperation : DecisionTreeOperationBase
{
    /// <inheritdoc />
    public override string Code => OperationCodes.BusbarVtToReserveBusVtVoltageCircuitsTransfer;

    /// <inheritdoc />
    protected override Node<LineOperationCriteria> BuildTree()
    {
        // Алгоритм (утверждённый):
        // NeedSwitchFromBusVTToReserve?
        //   нет -> null
        //   да  -> "Произвести перевод цепей напряжения ... с ТН ошиновки на резервный шинный ТН"
        return Node<LineOperationCriteria>.Decision(
            predicate: c => c.NeedSwitchFromBusVTToReserve,
            whenTrue: Node<LineOperationCriteria>.Action(
                InstructionTexts.TransferVoltageCircuitsFromBusbarVtToReserveBusVt),
            whenFalse: Node<LineOperationCriteria>.Action(null)
        );
    }
}
