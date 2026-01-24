namespace ApplicationLayer.InstructionGeneration.DeviceParams;

/// <summary>
/// Снимок ТН (VT) — одного элемента пары (основной/резервный).
/// </summary>
public sealed class VtSnapshot
{
    /// <summary>Наименование ТН (для UI/логов).</summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>Русское место подключения (для UI/выводов).</summary>
    public string? Place { get; init; }

    /// <summary>Код места подключения (для алгоритмов).</summary>
    public string? PlaceCode { get; init; }
}
