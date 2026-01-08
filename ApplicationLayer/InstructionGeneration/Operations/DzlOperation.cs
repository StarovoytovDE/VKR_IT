using ApplicationLayer.InstructionGeneration.Context;
using ApplicationLayer.InstructionGeneration.Models;
using ApplicationLayer.InstructionGeneration.Specifications;

namespace ApplicationLayer.InstructionGeneration.Operations;

public sealed class DzlOperation : IOperation
{
    private readonly ISpecification<InstructionContext> _hasDzl =
        new PredicateSpecification<InstructionContext>(ctx => ctx.HasDZL);

    private readonly ISpecification<InstructionContext> _dzlNormalEnabled =
        new PredicateSpecification<InstructionContext>(ctx => ctx.DZLNormalEnabled);

    private readonly ISpecification<InstructionContext> _dzlEnabled =
        new PredicateSpecification<InstructionContext>(ctx => ctx.DZLEnabled);

    public IEnumerable<InstructionItem> Build(InstructionContext ctx)
    {
        const int baseOrder = 200;

        if (_hasDzl.IsSatisfiedBy(ctx))
        {
            yield return new InstructionItem(
                InstructionCodes.DzlHasFunction,
                "Вывод ДЗЛ",
                "В устройстве есть функция ДЗЛ?",
                baseOrder);
        }

        if (_dzlNormalEnabled.IsSatisfiedBy(ctx))
        {
            yield return new InstructionItem(
                InstructionCodes.DzlNormalEnabled,
                "Вывод ДЗЛ",
                "Функция нормально введена?",
                baseOrder + 1);
        }

        if (_dzlEnabled.IsSatisfiedBy(ctx))
        {
            yield return new InstructionItem(
                InstructionCodes.DzlEnabled,
                "Вывод ДЗЛ",
                "Функция введена?",
                baseOrder + 2);
        }
    }
}
