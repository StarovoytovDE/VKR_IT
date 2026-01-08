using ApplicationLayer.InstructionGeneration.Context;
using ApplicationLayer.InstructionGeneration.Models;
using ApplicationLayer.InstructionGeneration.Specifications;

namespace ApplicationLayer.InstructionGeneration.Operations;

public sealed class TapvOperation : IOperation
{
    private readonly ISpecification<InstructionContext> _needDisableTapv =
        new PredicateSpecification<InstructionContext>(ctx => ctx.NeedDisableTAPV);

    public IEnumerable<InstructionItem> Build(InstructionContext ctx)
    {
        if (!_needDisableTapv.IsSatisfiedBy(ctx))
        {
            yield break;
        }

        yield return new InstructionItem(
            InstructionCodes.TapvDisable,
            "Вывод ТАПВ",
            "Вывести ТАПВ выключателей ВЛ в данном устройстве?",
            500);
    }
}
