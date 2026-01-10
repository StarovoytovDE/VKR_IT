using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Operations.DecisionTree;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Операция: вывод ОАПВ для действия "Вывод ВЛ без замыкания поля".
/// </summary>
public sealed class OapvNoFieldClosingOperation : DecisionTreeOperationBase
{
    /// <inheritdoc />
    public override string Code => OperationCodes.OapvNoFieldClosing;

    /// <inheritdoc />
    protected override Node<LineOperationCriteria> BuildTree()
    {
        // Алгоритм (утверждённый по схеме):
        // HasOAPV?
        //   нет -> null
        //   да  -> OAPVEnabled?
        //          нет -> null
        //          да  -> BothLineBreakerCTsOnSubstationSide?
        //                 нет -> null
        //                 да  -> "Вывести функцию ОАПВ"

        return Node<LineOperationCriteria>.Decision(
            predicate: c => c.HasOAPV,
            whenTrue: Node<LineOperationCriteria>.Decision(
                predicate: c => c.OAPVEnabled,
                whenTrue: Node<LineOperationCriteria>.Decision(
                    predicate: c => c.BothLineBreakerCTsOnSubstationSide,
                    whenTrue: Node<LineOperationCriteria>.Action(
                        InstructionTexts.WithdrawFunction(FunctionNames.OAPV)),
                    whenFalse: Node<LineOperationCriteria>.Action(null)
                ),
                whenFalse: Node<LineOperationCriteria>.Action(null)
            ),
            whenFalse: Node<LineOperationCriteria>.Action(null)
        );
    }
}
