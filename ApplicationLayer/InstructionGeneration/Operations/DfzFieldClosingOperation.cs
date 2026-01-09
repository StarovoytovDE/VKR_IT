using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Models;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Decision tree for DFZ when field closing is applied.
/// </summary>
public sealed class DfzFieldClosingOperation : IOperation
{
    /// <inheritdoc />
    public OperationDecision Evaluate(LineOperationCriteria criteria)
    {
        if (!criteria.HasDFZ)
        {
            return OperationDecision.None;
        }

        if (!criteria.DFZEnabled)
        {
            return OperationDecision.None;
        }

        if (!criteria.DFZConnectedToLineCT)
        {
            return criteria.IsOnlyFunctionInDevice
                ? CreateDeviceDecision(criteria)
                : CreateFunctionDecision(criteria);
        }

        if (!criteria.DFZConnectedToLineVT)
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

        return criteria.IsOnlyFunctionInDevice
            ? CreateDeviceDecision(criteria)
            : CreateFunctionDecision(criteria);
    }

    private static OperationDecision CreateFunctionDecision(LineOperationCriteria criteria)
    {
        var item = InstructionItemFactory.Create(
            criteria,
            InstructionCodes.DfzDisableFunction,
            InstructionTexts.DisableDfzFunction,
            InstructionOrder.Dfz);

        return new OperationDecision(true, item);
    }

    private static OperationDecision CreateDeviceDecision(LineOperationCriteria criteria)
    {
        var item = InstructionItemFactory.Create(
            criteria,
            InstructionCodes.DfzDisableDevice,
            InstructionTexts.DisableDfzDevice,
            InstructionOrder.Dfz);

        return new OperationDecision(true, item);
    }
}
