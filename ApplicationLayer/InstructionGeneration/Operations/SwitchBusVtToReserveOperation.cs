using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Models;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Decision tree for switching bus VT to reserve.
/// </summary>
public sealed class SwitchBusVtToReserveOperation : IOperation
{
    /// <inheritdoc />
    public OperationDecision Evaluate(LineOperationCriteria criteria)
    {
        if (!criteria.NeedSwitchBusVTToReserve)
        {
            return OperationDecision.None;
        }

        var item = InstructionItemFactory.Create(
            criteria,
            InstructionCodes.SwitchBusVtToReserve,
            InstructionTexts.SwitchBusVtToReserve,
            InstructionOrder.SwitchVt);

        return new OperationDecision(true, item, true, true);
    }
}
