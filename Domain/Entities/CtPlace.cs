namespace Domain.Entities;

/// <summary>
/// Место подключения ТТ (CT) для устройства.
/// </summary>
public sealed class CtPlace
{
    /// <summary>
    /// Идентификатор записи места подключения CT.
    /// </summary>
    public long CtPlaceId { get; set; }

    /// <summary>
    /// Идентификатор устройства.
    /// </summary>
    public long DeviceId { get; set; }

    /// <summary>
    /// Наименование варианта.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Место подключения (русское значение для UI/выводов).
    /// Например: "Линейный ТТ до ЛР", "Линейный ТТ после ЛР", "Сумма токов выключателей линии".
    /// </summary>
    public string Place { get; set; } = string.Empty;

    /// <summary>
    /// Код места подключения (для алгоритмов).
    /// Например: CT_LINE_BEFORE_LR / CT_LINE_AFTER_LR / CT_SUM_BREAKERS.
    /// </summary>
    public string PlaceCode { get; set; } = string.Empty;

    /// <summary>
    /// Навигация на устройство.
    /// </summary>
    public Device Device { get; set; } = null!;
}
