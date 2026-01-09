using ApplicationLayer.InstructionGeneration.Models;
using ApplicationLayer.InstructionGeneration.Operations;
using Xunit;

namespace Tests.InstructionGeneration.Operations;

public class DzlFieldClosingOperationTests
{
    [Fact]
    public void Evaluate_NoDzl_ReturnsNone()
    {
        var criteria = TestCriteriaFactory.Create(ActionCode.VlOutFieldClosing) with
        {
            HasDZL = false,
        };

        var operation = new DzlFieldClosingOperation();
        var decision = operation.Evaluate(criteria);

        Assert.False(decision.AddItem);
    }

    [Fact]
    public void Evaluate_Enabled_DisablesDevice()
    {
        var criteria = TestCriteriaFactory.Create(ActionCode.VlOutFieldClosing) with
        {
            HasDZL = true,
            DZLEnabled = true,
        };

        var operation = new DzlFieldClosingOperation();
        var decision = operation.Evaluate(criteria);

        Assert.True(decision.AddItem);
        Assert.Equal(InstructionCodes.DzlDisableDevice, decision.Item?.Code);
    }
}
