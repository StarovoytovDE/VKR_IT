using ApplicationLayer.InstructionGeneration.Operations;

namespace ApplicationLayer.InstructionGeneration.Registry;

public sealed class VLOutFieldClosingInstructionSet
{
    public IReadOnlyList<IOperation> Operations { get; } = new List<IOperation>
    {
        new DfzOperation(),
        new DzlOperation(),
        new DzOperation(),
        new OapvOperation(),
        new TapvOperation(),
        new UpaskOperation(),
        new SwitchLineVtToReserveOperation(),
        new DisconnectLineCtFromDzoOperation(),
        new MtzoShinovkaAtoBOperation(),
        new SwitchBusVtToReserveOperation()
    };
}
