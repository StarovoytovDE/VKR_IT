using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Operations.DecisionTree;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Операция вывода ДФЗ при выводе ВЛ с одной стороны
/// (когда линия остается включенной с противоположной стороны).
/// </summary>
public sealed class DfzSingleSideWithdrawalOperation : DecisionTreeOperationBase
{
    /// <inheritdoc />
    public override string Code => OperationCodes.DfzSingleSideWithdrawal;

    /// <inheritdoc />
    protected override Node<LineOperationCriteria> BuildTree()
    {
        var withdraw = DfzNodes.WithdrawByOnlyFunctionRule();

        // Специфика "enabled" у этого варианта начинается с общего ромба DeviceConnectedToLineCT.
        var enabledBranch =
            DfzNodes.IfDeviceConnectedToLineCT(
                whenTrue: withdraw,
                whenFalse: Node<LineOperationCriteria>.Action(null)
            );

        return DfzNodes.HasAndEnabled(enabledBranch);
    }
}
