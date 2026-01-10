using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Operations.DecisionTree;

// Алиас для читабельности деревьев решений
using DT = ApplicationLayer.InstructionGeneration.Operations.DecisionTree.Node<ApplicationLayer.InstructionGeneration.Criteria.LineOperationCriteria>;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Операция: перевод цепей напряжения устройств РЗ и СА,
/// нормально подключенных к линейному ТН, с линейного ТН на резервный.
/// Применяется для действия: LineWithdrawalWithFieldClosing.
/// </summary>
public sealed class LineVtToReserveVoltageCircuitsTransferOperation : DecisionTreeOperationBase
{
    /// <inheritdoc />
    public override string Code => OperationCodes.LineVtToReserveVoltageCircuitsTransfer;

    /// <inheritdoc />
    protected override DT BuildTree()
    {
        // Алгоритм (утверждённый):
        // NeedSwitchFromLineVTToReserve?
        //   нет -> null
        //   да  -> "Произвести перевод цепей напряжения ... с линейного ТН на резервный"
        return DT.Decision(
            predicate: c => c.NeedSwitchFromLineVTToReserve,
            whenTrue: DT.Action(InstructionTexts.TransferVoltageCircuitsFromLineVtToReserve),
            whenFalse: DT.Action(null)
        );
    }
}
