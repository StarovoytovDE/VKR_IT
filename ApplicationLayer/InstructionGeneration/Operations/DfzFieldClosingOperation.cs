using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Operations.DecisionTree;

// Алиасы для читабельности
using DT = ApplicationLayer.InstructionGeneration.Operations.DecisionTree.Node<ApplicationLayer.InstructionGeneration.Criteria.LineOperationCriteria>;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Операция: ДФЗ при выводе линии с замыканием поля.
/// </summary>
public sealed class DfzFieldClosingOperation : DecisionTreeOperationBase
{
    /// <inheritdoc />
    public override string Code => OperationCodes.DfzFieldClosing;

    /// <inheritdoc />
    protected override DT BuildTree()
    {
        var withdraw = OperationNodes.WithdrawByOnlyFunctionRule("ДФЗ");

        return DT.Decision(
            predicate: c => c.HasDFZ,
            whenTrue: DT.Decision(
                predicate: c => c.DFZEnabled,
                whenTrue: DT.Decision(
                    predicate: c => c.DFZConnectedToLineCT,
                    whenTrue: DT.Decision(
                        predicate: c => c.DFZConnectedToLineVT,
                        whenTrue: DT.Decision(
                            predicate: c => c.NeedSwitchLineVTToReserve,
                            whenTrue: DT.Action(InstructionTexts.FollowVoltageTransferInstructions),
                            whenFalse: withdraw
                        ),
                        whenFalse: DT.Action(null)
                    ),
                    whenFalse: withdraw
                ),
                whenFalse: DT.Action(null)
            ),
            whenFalse: DT.Action(null)
        );
    }
}
