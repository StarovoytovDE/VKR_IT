using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Operations.DecisionTree;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Операция: вывод ДФЗ для действия "Вывод ВЛ без замыкания поля".
/// </summary>
public sealed class DfzNoFieldClosingOperation : DecisionTreeOperationBase
{
    /// <inheritdoc />
    public override string Code => OperationCodes.DfzNoFieldClosing;

    /// <inheritdoc />
    protected override Node<LineOperationCriteria> BuildTree()
    {
        var withdraw = DfzNodes.WithdrawByOnlyFunctionRule();

        var enabledBranch =
            Node<LineOperationCriteria>.Decision(
                predicate: c => c.BothLineBreakerCTsOnSubstationSide,
                whenTrue: withdraw,
                whenFalse: Node<LineOperationCriteria>.Action(null)
            );

        return DfzNodes.HasAndEnabled(enabledBranch);
    }
}
