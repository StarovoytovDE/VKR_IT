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
        // Новый алгоритм:
        // OAPVEnabled? -> OAPVState?
        //   нет/false -> null
        //   да/true   -> BothLineBreakerCTsOnSubstationSide?
        //                нет -> null
        //                да  -> OAPVSwitchOff?
        //                       true  -> "вывести функцию ОАПВ"
        //                       false -> null
        var afterSpecific = Node<LineOperationCriteria>.Decision(
            predicate: c => c.BothLineBreakerCTsOnSubstationSide,
            whenTrue: OapvNodes.SwitchOff(OapvNodes.WithdrawFunction()),
            whenFalse: Node<LineOperationCriteria>.NoAction()
        );

        return OapvNodes.EnabledAndState(afterSpecific);
    }
}
