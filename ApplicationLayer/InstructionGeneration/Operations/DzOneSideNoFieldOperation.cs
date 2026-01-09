using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Models;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Decision tree for DZ when the line remains energized on one side without field closing.
/// </summary>
public sealed class DzOneSideNoFieldOperation : IOperation
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

        return criteria.IsOnlyFunctionInDevice
            ? CreateDeviceDecision(criteria)
            : CreateFunctionDecision(criteria);
    }

    private static OperationDecision CreateFunctionDecision(LineOperationCriteria criteria)
    {
        var item = InstructionItemFactory.Create(
            criteria,
            InstructionCodes.DzDisableFunction,
            InstructionTexts.DisableDzFunction,
            InstructionOrder.Dz);

        return new OperationDecision(true, item);
    }

    private static OperationDecision CreateDeviceDecision(LineOperationCriteria criteria)
    {
        var item = InstructionItemFactory.Create(
            criteria,
            InstructionCodes.DzDisableDevice,
            InstructionTexts.DisableDzDevice,
            InstructionOrder.Dz);

        return new OperationDecision(true, item);
    }
}
