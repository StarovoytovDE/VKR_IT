namespace ApplicationLayer.InstructionGeneration.Models;

internal static class InstructionTexts
{
    internal const string DisableDfzFunction = "Вывести функцию ДФЗ";
    internal const string DisableDfzDevice = "Вывести устройство РЗ и СА";
    internal const string DisableDzlDevice = "Вывести устройство РЗ и СА (ДЗЛ)";
    internal const string DisableDzFunction = "Вывести функцию ДЗ";
    internal const string DisableDzDevice = "Вывести устройство РЗ и СА";
    internal const string DisableOapvFunction = "Вывести функцию ОАПВ";
    internal const string DisableTapvFunction = "Вывести функцию ТАПВ";
    internal const string DisableUpaskReceivers = "Вывести цепи приёмников УПАСК ВЧ-каналов ПА";
    internal const string SwitchLineVtToReserve =
        "Цепи напряжения устройств РЗ и СА, нормально подключенные к линейному ТН, перевести с линейного ТН на резервный";
    internal const string SwitchBusVtToReserve =
        "Цепи напряжения устройства, подключенного к ТН ошиновки, перевести на резервный шинный ТН";
    internal const string DisconnectLineCtFromDzo = "Токовые цепи линейного ТТ отключить от ДЗО данной ВЛ";
    internal const string MtzoShinovkaAToB =
        "После отключения выключателей линии необходимо ввести в работу МТЗ ошиновки – переключить уставки с группы «А» на группу «B»";
    internal const string FollowVoltageSwitchInstructions =
        "Сформировать указания по операциям с устройствами РЗ и СА при переводе цепей напряжения";
}
