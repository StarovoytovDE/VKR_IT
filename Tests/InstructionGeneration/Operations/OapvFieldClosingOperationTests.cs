using ApplicationLayer.InstructionGeneration.Models;
using ApplicationLayer.InstructionGeneration.Operations;
using Xunit;

namespace Tests.InstructionGeneration.Operations;

public class OapvFieldClosingOperationTests
{
    [Fact]
    public void Evaluate_NoOapv_ReturnsNone()
    {
        var criteria = TestCriteriaFactory.Create(ActionCode.VlOutFieldClosing) with
        {
            HasOAPV = false,
        };

        var operation = new OapvFieldClosingOperation();
        var decision = operation.Evaluate(criteria);

        Assert.False(decision.AddItem);
    }

    [Fact]
    public void Evaluate_Enabled_DisablesFunction()
    {
        var criteria = TestCriteriaFactory.Create(ActionCode.VlOutFieldClosing) with
        {
            HasOAPV = true,
            OAPVEnabled = true,
        };

        var operation = new OapvFieldClosingOperation();
        var decision = operation.Evaluate(criteria);

        Assert.Equal(InstructionCodes.OapvDisableFunction, decision.Item?.Code);
    }
}
