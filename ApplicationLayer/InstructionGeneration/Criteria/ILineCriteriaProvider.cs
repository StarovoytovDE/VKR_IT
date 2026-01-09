using ApplicationLayer.InstructionGeneration.Models;

namespace ApplicationLayer.InstructionGeneration.Criteria;

/// <summary>
/// Provides line operation criteria from a data source.
/// </summary>
public interface ILineCriteriaProvider
{
    /// <summary>
    /// Gets criteria for the specified action code.
    /// </summary>
    /// <param name="actionCode">The action code.</param>
    /// <returns>A collection of line operation criteria.</returns>
    IEnumerable<LineOperationCriteria> GetCriteria(ActionCode actionCode);
}
