namespace Domain.Entities;

/// <summary>
/// Место подключения трансформаторов тока (ТТ) устройства.
/// </summary>
public sealed class CtPlace
{
    /// <summary>PK.</summary>
    public long CtPlaceId { get; set; }

    /// <summary>FK → Device.</summary>
    public long DeviceId { get; set; }

    /// <summary>Наименование варианта (для UI).</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Место подключения (РУССКОЕ значение).
    /// Используется в UI и выводе указаний.
    /// </summary>
    public string Place { get; set; } = string.Empty;

    /// <summary>
    /// Код места подключения (для алгоритмов).
    /// CT_LINE_BEFORE_LR / CT_LINE_AFTER_LR / CT_SUM_BREAKERS
    /// </summary>
    public string PlaceCode { get; set; } = string.Empty;

    /// <summary>Навигация → Device.</summary>
    public Device Device { get; set; } = null!;
}
