namespace Domain.Entities;

/// <summary>
/// Функция ДЗЛ, привязанная к устройству.
/// Признак наличия функции в логике определяется фактом наличия записи ДЗЛ для устройства.
/// </summary>
public sealed class Dzl
{
    /// <summary>
    /// Идентификатор ДЗЛ.
    /// </summary>
    public long DzlId { get; set; }

    /// <summary>
    /// Идентификатор устройства.
    /// </summary>
    public long DeviceId { get; set; }

    /// <summary>
    /// Код функции.
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Наименование функции.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Состояние (state).
    /// </summary>
    public bool State { get; set; }

    /// <summary>
    /// Навигация на устройство.
    /// </summary>
    public Device Device { get; set; } = null!;
}
