using ApplicationLayer.InstructionGeneration.Models;
using ApplicationLayer.InstructionGeneration.Operations;
using Xunit;

namespace Tests.InstructionGeneration.Operations;

public class DzOneSideNoFieldOperationTests
{
    [Fact]
    public void Evaluate_NoLineVt_ReturnsNone()
    {
        var criteria = TestCriteriaFactory.Create(ActionCode.VlOutNoFieldClosing) with
        {
            HasDZ = true,
            DZEnabled = true,
            DZConnectedToLineVT = false,
        };

        var operation = new DzOneSideNoFieldOperation();
        var decision = operation.Evaluate(criteria);

        Assert.False(decision.AddItem);
    }

    [Fact]
    public void Evaluate_OnlyFunction_DisablesDevice()
    {
        var criteria = TestCriteriaFactory.Create(ActionCode.VlOutNoFieldClosing) with
        {
            HasDZ = true,
            DZEnabled = true,
            DZConnectedToLineVT = true,
            IsOnlyFunctionInDevice = true,
        };

        var operation = new DzOneSideNoFieldOperation();
        var decision = operation.Evaluate(criteria);

        Assert.Equal(InstructionCodes.DzDisableDevice, decision.Item?.Code);
    }

    [Fact]
    public void Evaluate_NotOnlyFunction_DisablesFunction()
    {
        var criteria = TestCriteriaFactory.Create(ActionCode.VlOutNoFieldClosing) with
        {
            HasDZ = true,
            DZEnabled = true,
            DZConnectedToLineVT = true,
            IsOnlyFunctionInDevice = false,
        };

        var operation = new DzOneSideNoFieldOperation();
        var decision = operation.Evaluate(criteria);

        Assert.Equal(InstructionCodes.DzDisableFunction, decision.Item?.Code);
    }
}
