using ApplicationLayer.InstructionGeneration.Context;
using ApplicationLayer.InstructionGeneration.Models;

namespace ApplicationLayer.InstructionGeneration.Operations;

public interface IOperation
{
    IEnumerable<InstructionItem> Build(InstructionContext ctx);
}
