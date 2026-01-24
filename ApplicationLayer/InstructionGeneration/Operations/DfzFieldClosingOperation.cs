using System;
using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Operations.DecisionTree;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Операция: ДФЗ при выводе линии с замыканием поля.
/// </summary>
public sealed class DfzFieldClosingOperation : DecisionTreeOperationBase
{
    /// <inheritdoc />
    public override string Code => OperationCodes.DfzFieldClosing;

    /// <inheritdoc />
    protected override Node<LineOperationCriteria> BuildTree()
    {
        var withdraw = DfzNodes.WithdrawByOnlyFunctionRule();

        // Ветка whenTrue: устройство подключено к линейному ТТ -> дальше частные условия.
        var connectedToLineCtBranch =
            Node<LineOperationCriteria>.Decision(
                predicate: c => c.CtRemainsEnergizedOnThisSide,
                whenTrue: Node<LineOperationCriteria>.Decision(
                    predicate: IsAnyVtSwitchRequired,
                    whenTrue: Node<LineOperationCriteria>.Action(InstructionTexts.FollowVoltageTransferInstructions),
                    whenFalse: withdraw
                ),
                whenFalse: Node<LineOperationCriteria>.Action(null)
            );

        // Ветка whenFalse: устройство НЕ подключено к линейному ТТ -> выводим ДФЗ.
        // (в текущей логике DFZ выводится, когда нет "линейного ТТ", см. критерии DeviceConnectedToLineCT).
        var enabledBranch =
            DfzNodes.IfDeviceConnectedToLineCT(
                whenTrue: connectedToLineCtBranch,
                whenFalse: withdraw
            );

        return DfzNodes.HasAndEnabled(enabledBranch);
    }

    /// <summary>
    /// Определяет, требуется ли перевод цепей напряжения устройства на резервный ТН
    /// по общим параметрам устройства (работаем по place_code, а не по русским строкам).
    /// </summary>
    private static bool IsAnyVtSwitchRequired(LineOperationCriteria c)
    {
        ArgumentNullException.ThrowIfNull(c);

        return c.VtSwitchTrue &&
               !string.IsNullOrWhiteSpace(c.MainVtPlaceCode) &&
               !string.IsNullOrWhiteSpace(c.ReserveVtPlaceCode) &&
               !string.Equals(c.MainVtPlaceCode, c.ReserveVtPlaceCode, StringComparison.Ordinal);
    }
}
