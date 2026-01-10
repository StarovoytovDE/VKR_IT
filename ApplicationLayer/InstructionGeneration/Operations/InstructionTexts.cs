namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Единые тексты формируемых указаний.
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
    public const string FollowVoltageTransferInstructions =
        "Следовать указаниям при переводе цепей напряжения устройства РЗ и СА";

    /// <summary>
    /// Указание: вывести приёмники УПАСК, по которым организована передача команд РЗ.
    /// </summary>
    public const string WithdrawUpaskReceiversForRzCommands =
        "Вывести приёмники УПАСК, по которым организована передача команд РЗ";

    /// <summary>
    /// Указание: произвести перевод цепей напряжения устройств РЗ и СА,
    /// нормально подключенных к линейному ТН, с линейного ТН на резервный.
    /// </summary>
    public const string TransferVoltageCircuitsFromLineVtToReserve =
        "Произвести перевод цепей напряжения устройств РЗ и СА, нормально " +
        "подключенных к линейному ТН, с линейного ТН на резервный";

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

    /// <summary>
    /// Указание: произвести перевод цепей напряжения устройств РЗ и СА, нормально подключенных к ТН ошиновки,
    /// с ТН ошиновки на резервный шинный ТН.
    /// </summary>
    public const string TransferVoltageCircuitsFromBusbarVtToReserveBusVt =
        "Произвести перевод цепей напряжения устройств РЗ и СА, нормально подключенных к ТН ошиновки, с ТН ошиновки на резервный шинный ТН";
}
