using ApplicationLayer.InstructionGeneration.Context;
using ApplicationLayer.InstructionGeneration.Generation;
using ApplicationLayer.InstructionGeneration.Models;
using ApplicationLayer;
using Xunit;

namespace Tests;

public class InstructionGenerationTests
{
    [Fact]
    public void Generate_WhenFieldClosingNotAllowed_ReturnsOnlyWarning()
    {
        var context = new InstructionContext(
            actionCode: "VL_OUT",
            isFieldClosing: true,
            isFieldClosingAllowed: false,
            hasDfz: true,
            dfzNormalEnabled: true,
            dfzEnabled: true,
            connectedToLineCt: true,
            connectedToLineVt: true,
            needSwitchVtToReserve: true,
            bothCbctReverseSide: true,
            hasDzl: true,
            dzlNormalEnabled: true,
            dzlEnabled: true,
            hasDz: true,
            dzNormalEnabled: true,
            dzEnabled: true,
            needDisableOapv: true,
            needDisableOapvVl: true,
            needDisableTapv: true,
            needDisableTapvInDevice: true,
            needDisableUpaskReceivers: true,
            needDisconnectLineCtFromDzo: true,
            needMtzoShinovkaAtoB: true,
            needSwitchBusVtToReserve: true);

        var generator = new VLOutFieldClosingInstructionGenerator();

        var result = generator.Generate(context);

        Assert.Single(result.Items);
        Assert.Equal(InstructionCodes.FieldClosingNotAllowed, result.Items[0].Code);
        Assert.Equal(InstructionItemType.Warning, result.Items[0].Type);
    }

    [Fact]
    public void Generate_WhenFieldClosingAllowed_IncludesDfzItemsWithoutReverseSide()
    {
        var context = new InstructionContext(
            actionCode: "VL_OUT",
            isFieldClosing: true,
            isFieldClosingAllowed: true,
            hasDfz: true,
            dfzNormalEnabled: true,
            dfzEnabled: true,
            connectedToLineCt: true,
            connectedToLineVt: true,
            needSwitchVtToReserve: true,
            bothCbctReverseSide: true,
            hasDzl: false,
            dzlNormalEnabled: false,
            dzlEnabled: false,
            hasDz: false,
            dzNormalEnabled: false,
            dzEnabled: false,
            needDisableOapv: false,
            needDisableOapvVl: false,
            needDisableTapv: false,
            needDisableTapvInDevice: false,
            needDisableUpaskReceivers: false,
            needDisconnectLineCtFromDzo: false,
            needMtzoShinovkaAtoB: false,
            needSwitchBusVtToReserve: false);

        var generator = new VLOutFieldClosingInstructionGenerator();

        var result = generator.Generate(context);
        var codes = result.Items.Select(item => item.Code).ToList();

        Assert.Contains(InstructionCodes.DfzHasFunction, codes);
        Assert.Contains(InstructionCodes.DfzNormalEnabled, codes);
        Assert.Contains(InstructionCodes.DfzEnabled, codes);
        Assert.Contains(InstructionCodes.DfzConnectedToLineCt, codes);
        Assert.Contains(InstructionCodes.DfzConnectedToLineVt, codes);
        Assert.Contains(InstructionCodes.DfzSwitchVtToReserve, codes);
        Assert.DoesNotContain("BOTH_CB_CT_REVERSE_SIDE", codes);
    }
}