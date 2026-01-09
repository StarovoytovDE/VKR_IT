using ApplicationLayer.InstructionGeneration.Models;
using ApplicationLayer.InstructionGeneration.Operations;
using Xunit;

namespace Tests.InstructionGeneration.Operations;

public class MtzoShinovkaAtoBOperationTests
{
    [Fact]
    public void Evaluate_NotRequired_ReturnsNone()
    {
        var criteria = TestCriteriaFactory.Create(ActionCode.VlOutFieldClosing) with
        {
            NeedMtzoShinovkaAtoB = false,
        };

        var operation = new MtzoShinovkaAtoBOperation();
        var decision = operation.Evaluate(criteria);

        Assert.False(decision.AddItem);
    }

    [Fact]
    public void Evaluate_CtNotEnergized_ReturnsNone()
    {
        var criteria = TestCriteriaFactory.Create(ActionCode.VlOutFieldClosing) with
        {
            NeedMtzoShinovkaAtoB = true,
            CtRemainsEnergizedOnThisSide = false,
        };

        var operation = new MtzoShinovkaAtoBOperation();
        var decision = operation.Evaluate(criteria);

        Assert.False(decision.AddItem);
    }

    [Fact]
    public void Evaluate_RequiredAndEnergized_ReturnsInstruction()
    {
        var criteria = TestCriteriaFactory.Create(ActionCode.VlOutFieldClosing) with
        {
            NeedMtzoShinovkaAtoB = true,
            CtRemainsEnergizedOnThisSide = true,
        };

        var operation = new MtzoShinovkaAtoBOperation();
        var decision = operation.Evaluate(criteria);

        Assert.Equal(InstructionCodes.MtzoShinovkaAToB, decision.Item?.Code);
    }
}
