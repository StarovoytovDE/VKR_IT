using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Operations.DecisionTree;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Операция: вывод ДЗ для действия "Вывод ВЛ без замыкания поля".
/// </summary>
public sealed class DzNoFieldClosingOperation : DecisionTreeOperationBase
{
    /// <inheritdoc />
    public override string Code => OperationCodes.DzNoFieldClosing;

    /// <inheritdoc />
    protected override Node<LineOperationCriteria> BuildTree()
    {
        // Алгоритм (утверждённый):
        // HasDZ?
        //   нет -> null
        //   да  -> DZEnabled?
        //          нет -> null
        //          да  -> BothLineBreakerCTsOnSubstationSide?
        //                 нет -> null
        //                 да  -> "Вывести функцию ДЗ"

        return Node<LineOperationCriteria>.Decision(
            predicate: c => c.HasDZ,
            whenTrue: Node<LineOperationCriteria>.Decision(
                predicate: c => c.DZEnabled,
                whenTrue: Node<LineOperationCriteria>.Decision(
                    predicate: c => c.BothLineBreakerCTsOnSubstationSide,
                    whenTrue: Node<LineOperationCriteria>.Action(
                        InstructionTexts.WithdrawFunction(FunctionNames.DZ)),
                    whenFalse: Node<LineOperationCriteria>.Action(null)
                ),
                whenFalse: Node<LineOperationCriteria>.Action(null)
            ),
            whenFalse: Node<LineOperationCriteria>.Action(null)
        );
    }
}
