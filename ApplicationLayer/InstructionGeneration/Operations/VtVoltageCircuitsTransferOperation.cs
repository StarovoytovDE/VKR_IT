using System;
using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Operations.DecisionTree;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Универсальная операция перевода цепей напряжения устройств РЗ и СА
/// с основного ТН на резервный ТН с динамической подстановкой имени и места установки.
/// </summary>
public sealed class VtVoltageCircuitsTransferOperation : DecisionTreeOperationBase
{
    /// <inheritdoc />
    public override string Code => OperationCodes.VtVoltageCircuitsTransfer;

    /// <inheritdoc />
    protected override Node<LineOperationCriteria> BuildTree()
    {
        return Node<LineOperationCriteria>.Decision(
            predicate: IsVtSwitchRequired,
            whenTrue: Node<LineOperationCriteria>.Action(c =>
                InstructionTexts.BuildVtVoltageCircuitsTransfer(
                    mainVtName: c.MainVtName,
                    mainVtPlace: c.MainVtPlace,
                    reserveVtName: c.ReserveVtName,
                    reserveVtPlace: c.ReserveVtPlace)),
            whenFalse: Node<LineOperationCriteria>.NoAction()
        );
    }

    /// <summary>
    /// Требуется ли перевод цепей напряжения по общим параметрам устройства.
    /// Условие: VtSwitchTrue=true, place_code заполнены и отличаются.
    /// </summary>
    private static bool IsVtSwitchRequired(LineOperationCriteria c)
    {
        ArgumentNullException.ThrowIfNull(c);

        if (!c.VtSwitchTrue)
            return false;

        if (string.IsNullOrWhiteSpace(c.MainVtPlaceCode) || string.IsNullOrWhiteSpace(c.ReserveVtPlaceCode))
            return false;

        return !string.Equals(c.MainVtPlaceCode, c.ReserveVtPlaceCode, StringComparison.Ordinal);
    }
}
