using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Operations.DecisionTree;

// Алиас для читабельности деревьев решений
using DT = ApplicationLayer.InstructionGeneration.Operations.DecisionTree.Node<ApplicationLayer.InstructionGeneration.Criteria.LineOperationCriteria>;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Операция: вывод ДЗ для действия "Вывод ВЛ с замыканием поля".
/// </summary>
public sealed class DzFieldClosingOperation : DecisionTreeOperationBase
{
    /// <inheritdoc />
    public override string Code => OperationCodes.DzFieldClosing;

    /// <inheritdoc />
    protected override DT BuildTree()
    {
        // Алгоритм (подтверждённый):
        // HasDZ?
        //   нет -> null
        //   да  -> DZEnabled?
        //          нет -> null
        //          да  -> DZConnectedToLineVT?
        //                 нет -> null
        //                 да  -> NeedSwitchLineVTToReserve?
        //                        да  -> FollowVoltageTransferInstructions
        //                        нет -> WithdrawFunction("ДЗ")

        return DT.Decision(
            predicate: c => c.HasDZ,
            whenTrue: DT.Decision(
                predicate: c => c.DZEnabled,
                whenTrue: DT.Decision(
                    predicate: c => c.DZConnectedToLineVT,
                    whenTrue: DT.Decision(
                        predicate: c => c.NeedSwitchVTToReserve,
                        whenTrue: DT.Action(InstructionTexts.FollowVoltageTransferInstructions),
                        whenFalse: DT.Action(InstructionTexts.WithdrawFunction(FunctionNames.DZ))
                    ),
                    whenFalse: DT.Action(null)
                ),
                whenFalse: DT.Action(null)
            ),
            whenFalse: DT.Action(null)
        );
    }
}
