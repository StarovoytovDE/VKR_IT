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
        // Новый алгоритм:
        // TAPVEnabled? -> TAPVState?
        //   нет/false -> null
        //   да/true   -> BothLineBreakerCTsOnSubstationSide?
        //                нет -> null
        //                да  -> TAPVSwitchOff?
        //                       true  -> "вывести функцию ТАПВ"
        //                       false -> null
        var afterSpecific = Node<LineOperationCriteria>.Decision(
            predicate: c => c.BothLineBreakerCTsOnSubstationSide,
            whenTrue: TapvNodes.SwitchOff(TapvNodes.WithdrawFunction()),
            whenFalse: Node<LineOperationCriteria>.Action(null)
        );

        return TapvNodes.EnabledAndState(afterSpecific);
    }
}
