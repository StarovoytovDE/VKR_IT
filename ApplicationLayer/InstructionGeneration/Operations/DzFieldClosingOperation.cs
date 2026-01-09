using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Models;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Decision tree for DZ when field closing is applied.
/// </summary>
public sealed class DzFieldClosingOperation : IOperation
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

        if (!criteria.DZConnectedToLineVT)
        {
            return OperationDecision.None;
        }

        if (criteria.NeedSwitchLineVTToReserve || criteria.NeedSwitchBusVTToReserve)
        {
            var item = InstructionItemFactory.Create(
                criteria,
                InstructionCodes.FollowVoltageSwitchInstructions,
                InstructionTexts.FollowVoltageSwitchInstructions,
                InstructionOrder.SwitchVt);

            return new OperationDecision(true, item, true, false);
        }

        var result = InstructionItemFactory.Create(
            criteria,
            InstructionCodes.DzDisableFunction,
            InstructionTexts.DisableDzFunction,
            InstructionOrder.Dz);

        return new OperationDecision(true, result);
    }
}
