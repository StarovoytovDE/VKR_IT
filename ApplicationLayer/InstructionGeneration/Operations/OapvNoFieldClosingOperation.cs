using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Operations.DecisionTree;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Операция: вывод ОАПВ для действия "Вывод ВЛ без замыкания поля".
/// </summary>
public sealed class OapvNoFieldClosingOperation : DecisionTreeOperationBase
{
    /// <inheritdoc />
    public override string Code => OperationCodes.OapvNoFieldClosing;

    /// <inheritdoc />
    protected override Node<LineOperationCriteria> BuildTree()
    {
        // Специфика этого варианта начинается только после Has/Enabled:
        // BothLineBreakerCTsOnSubstationSide? -> вывести ОАПВ : null
        var enabledBranch =
            Node<LineOperationCriteria>.Decision(
                predicate: c => c.BothLineBreakerCTsOnSubstationSide,
                whenTrue: OapvNodes.WithdrawFunction(),
                whenFalse: Node<LineOperationCriteria>.Action(null)
            );

        return OapvNodes.HasAndEnabled(enabledBranch);
    }
}
