namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Шаблоны и фабрики текстов указаний.
/// </summary>
public static class InstructionTexts
{
    /// <summary>Указание: вывести устройство (когда функция единственная в устройстве).</summary>
    public const string WithdrawDevice = "Вывести устройство";

    /// <summary>
    /// Формирует указание: вывести конкретную функцию (например, ДФЗ/ДЗЛ).
    /// </summary>
    public static string WithdrawFunction(string functionName)
        => $"Вывести функцию {functionName}";

    /// <summary>
    /// Указание: следовать регламенту при переводе цепей напряжения.
    /// </summary>
    public static string FollowVoltageTransferInstructions(string functionName)
        => $"Следовать указаниям при переводе цепей напряжения устройства РЗ и СА для функции {functionName}";

    /// <summary>
    /// Указание: вывести приёмники УПАСК, по которым организована передача команд РЗ.
    /// </summary>
    public const string WithdrawUpaskReceiversForRzCommands =
        "Вывести приёмники УПАСК, по которым организована передача команд РЗ";

    /// <summary>
    /// Формирует указание перевода цепей напряжения с основного ТН на резервный ТН
    /// с подстановкой имени и места установки для обоих ТН.
    /// </summary>
    public static string BuildVtVoltageCircuitsTransfer(
        string mainVtName,
        string mainVtPlace,
        string reserveVtName,
        string reserveVtPlace)
    {
        // Требуемый формат (пример):
        // Произвести перевод цепей напряжения устройств РЗ и СА, нормально подключенных к Main1 (Линейный ТН),
        // с Main1 (Линейный ТН) на NotMain1 (Шинный ТН)
        return
            $"Произвести перевод цепей напряжения устройств РЗ и СА, нормально подключенных к {mainVtName} ({mainVtPlace}), " +
            $"с {mainVtName} ({mainVtPlace}) на {reserveVtName} ({reserveVtPlace})";
    }

    /// <summary>
    /// Указание: произвести отключение токовых цепей линейного ТТ от ДЗО данной ВЛ.
    /// </summary>
    public const string DisconnectLineCtFromDzo =
        "Произвести отключение токовых цепей линейного ТТ от ДЗО данной ВЛ";

    /// <summary>
    /// Указание: после отключения выключателей линии переключить уставки МТЗ ошиновки с группы «А» на группу «B».
    /// </summary>
    public const string MtzoShinovkaSwitchGroupAtoB =
        "После отключения выключателей линии, необходимо в РЗ с РС и (или) РЗ с БС ввести в работу МТЗ ошиновки – переключить уставки с группы «А» на группу «B».";
}
