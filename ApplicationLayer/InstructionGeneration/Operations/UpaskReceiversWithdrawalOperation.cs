using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Operations.DecisionTree;

// Алиас для читабельности деревьев решений
using DT = ApplicationLayer.InstructionGeneration.Operations.DecisionTree.Node<ApplicationLayer.InstructionGeneration.Criteria.LineOperationCriteria>;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Операция: вывести приёмники УПАСК, по которым организована передача команд РЗ.
/// Применяется для действия: LineWithdrawalWithFieldClosing.
/// </summary>
public sealed class UpaskReceiversWithdrawalOperation : DecisionTreeOperationBase
{
    /// <inheritdoc />
    public override string Code => OperationCodes.UpaskReceiversWithdrawal;

    /// <inheritdoc />
    protected override DT BuildTree()
    {
        // Алгоритм (утверждённый):
        // NeedDisableUpaskReceivers?
        //   нет -> null
        //   да  -> "Вывести приёмники УПАСК, по которым организована передача команд РЗ"
        return DT.Decision(
            predicate: c => c.NeedDisableUpaskReceivers,
            whenTrue: DT.Action(InstructionTexts.WithdrawUpaskReceiversForRzCommands),
            whenFalse: DT.Action(null)
        );
    }
}
