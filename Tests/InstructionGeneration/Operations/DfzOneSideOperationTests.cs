using ApplicationLayer.InstructionGeneration.Models;
using ApplicationLayer.InstructionGeneration.Operations;
using Xunit;

namespace Tests.InstructionGeneration.Operations;

public class DfzOneSideOperationTests
{
    [Fact]
    public void Evaluate_NoConnectionToCt_ReturnsNone()
    {
        var criteria = TestCriteriaFactory.Create(ActionCode.VlOutOneSide) with
        {
            HasDFZ = true,
            DFZEnabled = true,
            DFZConnectedToLineCT = false,
        };

        var operation = new DfzOneSideOperation();
        var decision = operation.Evaluate(criteria);

        Assert.False(decision.AddItem);
    }

    [Fact]
    public void Evaluate_OnlyFunction_DisablesDevice()
    {
        var criteria = TestCriteriaFactory.Create(ActionCode.VlOutOneSide) with
        {
            HasDFZ = true,
            DFZEnabled = true,
            DFZConnectedToLineCT = true,
            IsOnlyFunctionInDevice = true,
        };

        var operation = new DfzOneSideOperation();
        var decision = operation.Evaluate(criteria);

        Assert.Equal(InstructionCodes.DfzDisableDevice, decision.Item?.Code);
    }

    [Fact]
    public void Evaluate_NotOnlyFunction_DisablesFunction()
    {
        var criteria = TestCriteriaFactory.Create(ActionCode.VlOutOneSide) with
        {
            HasDFZ = true,
            DFZEnabled = true,
            DFZConnectedToLineCT = true,
            IsOnlyFunctionInDevice = false,
        };

        var operation = new DfzOneSideOperation();
        var decision = operation.Evaluate(criteria);

        Assert.Equal(InstructionCodes.DfzDisableFunction, decision.Item?.Code);
    }
}
