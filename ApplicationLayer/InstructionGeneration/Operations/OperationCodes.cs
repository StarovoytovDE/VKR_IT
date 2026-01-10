namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Единые коды операций.
/// Используются для конфигурации соответствия ActionCode → набор операций.
/// </summary>
public static class OperationCodes
{
    /// <summary>ДФЗ при выводе линии с замыканием поля.</summary>
    public const string DfzFieldClosing = "DFZ_FIELD_CLOSING";

    /// <summary>ДЗЛ при выводе линии с замыканием поля.</summary>
    public const string DzlFieldClosing = "DZL_FIELD_CLOSING";

    /// <summary>ДЗЛ при выводе линии без замыкания поля.</summary>
    public const string DzlNoFieldClosing = "DZL_NO_FIELD_CLOSING";

    // Дальше по мере реализации 14 операций добавишь:
    // public const string DzFieldClosing = "...";
    // public const string DzNoFieldClosing = "...";
    // и т.п.
}
