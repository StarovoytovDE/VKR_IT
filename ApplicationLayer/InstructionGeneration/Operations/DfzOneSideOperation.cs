using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Models;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Decision tree for DFZ when the line remains energized on one side.
/// </summary>
public sealed class DfzOneSideOperation : IOperation
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
            return OperationDecision.None;
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
