using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Models;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Decision tree for DZ without field closing.
/// </summary>
public sealed class DzNoFieldClosingOperation : IOperation
{
    /// <inheritdoc />
    public OperationDecision Evaluate(LineOperationCriteria criteria)
    {
        if (!criteria.HasDZ)
        {
            return OperationDecision.None;
        }

        if (!criteria.DZEnabled)
        {
            return OperationDecision.None;
        }

        if (!criteria.BothCbctReverseSide)
        {
            return OperationDecision.None;
        }

        var item = InstructionItemFactory.Create(
            criteria,
            InstructionCodes.DzDisableFunction,
            InstructionTexts.DisableDzFunction,
            InstructionOrder.Dz);

        return new OperationDecision(true, item);
    }
}
