using ApplicationLayer.InstructionGeneration.Models;

namespace ApplicationLayer.InstructionGeneration.Criteria;

/// <summary>
/// Provides line operation criteria from an in-memory collection.
/// </summary>
public sealed class InMemoryLineCriteriaProvider : ILineCriteriaProvider
{
    private readonly IReadOnlyList<LineOperationCriteria> _criteria;

    /// <summary>
    /// Initializes a new instance of the <see cref="InMemoryLineCriteriaProvider"/> class.
    /// </summary>
    /// <param name="criteria">The criteria to serve.</param>
    public InMemoryLineCriteriaProvider(IEnumerable<LineOperationCriteria> criteria)
    {
        _criteria = criteria.ToList();
    }

    /// <inheritdoc />
    public IEnumerable<LineOperationCriteria> GetCriteria(ActionCode actionCode)
    {
        return _criteria.Where(c => c.ActionCode == actionCode);
    }
}
