using ApplicationLayer.InstructionGeneration.Models;
using ApplicationLayer.InstructionGeneration.Operations;
using Xunit;

namespace Tests.InstructionGeneration.Operations;

public class DisconnectLineCtFromDzoOperationTests
{
    [Fact]
    public void Evaluate_NotRequired_ReturnsNone()
    {
        var criteria = TestCriteriaFactory.Create(ActionCode.VlOutFieldClosing) with
        {
            NeedDisconnectLineCTFromDZO = false,
        };

        var operation = new DisconnectLineCtFromDzoOperation();
        var decision = operation.Evaluate(criteria);

        Assert.False(decision.AddItem);
    }

    [Fact]
    public void Evaluate_Required_ReturnsInstruction()
    {
        var criteria = TestCriteriaFactory.Create(ActionCode.VlOutFieldClosing) with
        {
            NeedDisconnectLineCTFromDZO = true,
        };

        var operation = new DisconnectLineCtFromDzoOperation();
        var decision = operation.Evaluate(criteria);

        Assert.Equal(InstructionCodes.DisconnectLineCtFromDzo, decision.Item?.Code);
    }
}
