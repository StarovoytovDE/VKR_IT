using ApplicationLayer.InstructionGeneration.Models;
using ApplicationLayer.InstructionGeneration.Operations;
using Xunit;

namespace Tests.InstructionGeneration.Operations;

public class TapvGenericOperationTests
{
    [Fact]
    public void Evaluate_NoTapv_ReturnsNone()
    {
        var criteria = TestCriteriaFactory.Create(ActionCode.VlOutFieldClosing) with
        {
            HasTAPV = false,
        };

        var operation = new TapvGenericOperation();
        var decision = operation.Evaluate(criteria);

        Assert.False(decision.AddItem);
    }

    [Fact]
    public void Evaluate_Enabled_DisablesFunction()
    {
        var criteria = TestCriteriaFactory.Create(ActionCode.VlOutFieldClosing) with
        {
            HasTAPV = true,
            TAPVEnabled = true,
        };

        var operation = new TapvGenericOperation();
        var decision = operation.Evaluate(criteria);

        Assert.Equal(InstructionCodes.TapvDisableFunction, decision.Item?.Code);
    }
}
