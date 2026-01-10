using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Operations.DecisionTree;

// Алиас для читабельности деревьев решений
using DT = ApplicationLayer.InstructionGeneration.Operations.DecisionTree.Node<ApplicationLayer.InstructionGeneration.Criteria.LineOperationCriteria>;

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
    protected override DT BuildTree()
    {
        // Алгоритм (утверждённый):
        // HasOAPV?
        //   нет -> null
        //   да  -> OAPVEnabled?
        //          нет -> null
        //          да  -> "Вывести функцию ОАПВ"
        return DT.Decision(
            predicate: c => c.HasOAPV,
            whenTrue: DT.Decision(
                predicate: c => c.OAPVEnabled,
                whenTrue: DT.Action(InstructionTexts.WithdrawFunction(FunctionNames.OAPV)),
                whenFalse: DT.Action(null)
            ),
            whenFalse: DT.Action(null)
        );
    }
}
