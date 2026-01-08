using ApplicationLayer.InstructionGeneration.Context;
using ApplicationLayer.InstructionGeneration.Models;
using ApplicationLayer.InstructionGeneration.Specifications;

namespace ApplicationLayer.InstructionGeneration.Operations;

public sealed class DisconnectLineCtFromDzoOperation : IOperation
{
    private readonly ISpecification<InstructionContext> _needDisconnectLineCtFromDzo =
        new PredicateSpecification<InstructionContext>(ctx => ctx.NeedDisconnectLineCTFromDZO);

    public IEnumerable<InstructionItem> Build(InstructionContext ctx)
    {
        if (!_needDisconnectLineCtFromDzo.IsSatisfiedBy(ctx))
        {
            yield break;
        }

        yield return new InstructionItem(
            InstructionCodes.DisconnectLineCtFromDzo,
            "Отключение цепей ТТ",
            "Токовые цепи линейного ТТ отключить от ДЗО.",
            800);
    }
}
