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

        // Ветка whenTrue: устройство подключено к линейному ТТ -> дальше ваши частные условия.
        var connectedToLineCtBranch =
            Node<LineOperationCriteria>.Decision(
                predicate: c => c.CtRemainsEnergizedOnThisSide,
                whenTrue: Node<LineOperationCriteria>.Decision(
                    predicate: c => c.NeedSwitchVTToReserve,
                    whenTrue: Node<LineOperationCriteria>.Action(InstructionTexts.FollowVoltageTransferInstructions),
                    whenFalse: withdraw
                ),
                whenFalse: Node<LineOperationCriteria>.Action(null)
            );

        // Общий ромб семейства DFZ вынесен в DfzNodes.
        // whenFalse здесь по вашей текущей логике ведёт на withdraw.
        var enabledBranch =
            DfzNodes.IfDeviceConnectedToLineCT(
                whenTrue: connectedToLineCtBranch,
                whenFalse: withdraw
            );

        return DfzNodes.HasAndEnabled(enabledBranch);
    }
}
