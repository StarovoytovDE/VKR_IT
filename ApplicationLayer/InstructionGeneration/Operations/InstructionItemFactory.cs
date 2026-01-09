using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Models;

namespace ApplicationLayer.InstructionGeneration.Operations;

internal static class InstructionItemFactory
{
    internal static InstructionItem Create(
        LineOperationCriteria criteria,
        string code,
        string text,
        int order,
        InstructionItemType type = InstructionItemType.Action)
    {
        return new InstructionItem(
            code,
            text,
            text,
            order,
            type,
            criteria.DeviceObjectId,
            criteria.Side);
    }
}
