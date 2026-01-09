using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Models;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Decision tree for switching line VT to reserve.
/// </summary>
public sealed class SwitchLineVtToReserveOperation : IOperation
{
    /// <inheritdoc />
    public OperationDecision Evaluate(LineOperationCriteria criteria)
    {
        if (!criteria.NeedSwitchLineVTToReserve)
        {
            return OperationDecision.None;
        }

        var item = InstructionItemFactory.Create(
            criteria,
            InstructionCodes.SwitchLineVtToReserve,
            InstructionTexts.SwitchLineVtToReserve,
            InstructionOrder.SwitchVt);

        return new OperationDecision(true, item, true, true);
    }
}
