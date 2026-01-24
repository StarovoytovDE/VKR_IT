namespace ApplicationLayer.InstructionGeneration.Requests;

/// <summary>
/// Оперативные состояния функций РЗ и СА,
/// задаваемые диспетчером для текущей ситуации.
/// </summary>
public sealed class FunctionStatesRequest
{
    /// <summary>Введена ли функция ДФЗ.</summary>
    public bool DfzEnabled { get; init; }

    /// <summary>Введена ли функция ДЗЛ.</summary>
    public bool DzlEnabled { get; init; }

    /// <summary>Введена ли функция ДЗ.</summary>
    public bool DzEnabled { get; init; }

    /// <summary>Введена ли функция ОАПВ.</summary>
    public bool OapvEnabled { get; init; }

    /// <summary>Введена ли функция ТАПВ.</summary>
    public bool TapvEnabled { get; init; }
}
