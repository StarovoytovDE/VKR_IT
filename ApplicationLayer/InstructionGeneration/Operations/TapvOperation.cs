using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Operations.DecisionTree;

// Алиас для читабельности деревьев решений
using DT = ApplicationLayer.InstructionGeneration.Operations.DecisionTree.Node<ApplicationLayer.InstructionGeneration.Criteria.LineOperationCriteria>;

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
    protected override DT BuildTree()
    {
        // Алгоритм (утверждённый):
        // HasTAPV?
        //   нет -> null
        //   да  -> TAPVEnabled?
        //          нет -> null
        //          да  -> "Вывести функцию ТАПВ"
        return DT.Decision(
            predicate: c => c.HasTAPV,
            whenTrue: DT.Decision(
                predicate: c => c.TAPVEnabled,
                whenTrue: DT.Action(InstructionTexts.WithdrawFunction(FunctionNames.TAPV)),
                whenFalse: DT.Action(null)
            ),
            whenFalse: DT.Action(null)
        );
    }
}
