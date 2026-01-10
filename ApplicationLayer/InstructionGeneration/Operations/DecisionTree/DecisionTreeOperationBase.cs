using System;
using ApplicationLayer.InstructionGeneration.Criteria;

namespace ApplicationLayer.InstructionGeneration.Operations.DecisionTree;

/// <summary>
/// Базовый класс для операций, реализуемых как дерево решений.
/// </summary>
public abstract class DecisionTreeOperationBase : IOperation
{
    private readonly Lazy<Node<LineOperationCriteria>> _root;

    /// <summary>
    /// Инициализирует операцию и лениво строит дерево решений.
    /// </summary>
    protected DecisionTreeOperationBase()
    {
        _root = new Lazy<Node<LineOperationCriteria>>(BuildTree);
    }

    /// <inheritdoc />
    public abstract string Code { get; }

    /// <inheritdoc />
    public string? Evaluate(LineOperationCriteria criteria)
    {
        ArgumentNullException.ThrowIfNull(criteria);
        return _root.Value.Eval(criteria);
    }

    /// <summary>
    /// Строит дерево решений конкретной операции.
    /// </summary>
    protected abstract Node<LineOperationCriteria> BuildTree();
}
