using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Operations.DecisionTree;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Операция: вывод ДФЗ для действия "Вывод ВЛ без замыкания поля".
/// </summary>
public sealed class DfzNoFieldClosingOperation : DecisionTreeOperationBase
{
    /// <inheritdoc />
    public override string Code => OperationCodes.DfzNoFieldClosing;

    /// <inheritdoc />
    protected override Node<LineOperationCriteria> BuildTree()
    {
        // Алгоритм (утверждённый):
        // HasDFZ?
        //   нет -> null
        //   да  -> DFZEnabled?
        //          нет -> null
        //          да  -> BothLineBreakerCTsOnSubstationSide?
        //                 нет -> null
        //                 да  -> IsOnlyFunctionInDevice?
        //                        да  -> "Вывести устройство"
        //                        нет -> "Вывести функцию ДФЗ"

        var withdraw = OperationNodes.WithdrawByOnlyFunctionRule(FunctionNames.DFZ);

        return Node<LineOperationCriteria>.Decision(
            predicate: c => c.HasDFZ,
            whenTrue: Node<LineOperationCriteria>.Decision(
                predicate: c => c.DFZEnabled,
                whenTrue: Node<LineOperationCriteria>.Decision(
                    predicate: c => c.BothLineBreakerCTsOnSubstationSide,
                    whenTrue: withdraw,
                    whenFalse: Node<LineOperationCriteria>.Action(null)
                ),
                whenFalse: Node<LineOperationCriteria>.Action(null)
            ),
            whenFalse: Node<LineOperationCriteria>.Action(null)
        );
    }
}
