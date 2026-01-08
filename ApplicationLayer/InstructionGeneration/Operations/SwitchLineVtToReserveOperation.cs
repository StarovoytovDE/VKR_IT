using ApplicationLayer.InstructionGeneration.Context;
using ApplicationLayer.InstructionGeneration.Models;
using ApplicationLayer.InstructionGeneration.Specifications;

namespace ApplicationLayer.InstructionGeneration.Operations;

public sealed class SwitchLineVtToReserveOperation : IOperation
{
    private readonly ISpecification<InstructionContext> _needSwitchVtToReserve =
        new PredicateSpecification<InstructionContext>(ctx => ctx.NeedSwitchVTToReserve);

    public IEnumerable<InstructionItem> Build(InstructionContext ctx)
    {
        if (!_needSwitchVtToReserve.IsSatisfiedBy(ctx))
        {
            yield break;
        }

        yield return new InstructionItem(
            InstructionCodes.SwitchLineVtToReserve,
            "Перевод цепей напряжения линейного ТН",
            "Произвести перевод цепей напряжения устройств РЗ и СА линии на резервный ТН.",
            700);
    }
}
