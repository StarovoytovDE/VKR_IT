namespace Domain.Entities;

/// <summary>
/// Трансформатор напряжения (ТН / VT), привязанный к устройству.
/// </summary>
public sealed class Vt
{
    /// <summary>PK.</summary>
    public long VtId { get; set; }

    /// <summary>FK → Device.</summary>
    public long DeviceId { get; set; }

    /// <summary>Признак основного ТН.</summary>
    public bool Main { get; set; }

    /// <summary>Наименование ТН.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Место подключения (РУССКОЕ значение).
    /// </summary>
    public string Place { get; set; } = string.Empty;

    /// <summary>
    /// Код места подключения (для алгоритмов).
    /// VT_LINE / VT_BUS / VT_BUSBAR
    /// </summary>
    public string PlaceCode { get; set; } = string.Empty;

    /// <summary>Навигация → Device.</summary>
    public Device Device { get; set; } = null!;
}
