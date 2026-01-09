using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Models;

namespace ApplicationLayer.InstructionGeneration.Generation;

/// <summary>
/// Generates instruction items for a set of device criteria.
/// </summary>
public interface IInstructionGenerator
{
    /// <summary>
    /// Generates instruction items for the provided criteria.
    /// </summary>
    /// <param name="deviceCriteria">The device criteria to evaluate.</param>
    /// <returns>The generation result.</returns>
    InstructionGenerationResult Generate(IEnumerable<LineOperationCriteria> deviceCriteria);
}
