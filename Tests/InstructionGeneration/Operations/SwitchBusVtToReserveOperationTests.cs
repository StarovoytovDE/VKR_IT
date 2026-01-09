using ApplicationLayer.InstructionGeneration.Models;
using ApplicationLayer.InstructionGeneration.Operations;
using Xunit;

namespace Tests.InstructionGeneration.Operations;

public class SwitchBusVtToReserveOperationTests
{
    [Fact]
    public void Evaluate_NotRequired_ReturnsNone()
    {
        var criteria = TestCriteriaFactory.Create(ActionCode.VlOutFieldClosing) with
        {
            NeedSwitchBusVTToReserve = false,
        };

        var operation = new SwitchBusVtToReserveOperation();
        var decision = operation.Evaluate(criteria);

        Assert.False(decision.AddItem);
    }

    [Fact]
    public void Evaluate_Required_ReturnsInstruction()
    {
        var criteria = TestCriteriaFactory.Create(ActionCode.VlOutFieldClosing) with
        {
            NeedSwitchBusVTToReserve = true,
        };

        var operation = new SwitchBusVtToReserveOperation();
        var decision = operation.Evaluate(criteria);

        Assert.Equal(InstructionCodes.SwitchBusVtToReserve, decision.Item?.Code);
    }
}
