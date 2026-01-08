using ApplicationLayer.InstructionGeneration.Context;
using ApplicationLayer.InstructionGeneration.Models;
using ApplicationLayer.InstructionGeneration.Specifications;

namespace ApplicationLayer.InstructionGeneration.Operations;

public sealed class DfzOperation : IOperation
{
    private readonly ISpecification<InstructionContext> _hasDfz =
        new PredicateSpecification<InstructionContext>(ctx => ctx.HasDFZ);

    private readonly ISpecification<InstructionContext> _dfzNormalEnabled =
        new PredicateSpecification<InstructionContext>(ctx => ctx.DFZNormalEnabled);

    private readonly ISpecification<InstructionContext> _dfzEnabled =
        new PredicateSpecification<InstructionContext>(ctx => ctx.DFZEnabled);

    private readonly ISpecification<InstructionContext> _connectedToLineCt =
        new PredicateSpecification<InstructionContext>(ctx => ctx.ConnectedToLineCT);

    private readonly ISpecification<InstructionContext> _connectedToLineVt =
        new PredicateSpecification<InstructionContext>(ctx => ctx.ConnectedToLineVT);

    private readonly ISpecification<InstructionContext> _needSwitchVtToReserve =
        new PredicateSpecification<InstructionContext>(ctx => ctx.NeedSwitchVTToReserve);

    public IEnumerable<InstructionItem> Build(InstructionContext ctx)
    {
        const int baseOrder = 100;

        if (_hasDfz.IsSatisfiedBy(ctx))
        {
            yield return new InstructionItem(
                InstructionCodes.DfzHasFunction,
                "Вывод ДФЗ",
                "В устройстве есть функция ДФЗ?",
                baseOrder);
        }

        if (_dfzNormalEnabled.IsSatisfiedBy(ctx))
        {
            yield return new InstructionItem(
                InstructionCodes.DfzNormalEnabled,
                "Вывод ДФЗ",
                "Функция нормально введена?",
                baseOrder + 1);
        }

        if (_dfzEnabled.IsSatisfiedBy(ctx))
        {
            yield return new InstructionItem(
                InstructionCodes.DfzEnabled,
                "Вывод ДФЗ",
                "Функция введена?",
                baseOrder + 2);
        }

        if (_connectedToLineCt.IsSatisfiedBy(ctx))
        {
            yield return new InstructionItem(
                InstructionCodes.DfzConnectedToLineCt,
                "Вывод ДФЗ",
                "Устройство подключено к линейному ТТ?",
                baseOrder + 3);
        }

        if (_connectedToLineVt.IsSatisfiedBy(ctx))
        {
            yield return new InstructionItem(
                InstructionCodes.DfzConnectedToLineVt,
                "Вывод ДФЗ",
                "Устройство подключено к линейному ТН?",
                baseOrder + 4);
        }

        if (_needSwitchVtToReserve.IsSatisfiedBy(ctx))
        {
            yield return new InstructionItem(
                InstructionCodes.DfzSwitchVtToReserve,
                "Вывод ДФЗ",
                "Производится перевод цепей напряжения на резервный ТН?",
                baseOrder + 5);
        }
    }
}
