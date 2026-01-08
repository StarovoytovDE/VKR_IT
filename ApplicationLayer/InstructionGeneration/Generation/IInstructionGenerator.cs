using ApplicationLayer.InstructionGeneration.Context;
using ApplicationLayer.InstructionGeneration.Models;

namespace ApplicationLayer.InstructionGeneration.Generation;

public interface IInstructionGenerator
{
    InstructionResultDto Generate(InstructionContext ctx);
}
