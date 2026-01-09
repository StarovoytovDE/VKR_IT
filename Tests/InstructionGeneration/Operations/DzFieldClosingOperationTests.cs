using ApplicationLayer.InstructionGeneration.Models;
using ApplicationLayer.InstructionGeneration.Operations;
using Xunit;

namespace Tests.InstructionGeneration.Operations;

public class DzFieldClosingOperationTests
{
    [Fact]
    public void Evaluate_NoDz_ReturnsNone()
    {
        var criteria = TestCriteriaFactory.Create(ActionCode.VlOutFieldClosing) with
        {
            HasDZ = false,
        };

        var operation = new DzFieldClosingOperation();
        var decision = operation.Evaluate(criteria);

        Assert.False(decision.AddItem);
    }

    [Fact]
    public void Evaluate_SwitchNeeded_ReturnsFollowInstructions()
    {
        var criteria = TestCriteriaFactory.Create(ActionCode.VlOutFieldClosing) with
        {
            HasDZ = true,
            DZEnabled = true,
            DZConnectedToLineVT = true,
            NeedSwitchBusVTToReserve = true,
        };

        var operation = new DzFieldClosingOperation();
        var decision = operation.Evaluate(criteria);

        Assert.True(decision.AddItem);
        Assert.Equal(InstructionCodes.FollowVoltageSwitchInstructions, decision.Item?.Code);
    }

    [Fact]
    public void Evaluate_Enabled_DisablesFunction()
    {
        var criteria = TestCriteriaFactory.Create(ActionCode.VlOutFieldClosing) with
        {
            HasDZ = true,
            DZEnabled = true,
            DZConnectedToLineVT = true,
        };

        var operation = new DzFieldClosingOperation();
        var decision = operation.Evaluate(criteria);

        Assert.Equal(InstructionCodes.DzDisableFunction, decision.Item?.Code);
    }
}
