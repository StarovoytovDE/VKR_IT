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
    /// Место подключения (строкой, как в диаграмме place).
    /// </summary>
    public string Place { get; set; } = string.Empty;

    /// <summary>
    /// Навигация на устройство.
    /// </summary>
    public Device Device { get; set; } = null!;
}
