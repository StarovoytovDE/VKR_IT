using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Operations.DecisionTree;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Операция: вывод ОАПВ для действия "Вывод ВЛ с замыканием поля".
/// </summary>
public sealed class OapvOperation : DecisionTreeOperationBase
{
    /// <inheritdoc />
    public override string Code => OperationCodes.Oapv;

    /// <inheritdoc />
    protected override Node<LineOperationCriteria> BuildTree()
    {
        // Новый алгоритм:
        // OAPVEnabled? -> OAPVState?
        //   нет/false -> null
        //   да/true   -> OAPVSwitchOff?
        //                true  -> "вывести функцию ОАПВ"
        //                false -> null
        return OapvNodes.EnabledAndState(
            whenTrue: OapvNodes.SwitchOff(OapvNodes.WithdrawFunction())
        );
    }
}
