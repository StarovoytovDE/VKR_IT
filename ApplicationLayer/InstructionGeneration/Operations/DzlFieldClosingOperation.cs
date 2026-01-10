using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Operations.DecisionTree;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Операция: ДЗЛ при выводе линии с замыканием поля.
/// </summary>
public sealed class DzlFieldClosingOperation : DecisionTreeOperationBase
{
    /// <inheritdoc />
    public override string Code => OperationCodes.DzlFieldClosing;

    /// <inheritdoc />
    protected override Node<LineOperationCriteria> BuildTree()
    {
        var withdraw = OperationNodes.WithdrawByOnlyFunctionRule("ДЗЛ");

        return Node<LineOperationCriteria>.Decision(
            predicate: c => c.HasDZL,
            whenTrue: Node<LineOperationCriteria>.Decision(
                predicate: c => c.DZLEnabled,
                whenTrue: withdraw,
                whenFalse: Node<LineOperationCriteria>.Action(null)
            ),
            whenFalse: Node<LineOperationCriteria>.Action(null)
        );
    }
}
