using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Generation;
using ApplicationLayer.InstructionGeneration.Models;
using Xunit;

namespace Tests.InstructionGeneration.Generation;

public class InstructionGeneratorTests
{
    [Fact]
    public void Generate_MultipleDevicesAcrossSides_ReturnsOrderedItems()
    {
        var criteria = new List<LineOperationCriteria>
        {
            new("VL-1", SideOfLine.Begin, 1, ActionCode.VlOutFieldClosing)
            {
                HasDFZ = true,
                DFZEnabled = true,
                DFZConnectedToLineCT = false,
                IsOnlyFunctionInDevice = false,
            },
            new("VL-1", SideOfLine.Begin, 2, ActionCode.VlOutFieldClosing)
            {
                HasDZ = true,
                DZEnabled = true,
                DZConnectedToLineVT = true,
            },
            new("VL-1", SideOfLine.End, 3, ActionCode.VlOutFieldClosing)
            {
                HasOAPV = true,
                OAPVEnabled = true,
            },
        };

        var generator = new InstructionGenerator(ActionCode.VlOutFieldClosing);
        var result = generator.Generate(criteria);

        Assert.Equal(3, result.Items.Count);
        Assert.Equal(new[]
        {
            InstructionCodes.DfzDisableFunction,
            InstructionCodes.DzDisableFunction,
            InstructionCodes.OapvDisableFunction,
        }, result.Items.Select(item => item.Code));
    }

    [Fact]
    public void Generate_BothSwitchFlags_ReturnsTwoSwitchInstructions()
    {
        var criteria = new List<LineOperationCriteria>
        {
            new("VL-1", SideOfLine.Begin, 1, ActionCode.VlOutFieldClosing)
            {
                NeedSwitchLineVTToReserve = true,
                NeedSwitchBusVTToReserve = true,
            },
        };

        var generator = new InstructionGenerator(ActionCode.VlOutFieldClosing);
        var result = generator.Generate(criteria);

        Assert.Equal(2, result.Items.Count);
        Assert.Contains(result.Items, item => item.Code == InstructionCodes.SwitchLineVtToReserve);
        Assert.Contains(result.Items, item => item.Code == InstructionCodes.SwitchBusVtToReserve);
    }

    [Fact]
    public void Generate_WhenSpecificSwitchExists_RemovesFollowInstruction()
    {
        var criteria = new List<LineOperationCriteria>
        {
            new("VL-1", SideOfLine.Begin, 1, ActionCode.VlOutFieldClosing)
            {
                HasDFZ = true,
                DFZEnabled = true,
                DFZConnectedToLineCT = true,
                DFZConnectedToLineVT = true,
                NeedSwitchLineVTToReserve = true,
            },
        };

        var generator = new InstructionGenerator(ActionCode.VlOutFieldClosing);
        var result = generator.Generate(criteria);

        Assert.Single(result.Items);
        Assert.DoesNotContain(result.Items, item => item.Code == InstructionCodes.FollowVoltageSwitchInstructions);
        Assert.Contains(result.Items, item => item.Code == InstructionCodes.SwitchLineVtToReserve);
    }
}
