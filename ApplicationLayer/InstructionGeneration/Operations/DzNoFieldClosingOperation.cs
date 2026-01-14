using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Operations.DecisionTree;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Операция: вывод ДЗ для действия "Вывод ВЛ без замыкания поля".
/// </summary>
public sealed class DzNoFieldClosingOperation : DecisionTreeOperationBase
{
    /// <inheritdoc />
    public override string Code => OperationCodes.DzNoFieldClosing;

    /// <inheritdoc />
    protected override Node<LineOperationCriteria> BuildTree()
    {
        var enabledBranch =
            Node<LineOperationCriteria>.Decision(
                predicate: c => c.BothLineBreakerCTsOnSubstationSide,
                whenTrue: DzNodes.WithdrawFunction(),
                whenFalse: Node<LineOperationCriteria>.Action(null)
            );

        return DzNodes.HasAndEnabled(enabledBranch);
    }
}
