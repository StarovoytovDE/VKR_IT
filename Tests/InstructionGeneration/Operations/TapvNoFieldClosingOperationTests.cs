using ApplicationLayer.InstructionGeneration.Models;
using ApplicationLayer.InstructionGeneration.Operations;
using Xunit;

namespace Tests.InstructionGeneration.Operations;

public class TapvNoFieldClosingOperationTests
{
    [Fact]
    public void Evaluate_NotReverseSide_ReturnsNone()
    {
        var criteria = TestCriteriaFactory.Create(ActionCode.VlOutNoFieldClosing) with
        {
            HasTAPV = true,
            TAPVEnabled = true,
            BothCbctReverseSide = false,
        };

        var operation = new TapvNoFieldClosingOperation();
        var decision = operation.Evaluate(criteria);

        Assert.False(decision.AddItem);
    }

    [Fact]
    public void Evaluate_ReverseSide_DisablesFunction()
    {
        var criteria = TestCriteriaFactory.Create(ActionCode.VlOutNoFieldClosing) with
        {
            HasTAPV = true,
            TAPVEnabled = true,
            BothCbctReverseSide = true,
        };

        var operation = new TapvNoFieldClosingOperation();
        var decision = operation.Evaluate(criteria);

        Assert.Equal(InstructionCodes.TapvDisableFunction, decision.Item?.Code);
    }
}
