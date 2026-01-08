using ApplicationLayer.InstructionGeneration.Context;
using ApplicationLayer.InstructionGeneration.Models;
using ApplicationLayer.InstructionGeneration.Specifications;

namespace ApplicationLayer.InstructionGeneration.Operations;

public sealed class SwitchBusVtToReserveOperation : IOperation
{
    private readonly ISpecification<InstructionContext> _needSwitchBusVtToReserve =
        new PredicateSpecification<InstructionContext>(ctx => ctx.NeedSwitchBusVTToReserve);

    public IEnumerable<InstructionItem> Build(InstructionContext ctx)
    {
        if (!_needSwitchBusVtToReserve.IsSatisfiedBy(ctx))
        {
            yield break;
        }

        yield return new InstructionItem(
            InstructionCodes.SwitchBusVtToReserve,
            "Перевод шинного ТН",
            "Произвести перевод цепей напряжения шинного ТН на резервный.",
            1000);
    }
}
