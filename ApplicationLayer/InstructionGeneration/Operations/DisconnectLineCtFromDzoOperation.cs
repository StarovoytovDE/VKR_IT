using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Operations.DecisionTree;

// Алиас для читабельности деревьев решений
using DT = ApplicationLayer.InstructionGeneration.Operations.DecisionTree.Node<ApplicationLayer.InstructionGeneration.Criteria.LineOperationCriteria>;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Операция: отключить токовые цепи линейного ТТ от ДЗО данной ВЛ.
/// Применяется для действий:
/// - LineWithdrawalWithFieldClosing
/// - LineWithdrawalWithBusSideDisconnector
/// </summary>
public sealed class DisconnectLineCtFromDzoOperation : DecisionTreeOperationBase
{
    /// <inheritdoc />
    public override string Code => OperationCodes.DisconnectLineCtFromDzo;

    /// <inheritdoc />
    protected override DT BuildTree()
    {
        // Алгоритм (утверждённый):
        // DeviceConnectedToLineCT?
        //   нет -> null
        //   да  -> NeedDisconnectLineCTFromDZO?
        //          нет -> null
        //          да  -> "Произвести отключение токовых цепей линейного ТТ от ДЗО данной ВЛ"
        return DT.Decision(
            predicate: c => c.DeviceConnectedToLineCT,
            whenTrue: DT.Decision(
                predicate: c => c.NeedDisconnectLineCTFromDZO,
                whenTrue: DT.Action(InstructionTexts.DisconnectLineCtFromDzo),
                whenFalse: DT.Action(null)
            ),
            whenFalse: DT.Action(null)
        );
    }
}
