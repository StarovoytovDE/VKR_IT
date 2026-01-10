using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Operations.DecisionTree;

// Алиас для читабельности деревьев решений
using DT = ApplicationLayer.InstructionGeneration.Operations.DecisionTree.Node<ApplicationLayer.InstructionGeneration.Criteria.LineOperationCriteria>;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Операция: ввести в работу МТЗ ошиновки (переключить уставки с группы «А» на группу «B»).
/// Общий алгоритм для действий:
/// - LineWithdrawalWithFieldClosing
/// - LineWithdrawalWithoutFieldClosing
/// </summary>
public sealed class MtzoShinovkaAtoBOperation : DecisionTreeOperationBase
{
    /// <inheritdoc />
    public override string Code => OperationCodes.MtzoShinovkaAtoB;

    /// <inheritdoc />
    protected override DT BuildTree()
    {
        // Алгоритм (утверждённый, вариант A):
        // HasMtzoShinovka?
        //   нет -> null
        //   да  -> CtRemainsEnergizedOnThisSide?
        //          нет -> null
        //          да  -> "После отключения выключателей линии, ... переключить уставки ..."
        return DT.Decision(
            predicate: c => c.HasMtzoShinovka,
            whenTrue: DT.Decision(
                predicate: c => c.CtRemainsEnergizedOnThisSide,
                whenTrue: DT.Action(InstructionTexts.MtzoShinovkaSwitchGroupAtoB),
                whenFalse: DT.Action(null)
            ),
            whenFalse: DT.Action(null)
        );
    }
}
