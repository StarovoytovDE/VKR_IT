using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Operations.DecisionTree;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Операция: вывод ТАПВ для действия "Вывод ВЛ с замыканием поля".
/// </summary>
public sealed class TapvOperation : DecisionTreeOperationBase
{
    /// <inheritdoc />
    public override string Code => OperationCodes.Tapv;

    /// <inheritdoc />
    protected override Node<LineOperationCriteria> BuildTree()
    {
        // Новый алгоритм:
        // TAPVEnabled? -> TAPVState?
        //   нет/false -> null
        //   да/true   -> TAPVSwitchOff?
        //                true  -> "вывести функцию ТАПВ"
        //                false -> null
        return TapvNodes.EnabledAndState(
            whenTrue: TapvNodes.SwitchOff(TapvNodes.WithdrawFunction())
        );
    }
}
