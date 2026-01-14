using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Operations.DecisionTree;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Операция: вывод ТАПВ для действия "Вывод ВЛ без замыкания поля".
/// </summary>
public sealed class TapvNoFieldClosingOperation : DecisionTreeOperationBase
{
    /// <inheritdoc />
    public override string Code => OperationCodes.TapvNoFieldClosing;

    /// <inheritdoc />
    protected override Node<LineOperationCriteria> BuildTree()
    {
        var enabledBranch =
            Node<LineOperationCriteria>.Decision(
                predicate: c => c.BothLineBreakerCTsOnSubstationSide,
                whenTrue: TapvNodes.WithdrawFunction(),
                whenFalse: Node<LineOperationCriteria>.Action(null)
            );

        return TapvNodes.HasAndEnabled(enabledBranch);
    }
}
