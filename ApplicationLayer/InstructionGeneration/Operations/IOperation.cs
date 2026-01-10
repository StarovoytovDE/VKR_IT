using ApplicationLayer.InstructionGeneration.Criteria;

namespace ApplicationLayer.InstructionGeneration.Operations;

public interface IOperation
{
    /// <summary>
    /// Уникальный код операции (для трассировки/журнала).
    /// </summary>
    string Code { get; }

    /// <summary>
    /// Возвращает текст указания или null, если операция не требуется.
    /// </summary>
    string? Evaluate(LineOperationCriteria criteria);
}
