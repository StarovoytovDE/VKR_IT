using ApplicationLayer.InstructionGeneration.Context;
using ApplicationLayer.InstructionGeneration.Models;
using ApplicationLayer.InstructionGeneration.Specifications;

namespace ApplicationLayer.InstructionGeneration.Operations;

public sealed class MtzoShinovkaAtoBOperation : IOperation
{
    private readonly ISpecification<InstructionContext> _needMtzoShinovkaAtoB =
        new PredicateSpecification<InstructionContext>(ctx => ctx.NeedMTZOshinovkaAtoB);

    public IEnumerable<InstructionItem> Build(InstructionContext ctx)
    {
        if (!_needMtzoShinovkaAtoB.IsSatisfiedBy(ctx))
        {
            yield break;
        }

        yield return new InstructionItem(
            InstructionCodes.MtzoShinovkaAtoB,
            "МТЗ ошиновки А->B",
            "При выполнении работ на ошиновке А необходимо произвести перевод цепей МТЗ ошиновки А на ошиновку B согласно схеме и оперативным указаниям.",
            900);
    }
}
