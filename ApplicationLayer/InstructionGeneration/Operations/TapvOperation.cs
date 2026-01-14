using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Operations.DecisionTree;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Операция: вывести функцию ТАПВ.
/// Общий алгоритм для действий:
/// - LineWithdrawalWithFieldClosing
/// - LineWithdrawalWithoutFieldClosing
/// </summary>
public sealed class TapvOperation : DecisionTreeOperationBase
{
    /// <inheritdoc />
    public override string Code => OperationCodes.Tapv;

    /// <inheritdoc />
    protected override Node<LineOperationCriteria> BuildTree()
    {
        return TapvNodes.HasAndEnabled(TapvNodes.WithdrawFunction());
    }
}
