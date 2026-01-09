using ApplicationLayer.InstructionGeneration.Models;
using ApplicationLayer.InstructionGeneration.Operations;
using Xunit;

namespace Tests.InstructionGeneration.Operations;

public class UpaskFieldClosingOperationTests
{
    [Fact]
    public void Evaluate_NoNeed_ReturnsNone()
    {
        var criteria = TestCriteriaFactory.Create(ActionCode.VlOutFieldClosing) with
        {
            NeedDisableUpaskReceivers = false,
        };

        var operation = new UpaskFieldClosingOperation();
        var decision = operation.Evaluate(criteria);

        Assert.False(decision.AddItem);
    }

    [Fact]
    public void Evaluate_NeedDisable_ReturnsInstruction()
    {
        var criteria = TestCriteriaFactory.Create(ActionCode.VlOutFieldClosing) with
        {
            NeedDisableUpaskReceivers = true,
        };

        var operation = new UpaskFieldClosingOperation();
        var decision = operation.Evaluate(criteria);

        Assert.Equal(InstructionCodes.UpaskDisableReceivers, decision.Item?.Code);
    }
}
