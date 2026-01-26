namespace Domain.Entities;

/// <summary>
/// МТЗ ошиновки (mtz_busbar), привязанная к устройству.
/// </summary>
public sealed class MtzBusbar
{
    /// <summary>
    /// Идентификатор МТЗ ошиновки.
    /// </summary>
    public long MtzBusbarId { get; set; }

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
