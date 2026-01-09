using ApplicationLayer.InstructionGeneration.Models;
using ApplicationLayer.InstructionGeneration.Operations;
using Xunit;

namespace Tests.InstructionGeneration.Operations;

public class DfzNoFieldClosingOperationTests
{
    [Fact]
    public void Evaluate_Disabled_ReturnsNone()
    {
        var criteria = TestCriteriaFactory.Create(ActionCode.VlOutNoFieldClosing) with
        {
            HasDFZ = true,
            DFZEnabled = false,
        };

        var operation = new DfzNoFieldClosingOperation();
        var decision = operation.Evaluate(criteria);

        Assert.False(decision.AddItem);
    }

    [Fact]
    public void Evaluate_ReverseSideAndOnlyFunction_DisablesDevice()
    {
        var criteria = TestCriteriaFactory.Create(ActionCode.VlOutNoFieldClosing) with
        {
            HasDFZ = true,
            DFZEnabled = true,
            BothCbctReverseSide = true,
            IsOnlyFunctionInDevice = true,
        };

        var operation = new DfzNoFieldClosingOperation();
        var decision = operation.Evaluate(criteria);

        Assert.True(decision.AddItem);
        Assert.Equal(InstructionCodes.DfzDisableDevice, decision.Item?.Code);
    }

    [Fact]
    public void Evaluate_ReverseSideAndNotOnlyFunction_DisablesFunction()
    {
        var criteria = TestCriteriaFactory.Create(ActionCode.VlOutNoFieldClosing) with
        {
            HasDFZ = true,
            DFZEnabled = true,
            BothCbctReverseSide = true,
            IsOnlyFunctionInDevice = false,
        };

        var operation = new DfzNoFieldClosingOperation();
        var decision = operation.Evaluate(criteria);

        Assert.Equal(InstructionCodes.DfzDisableFunction, decision.Item?.Code);
    }
}
