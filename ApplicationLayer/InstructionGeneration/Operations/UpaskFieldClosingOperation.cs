using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Models;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Decision tree for disabling UPASK receivers when field closing is applied.
/// </summary>
public sealed class UpaskFieldClosingOperation : IOperation
{
    /// <inheritdoc />
    public OperationDecision Evaluate(LineOperationCriteria criteria)
    {
        if (!criteria.NeedDisableUpaskReceivers)
        {
            return OperationDecision.None;
        }

        var item = InstructionItemFactory.Create(
            criteria,
            InstructionCodes.UpaskDisableReceivers,
            InstructionTexts.DisableUpaskReceivers,
            InstructionOrder.Upask);

        return new OperationDecision(true, item);
    }
}
