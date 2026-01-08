using ApplicationLayer.InstructionGeneration.Context;
using ApplicationLayer.InstructionGeneration.Models;
using ApplicationLayer.InstructionGeneration.Specifications;

namespace ApplicationLayer.InstructionGeneration.Operations;

public sealed class OapvOperation : IOperation
{
    private readonly ISpecification<InstructionContext> _needDisableOapv =
        new PredicateSpecification<InstructionContext>(ctx => ctx.NeedDisableOAPV);

    public IEnumerable<InstructionItem> Build(InstructionContext ctx)
    {
        if (!_needDisableOapv.IsSatisfiedBy(ctx))
        {
            yield break;
        }

        yield return new InstructionItem(
            InstructionCodes.OapvDisable,
            "Вывод ОАПВ",
            "Производится вывод ОАПВ?",
            400);
    }
}
