using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Models;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Decision tree for MTZO shinovka A to B switching.
/// </summary>
public sealed class MtzoShinovkaAtoBOperation : IOperation
{
    /// <inheritdoc />
    public OperationDecision Evaluate(LineOperationCriteria criteria)
    {
        if (!criteria.NeedMtzoShinovkaAtoB)
        {
            return OperationDecision.None;
        }

        if (!criteria.CtRemainsEnergizedOnThisSide)
        {
            return OperationDecision.None;
        }

        var item = InstructionItemFactory.Create(
            criteria,
            InstructionCodes.MtzoShinovkaAToB,
            InstructionTexts.MtzoShinovkaAToB,
            InstructionOrder.Mtzo);

        return new OperationDecision(true, item);
    }
}
