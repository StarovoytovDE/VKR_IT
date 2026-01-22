namespace Domain.Entities;

/// <summary>
/// Функция ДЗЛ, привязанная к устройству.
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
    /// Признак наличия (haz_dzl).
    /// </summary>
    public bool HazDzl { get; set; }

    /// <summary>
    /// Состояние (state).
    /// </summary>
    public bool State { get; set; }

    /// <summary>
    /// Навигация на устройство.
    /// </summary>
    public Device Device { get; set; } = null!;
}
