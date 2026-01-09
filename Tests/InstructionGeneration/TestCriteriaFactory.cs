using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Models;

namespace Tests.InstructionGeneration;

internal static class TestCriteriaFactory
{
    internal static LineOperationCriteria Create(ActionCode actionCode)
    {
        return new LineOperationCriteria("VL-1", SideOfLine.Begin, 1, actionCode);
    }
}