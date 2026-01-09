using ApplicationLayer.InstructionGeneration.Models;
using ApplicationLayer.InstructionGeneration.Operations;
using Xunit;

namespace Tests.InstructionGeneration.Operations;

public class OapvNoFieldClosingOperationTests
{
    [Fact]
    public void Evaluate_NotReverseSide_ReturnsNone()
    {
        var criteria = TestCriteriaFactory.Create(ActionCode.VlOutNoFieldClosing) with
        {
            HasOAPV = true,
            OAPVEnabled = true,
            BothCbctReverseSide = false,
        };

        var operation = new OapvNoFieldClosingOperation();
        var decision = operation.Evaluate(criteria);

        Assert.False(decision.AddItem);
    }

    [Fact]
    public void Evaluate_ReverseSide_DisablesFunction()
    {
        var criteria = TestCriteriaFactory.Create(ActionCode.VlOutNoFieldClosing) with
        {
            HasOAPV = true,
            OAPVEnabled = true,
            BothCbctReverseSide = true,
        };

        var operation = new OapvNoFieldClosingOperation();
        var decision = operation.Evaluate(criteria);

        Assert.Equal(InstructionCodes.OapvDisableFunction, decision.Item?.Code);
    }
}
