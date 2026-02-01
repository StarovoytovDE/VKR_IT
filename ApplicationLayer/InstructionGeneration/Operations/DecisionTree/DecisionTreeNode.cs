using System;

namespace ApplicationLayer.InstructionGeneration.Operations.DecisionTree;

/// <summary>
/// Узел дерева решений, вычисляющий результат на основании критериев.
/// </summary>
/// <typeparam name="TCriteria">Тип критериев.</typeparam>
public abstract class Node<TCriteria>
{
    /// <summary>
    /// Вычисляет результат для заданных критериев.
    /// </summary>
    public abstract string? Eval(TCriteria criteria);

    /// <summary>
    /// Создаёт узел-условие (ромб на блок-схеме).
    /// </summary>
    public static Node<TCriteria> Decision(
        Func<TCriteria, bool> predicate,
        Node<TCriteria> whenTrue,
        Node<TCriteria> whenFalse)
        => new DecisionNode<TCriteria>(predicate, whenTrue, whenFalse);

    /// <summary>
    /// Создаёт узел-действие (лист дерева), возвращающий фиксированный результат.
    /// </summary>
    public static Node<TCriteria> Action(string? output)
        => new ActionNode<TCriteria>(output);

    /// <summary>
    /// Создаёт узел-действие (лист дерева), вычисляющий результат динамически по критериям.
    /// </summary>
    public static Node<TCriteria> Action(Func<TCriteria, string?> outputFactory)
        => new FuncActionNode<TCriteria>(outputFactory);

    /// <summary>
    /// Создаёт узел-действие «ничего не делать» (возвращает null).
    /// </summary>
    public static Node<TCriteria> NoAction()
        => new ActionNode<TCriteria>(null);
}

/// <summary>
/// Узел-условие дерева решений.
/// </summary>
public sealed class DecisionNode<TCriteria> : Node<TCriteria>
{
    private readonly Func<TCriteria, bool> _predicate;
    private readonly Node<TCriteria> _whenTrue;
    private readonly Node<TCriteria> _whenFalse;

    /// <summary>
    /// Создаёт узел-условие.
    /// </summary>
    public DecisionNode(
        Func<TCriteria, bool> predicate,
        Node<TCriteria> whenTrue,
        Node<TCriteria> whenFalse)
    {
        _predicate = predicate ?? throw new ArgumentNullException(nameof(predicate));
        _whenTrue = whenTrue ?? throw new ArgumentNullException(nameof(whenTrue));
        _whenFalse = whenFalse ?? throw new ArgumentNullException(nameof(whenFalse));
    }

    /// <inheritdoc />
    public override string? Eval(TCriteria criteria)
        => _predicate(criteria) ? _whenTrue.Eval(criteria) : _whenFalse.Eval(criteria);
}

/// <summary>
/// Узел-действие (лист дерева), возвращающий фиксированный результат.
/// </summary>
public sealed class ActionNode<TCriteria> : Node<TCriteria>
{
    private readonly string? _output;

    /// <summary>
    /// Создаёт узел-действие.
    /// </summary>
    public ActionNode(string? output)
    {
        _output = output;
    }

    /// <inheritdoc />
    public override string? Eval(TCriteria criteria) => _output;
}

/// <summary>
/// Узел-действие (лист дерева), вычисляющий результат по критериям.
/// </summary>
public sealed class FuncActionNode<TCriteria> : Node<TCriteria>
{
    private readonly Func<TCriteria, string?> _outputFactory;

    /// <summary>
    /// Создаёт узел-действие с фабрикой результата.
    /// </summary>
    public FuncActionNode(Func<TCriteria, string?> outputFactory)
    {
        _outputFactory = outputFactory ?? throw new ArgumentNullException(nameof(outputFactory));
    }

    /// <inheritdoc />
    public override string? Eval(TCriteria criteria) => _outputFactory(criteria);
}

