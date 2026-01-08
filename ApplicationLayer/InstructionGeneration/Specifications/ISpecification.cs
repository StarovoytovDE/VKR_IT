namespace ApplicationLayer.InstructionGeneration.Specifications;

public interface ISpecification<in T>
{
    bool IsSatisfiedBy(T candidate);
}
