namespace Domain.Entities;

/// <summary>
/// Трансформатор напряжения (ТН / VT), привязанный к устройству.
/// </summary>
public sealed class Vt
{
    /// <summary>
    /// Идентификатор VT.
    /// </summary>
    public long VtId { get; set; }

    /// <summary>
    /// Идентификатор устройства.
    /// </summary>
    public long DeviceId { get; set; }

    /// <summary>
    /// Признак основного VT.
    /// </summary>
    public bool Main { get; set; }

    /// <summary>
    /// Наименование VT.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Место подключения (русское значение для UI/выводов).
    /// Например: "Линейный ТН", "Шинный ТН", "ТН ошиновки".
    /// </summary>
    public string Place { get; set; } = string.Empty;

    /// <summary>
    /// Код места подключения (для алгоритмов).
    /// Например: VT_LINE / VT_BUS / VT_BUSBAR.
    /// </summary>
    public string PlaceCode { get; set; } = string.Empty;

    /// <summary>
    /// Навигация на устройство.
    /// </summary>
    public Device Device { get; set; } = null!;
}
