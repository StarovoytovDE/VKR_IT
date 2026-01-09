using ApplicationLayer.InstructionGeneration.Models;
using ApplicationLayer.InstructionGeneration.Operations;
using Xunit;

namespace Tests.InstructionGeneration.Operations;

public class DzNoFieldClosingOperationTests
{
    [Fact]
    public void Evaluate_NotReverseSide_ReturnsNone()
    {
        var criteria = TestCriteriaFactory.Create(ActionCode.VlOutNoFieldClosing) with
        {
            HasDZ = true,
            DZEnabled = true,
            BothCbctReverseSide = false,
        };

        var operation = new DzNoFieldClosingOperation();
        var decision = operation.Evaluate(criteria);

        Assert.False(decision.AddItem);
    }

    [Fact]
    public void Evaluate_ReverseSide_DisablesFunction()
    {
        var criteria = TestCriteriaFactory.Create(ActionCode.VlOutNoFieldClosing) with
        {
            HasDZ = true,
            DZEnabled = true,
            BothCbctReverseSide = true,
        };

        var operation = new DzNoFieldClosingOperation();
        var decision = operation.Evaluate(criteria);

        Assert.Equal(InstructionCodes.DzDisableFunction, decision.Item?.Code);
    }
}
