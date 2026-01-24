using System;
using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Operations.DecisionTree;
using Domain.ReferenceData;

// Алиас для читабельности деревьев решений
using DT = ApplicationLayer.InstructionGeneration.Operations.DecisionTree.Node<ApplicationLayer.InstructionGeneration.Criteria.LineOperationCriteria>;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Операция: перевод цепей напряжения устройств РЗ и СА,
/// нормально подключенных к линейному ТН, с линейного ТН на резервный.
/// </summary>
public sealed class LineVtToReserveVoltageCircuitsTransferOperation : DecisionTreeOperationBase
{
    /// <inheritdoc />
    public override string Code => OperationCodes.LineVtToReserveVoltageCircuitsTransfer;

    /// <inheritdoc />
    protected override DT BuildTree()
    {
        return DT.Decision(
            predicate: IsSwitchFromLineVtToReserveRequired,
            whenTrue: DT.Action(InstructionTexts.TransferVoltageCircuitsFromLineVtToReserve),
            whenFalse: DT.Action(null)
        );
    }

    /// <summary>
    /// Требуется ли перевод цепей напряжения с линейного ТН на резервный по кодам мест подключения.
    /// </summary>
    private static bool IsSwitchFromLineVtToReserveRequired(LineOperationCriteria c)
    {
        ArgumentNullException.ThrowIfNull(c);

        if (!c.VtSwitchTrue)
            return false;

        if (string.IsNullOrWhiteSpace(c.MainVtPlaceCode) || string.IsNullOrWhiteSpace(c.ReserveVtPlaceCode))
            return false;

        return string.Equals(c.MainVtPlaceCode, PlaceCodes.Vt.Line, StringComparison.Ordinal) &&
               !string.Equals(c.ReserveVtPlaceCode, PlaceCodes.Vt.Line, StringComparison.Ordinal) &&
               !string.Equals(c.MainVtPlaceCode, c.ReserveVtPlaceCode, StringComparison.Ordinal);
    }
}
