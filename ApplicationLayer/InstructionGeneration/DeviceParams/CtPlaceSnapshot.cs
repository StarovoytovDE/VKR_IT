namespace ApplicationLayer.InstructionGeneration.DeviceParams;

/// <summary>
/// Снимок места подключения ТТ (CT) для устройства.
/// </summary>
public sealed class CtPlaceSnapshot
{
    /// <summary>Наименование варианта (для UI/логов).</summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>Русское значение места подключения (для UI/выводов).</summary>
    public string? Place { get; init; }

    /// <summary>Код места подключения (для алгоритмов).</summary>
    public string? PlaceCode { get; init; }
}
