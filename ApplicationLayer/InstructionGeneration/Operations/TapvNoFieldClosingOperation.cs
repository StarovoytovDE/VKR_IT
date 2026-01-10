using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Operations.DecisionTree;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Операция: вывод ТАПВ для действия "Вывод ВЛ без замыкания поля".
/// </summary>
public sealed class TapvNoFieldClosingOperation : DecisionTreeOperationBase
{
    /// <inheritdoc />
    public override string Code => OperationCodes.TapvNoFieldClosing;

    /// <inheritdoc />
    protected override Node<LineOperationCriteria> BuildTree()
    {
        // Алгоритм (утверждённый по схеме):
        // HasTAPV?
        //   нет -> null
        //   да  -> TAPVEnabled?
        //          нет -> null
        //          да  -> BothLineBreakerCTsOnSubstationSide?
        //                 нет -> null
        //                 да  -> "Вывести функцию ТАПВ"

        return Node<LineOperationCriteria>.Decision(
            predicate: c => c.HasTAPV,
            whenTrue: Node<LineOperationCriteria>.Decision(
                predicate: c => c.TAPVEnabled,
                whenTrue: Node<LineOperationCriteria>.Decision(
                    predicate: c => c.BothLineBreakerCTsOnSubstationSide,
                    whenTrue: Node<LineOperationCriteria>.Action(
                        InstructionTexts.WithdrawFunction(FunctionNames.TAPV)),
                    whenFalse: Node<LineOperationCriteria>.Action(null)
                ),
                whenFalse: Node<LineOperationCriteria>.Action(null)
            ),
            whenFalse: Node<LineOperationCriteria>.Action(null)
        );
    }
}
