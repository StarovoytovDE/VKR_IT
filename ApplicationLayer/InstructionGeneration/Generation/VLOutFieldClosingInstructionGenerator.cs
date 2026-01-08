using ApplicationLayer.InstructionGeneration.Context;
using ApplicationLayer.InstructionGeneration.Models;
using ApplicationLayer.InstructionGeneration.Registry;

namespace ApplicationLayer.InstructionGeneration.Generation;

public sealed class VLOutFieldClosingInstructionGenerator : IInstructionGenerator
{
    private readonly VLOutFieldClosingInstructionSet _instructionSet = new();

    public InstructionResultDto Generate(InstructionContext ctx)
    {
        if (!ctx.IsFieldClosingAllowed)
        {
            var warningItem = new InstructionItem(
                InstructionCodes.FieldClosingNotAllowed,
                "Замыкание поля недопустимо",
                "Замыкание поля недопустимо. Генерация дальнейших операций прекращена.",
                0,
                InstructionItemType.Warning);

            return new InstructionResultDto(new List<InstructionItem> { warningItem });
        }

        var items = new List<InstructionItem>();
        foreach (var operation in _instructionSet.Operations)
        {
            items.AddRange(operation.Build(ctx));
        }

        return new InstructionResultDto(items.OrderBy(item => item.Order).ToList());
    }
}
