namespace Domain.Entities;

/// <summary>
/// Функция ТАПВ, привязанная к устройству.
/// </summary>
public sealed class Tapv
{
    /// <summary>
    /// Идентификатор ТАПВ.
    /// </summary>
    public long TapvId { get; set; }

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
    /// Признак "выключено" (switch_off).
    /// </summary>
    public bool SwitchOff { get; set; }

    /// <summary>
    /// Состояние (state).
    /// </summary>
    public bool State { get; set; }

    /// <summary>
    /// Навигация на устройство.
    /// </summary>
    public Device Device { get; set; } = null!;
}
