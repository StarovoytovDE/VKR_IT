using ApplicationLayer.InstructionGeneration.Models;
using ApplicationLayer.InstructionGeneration.Operations;
using Xunit;

namespace Tests.InstructionGeneration.Operations;

public class DfzFieldClosingOperationTests
{
    [Fact]
    public void Evaluate_NoDfz_ReturnsNone()
    {
        var criteria = TestCriteriaFactory.Create(ActionCode.VlOutFieldClosing) with
        {
            HasDFZ = false,
        };

        var operation = new DfzFieldClosingOperation();
        var decision = operation.Evaluate(criteria);

        Assert.False(decision.AddItem);
        Assert.Null(decision.Item);
    }

    [Fact]
    public void Evaluate_NotConnectedToCtAndNotOnlyFunction_DisablesFunction()
    {
        var criteria = TestCriteriaFactory.Create(ActionCode.VlOutFieldClosing) with
        {
            HasDFZ = true,
            DFZEnabled = true,
            DFZConnectedToLineCT = false,
            IsOnlyFunctionInDevice = false,
        };

        var operation = new DfzFieldClosingOperation();
        var decision = operation.Evaluate(criteria);

        Assert.True(decision.AddItem);
        Assert.Equal(InstructionCodes.DfzDisableFunction, decision.Item?.Code);
    }

    [Fact]
    public void Evaluate_NeedSwitchVoltage_ReturnsFollowInstructions()
    {
        var criteria = TestCriteriaFactory.Create(ActionCode.VlOutFieldClosing) with
        {
            HasDFZ = true,
            DFZEnabled = true,
            DFZConnectedToLineCT = true,
            DFZConnectedToLineVT = true,
            NeedSwitchLineVTToReserve = true,
        };

        var operation = new DfzFieldClosingOperation();
        var decision = operation.Evaluate(criteria);

        Assert.True(decision.AddItem);
        Assert.Equal(InstructionCodes.FollowVoltageSwitchInstructions, decision.Item?.Code);
    }
}
