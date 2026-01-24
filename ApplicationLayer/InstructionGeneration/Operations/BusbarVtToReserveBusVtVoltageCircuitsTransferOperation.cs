using System;
using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Operations.DecisionTree;
using Domain.ReferenceData;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Операция: перевод цепей напряжения устройств РЗ и СА, нормально подключенных к ТН ошиновки,
/// с ТН ошиновки на резервный шинный ТН.
/// </summary>
public sealed class BusbarVtToReserveBusVtVoltageCircuitsTransferOperation : DecisionTreeOperationBase
{
    /// <inheritdoc />
    public override string Code => OperationCodes.BusbarVtToReserveBusVtVoltageCircuitsTransfer;

    /// <inheritdoc />
    protected override Node<LineOperationCriteria> BuildTree()
    {
        return Node<LineOperationCriteria>.Decision(
            predicate: IsSwitchFromBusbarVtToReserveBusVtRequired,
            whenTrue: Node<LineOperationCriteria>.Action(
                InstructionTexts.TransferVoltageCircuitsFromBusbarVtToReserveBusVt),
            whenFalse: Node<LineOperationCriteria>.Action(null)
        );
    }

    /// <summary>
    /// Требуется ли перевод цепей напряжения с ТН ошиновки на резервный шинный ТН по кодам мест подключения.
    /// </summary>
    private static bool IsSwitchFromBusbarVtToReserveBusVtRequired(LineOperationCriteria c)
    {
        ArgumentNullException.ThrowIfNull(c);

        if (!c.VtSwitchTrue)
            return false;

        if (string.IsNullOrWhiteSpace(c.MainVtPlaceCode) || string.IsNullOrWhiteSpace(c.ReserveVtPlaceCode))
            return false;

        return string.Equals(c.MainVtPlaceCode, PlaceCodes.Vt.Busbar, StringComparison.Ordinal) &&
               string.Equals(c.ReserveVtPlaceCode, PlaceCodes.Vt.Bus, StringComparison.Ordinal) &&
               !string.Equals(c.MainVtPlaceCode, c.ReserveVtPlaceCode, StringComparison.Ordinal);
    }
}
