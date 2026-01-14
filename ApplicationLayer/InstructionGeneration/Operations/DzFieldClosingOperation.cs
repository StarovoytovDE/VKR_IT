using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Operations.DecisionTree;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Операция: вывод ДЗ для действия "Вывод ВЛ с замыканием поля".
/// </summary>
public sealed class DzFieldClosingOperation : DecisionTreeOperationBase
{
    /// <inheritdoc />
    public override string Code => OperationCodes.DzFieldClosing;

    /// <inheritdoc />
    protected override Node<LineOperationCriteria> BuildTree()
    {
        // Это — та же enabled-ветка, что у тебя была, только без повторения HasDZ/DZEnabled.
        var enabledBranch =
            Node<LineOperationCriteria>.Decision(
                predicate: c => c.DZConnectedToLineVT,
                whenTrue: Node<LineOperationCriteria>.Decision(
                    predicate: c => c.NeedSwitchVTToReserve,
                    whenTrue: Node<LineOperationCriteria>.Action(InstructionTexts.FollowVoltageTransferInstructions),
                    whenFalse: DzNodes.WithdrawFunction()
                ),
                whenFalse: Node<LineOperationCriteria>.Action(null)
            );

        return DzNodes.HasAndEnabled(enabledBranch);
    }
}
