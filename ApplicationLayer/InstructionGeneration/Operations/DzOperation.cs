using ApplicationLayer.InstructionGeneration.Context;
using ApplicationLayer.InstructionGeneration.Models;
using ApplicationLayer.InstructionGeneration.Specifications;

namespace ApplicationLayer.InstructionGeneration.Operations;

public sealed class DzOperation : IOperation
{
    private readonly ISpecification<InstructionContext> _hasDz =
        new PredicateSpecification<InstructionContext>(ctx => ctx.HasDZ);

    private readonly ISpecification<InstructionContext> _dzNormalEnabled =
        new PredicateSpecification<InstructionContext>(ctx => ctx.DZNormalEnabled);

    private readonly ISpecification<InstructionContext> _dzEnabled =
        new PredicateSpecification<InstructionContext>(ctx => ctx.DZEnabled);

    private readonly ISpecification<InstructionContext> _connectedToLineVt =
        new PredicateSpecification<InstructionContext>(ctx => ctx.ConnectedToLineVT);

    private readonly ISpecification<InstructionContext> _needSwitchVtToReserve =
        new PredicateSpecification<InstructionContext>(ctx => ctx.NeedSwitchVTToReserve);

    public IEnumerable<InstructionItem> Build(InstructionContext ctx)
    {
        const int baseOrder = 300;

        if (_hasDz.IsSatisfiedBy(ctx))
        {
            yield return new InstructionItem(
                InstructionCodes.DzHasFunction,
                "Вывод ДЗ",
                "В устройстве есть функция ДЗ?",
                baseOrder);
        }

        if (_dzNormalEnabled.IsSatisfiedBy(ctx))
        {
            yield return new InstructionItem(
                InstructionCodes.DzNormalEnabled,
                "Вывод ДЗ",
                "Функция нормально введена?",
                baseOrder + 1);
        }

        if (_dzEnabled.IsSatisfiedBy(ctx))
        {
            yield return new InstructionItem(
                InstructionCodes.DzEnabled,
                "Вывод ДЗ",
                "Функция введена?",
                baseOrder + 2);
        }

        if (_connectedToLineVt.IsSatisfiedBy(ctx))
        {
            yield return new InstructionItem(
                InstructionCodes.DzConnectedToLineVt,
                "Вывод ДЗ",
                "Устройство подключено к линейному ТН?",
                baseOrder + 3);
        }

        if (_needSwitchVtToReserve.IsSatisfiedBy(ctx))
        {
            yield return new InstructionItem(
                InstructionCodes.DzSwitchVtToReserve,
                "Вывод ДЗ",
                "Производится перевод цепей напряжения на резервный ТН?",
                baseOrder + 4);
        }
    }
}
