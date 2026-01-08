namespace ApplicationLayer.InstructionGeneration.Specifications;

public sealed class PredicateSpecification<T> : ISpecification<T>
{
    private readonly Func<T, bool> _predicate;

    public PredicateSpecification(Func<T, bool> predicate)
    {
        _predicate = predicate;
    }

    public bool IsSatisfiedBy(T candidate) => _predicate(candidate);
}
