using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Models;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Decision tree for DZL when field closing is applied.
/// </summary>
public sealed class DzlFieldClosingOperation : IOperation
{
    /// <inheritdoc />
    public OperationDecision Evaluate(LineOperationCriteria criteria)
    {
        if (!criteria.HasDZL)
        {
            return OperationDecision.None;
        }

        if (!criteria.DZLEnabled)
        {
            return OperationDecision.None;
        }

        var item = InstructionItemFactory.Create(
            criteria,
            InstructionCodes.DzlDisableDevice,
            InstructionTexts.DisableDzlDevice,
            InstructionOrder.Dzl);

        return new OperationDecision(true, item);
    }
}
