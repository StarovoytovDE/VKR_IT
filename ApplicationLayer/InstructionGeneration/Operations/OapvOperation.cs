using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Operations.DecisionTree;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Операция: вывести функцию ОАПВ.
/// Общий алгоритм для действий:
/// - LineWithdrawalWithFieldClosing
/// - LineWithdrawalWithoutFieldClosing
/// </summary>
public sealed class OapvOperation : DecisionTreeOperationBase
{
    /// <inheritdoc />
    public override string Code => OperationCodes.Oapv;

    /// <inheritdoc />
    protected override Node<LineOperationCriteria> BuildTree()
    {
        // enabled-ветка для "обычного" ОАПВ — просто вывести функцию.
        return OapvNodes.HasAndEnabled(OapvNodes.WithdrawFunction());
    }
}
