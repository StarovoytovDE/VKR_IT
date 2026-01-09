using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Models;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Represents a decision tree operation.
/// </summary>
public interface IOperation
{
    /// <summary>
    /// Evaluates the operation against the given criteria.
    /// </summary>
    /// <param name="criteria">The operation criteria.</param>
    /// <returns>The decision for the operation.</returns>
    OperationDecision Evaluate(LineOperationCriteria criteria);
}
