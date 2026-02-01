namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Коды операций генерации указаний.
/// </summary>
public static class OperationCodes
{
    /// <summary>ДФЗ при выводе линии с замыканием поля.</summary>
    public const string DfzFieldClosing = "DFZ_FIELD_CLOSING";

    /// <summary>ДЗЛ при выводе линии с замыканием поля.</summary>
    public const string DzlFieldClosing = "DZL_FIELD_CLOSING";

    /// <summary>ДЗ при выводе линии с замыканием поля.</summary>
    public const string DzFieldClosing = "DZ_FIELD_CLOSING";

    /// <summary>ОАПВ (общая операция для нескольких действий).</summary>
    public const string Oapv = "OAPV";

    /// <summary>ТАПВ (общая операция для нескольких действий).</summary>
    public const string Tapv = "TAPV";

    /// <summary>Вывод приёмников УПАСК для передачи команд РЗ (замыкание поля).</summary>
    public const string UpaskReceiversWithdrawal = "UPASK_RECEIVERS_WITHDRAWAL";

    /// <summary>
    /// Универсальный перевод цепей напряжения с основного ТН на резервный ТН (по параметрам Main/Reserve VT).
    /// </summary>
    public const string VtVoltageCircuitsTransfer =
        "VT_VOLTAGE_CIRCUITS_TRANSFER";

    /// <summary>Отключение токовых цепей линейного ТТ от ДЗО данной ВЛ.</summary>
    public const string DisconnectLineCtFromDzo = "DISCONNECT_LINE_CT_FROM_DZO";

    /// <summary>МТЗ ошиновки: переключение уставок с группы «А» на группу «B».</summary>
    public const string MtzoShinovkaAtoB = "MTZO_SHINOVKA_A_TO_B";

    /// <summary>ДФЗ при выводе линии без замыкания поля.</summary>
    public const string DfzNoFieldClosing = "DFZ_NO_FIELD_CLOSING";

    /// <summary>ДЗ при выводе линии без замыкания поля.</summary>
    public const string DzNoFieldClosing = "DZ_NO_FIELD_CLOSING";

    /// <summary>ОАПВ при выводе линии без замыкания поля.</summary>
    public const string OapvNoFieldClosing = "OAPV_NO_FIELD_CLOSING";

    /// <summary>ТАПВ при выводе линии без замыкания поля.</summary>
    public const string TapvNoFieldClosing = "TAPV_NO_FIELD_CLOSING";

    /// <summary>
    /// Вывод ДФЗ при выводе ВЛ с одной стороны.
    /// </summary>
    public const string DfzSingleSideWithdrawal = "DFZ_SINGLE_SIDE_WITHDRAWAL";
}
