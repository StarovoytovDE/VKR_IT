using ApplicationLayer.InstructionGeneration.Context;
using ApplicationLayer.InstructionGeneration.Models;
using ApplicationLayer.InstructionGeneration.Specifications;

namespace ApplicationLayer.InstructionGeneration.Operations;

public sealed class UpaskOperation : IOperation
{
    private readonly ISpecification<InstructionContext> _needDisableUpask =
        new PredicateSpecification<InstructionContext>(ctx => ctx.NeedDisableUPASKReceivers);

    public IEnumerable<InstructionItem> Build(InstructionContext ctx)
    {
        if (!_needDisableUpask.IsSatisfiedBy(ctx))
        {
            yield break;
        }

        yield return new InstructionItem(
            InstructionCodes.UpaskReceiversDisable,
            "УПАСК",
            "Вывести приёмники УПАСК в устройствах РЗА линии?",
            600);
    }
}
