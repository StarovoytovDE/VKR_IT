using ApplicationLayer.InstructionGeneration.Models;
using ApplicationLayer.InstructionGeneration.Operations;
using Xunit;

namespace Tests.InstructionGeneration.Operations;

public class SwitchLineVtToReserveOperationTests
{
    [Fact]
    public void Evaluate_NotRequired_ReturnsNone()
    {
        var criteria = TestCriteriaFactory.Create(ActionCode.VlOutFieldClosing) with
        {
            NeedSwitchLineVTToReserve = false,
        };

        var operation = new SwitchLineVtToReserveOperation();
        var decision = operation.Evaluate(criteria);

        Assert.False(decision.AddItem);
    }

    [Fact]
    public void Evaluate_Required_ReturnsInstruction()
    {
        var criteria = TestCriteriaFactory.Create(ActionCode.VlOutFieldClosing) with
        {
            NeedSwitchLineVTToReserve = true,
        };

        var operation = new SwitchLineVtToReserveOperation();
        var decision = operation.Evaluate(criteria);

        Assert.Equal(InstructionCodes.SwitchLineVtToReserve, decision.Item?.Code);
    }
}
