namespace ApplicationLayer.InstructionGeneration.Models;

public sealed class InstructionResultDto
{
    public InstructionResultDto(IReadOnlyList<InstructionItem> items)
    {
        Items = items;
    }

    public IReadOnlyList<InstructionItem> Items { get; }
}
