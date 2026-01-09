using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Models;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Decision tree for TAPV without field closing.
/// </summary>
public sealed class TapvNoFieldClosingOperation : IOperation
{
    /// <inheritdoc />
    public OperationDecision Evaluate(LineOperationCriteria criteria)
    {
        if (!criteria.HasTAPV)
        {
            return OperationDecision.None;
        }

        if (!criteria.TAPVEnabled)
        {
            return OperationDecision.None;
        }

        if (!criteria.BothCbctReverseSide)
        {
            return OperationDecision.None;
        }

        var item = InstructionItemFactory.Create(
            criteria,
            InstructionCodes.TapvDisableFunction,
            InstructionTexts.DisableTapvFunction,
            InstructionOrder.Tapv);

        return new OperationDecision(true, item);
    }
}
