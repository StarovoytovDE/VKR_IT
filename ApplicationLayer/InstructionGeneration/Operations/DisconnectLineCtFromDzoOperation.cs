using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Models;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Decision tree for disconnecting line CT from DZO.
/// </summary>
public sealed class DisconnectLineCtFromDzoOperation : IOperation
{
    /// <inheritdoc />
    public OperationDecision Evaluate(LineOperationCriteria criteria)
    {
        if (!criteria.NeedDisconnectLineCTFromDZO)
        {
            return OperationDecision.None;
        }

        var item = InstructionItemFactory.Create(
            criteria,
            InstructionCodes.DisconnectLineCtFromDzo,
            InstructionTexts.DisconnectLineCtFromDzo,
            InstructionOrder.DisconnectCtDzo);

        return new OperationDecision(true, item);
    }
}
