namespace Domain.Entities;

/// <summary>
/// Возможные места подключения ТТ (CT) для устройства.
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
    /// Место подключения (строкой, как в диаграмме place).
    /// </summary>
    public string Place { get; set; } = string.Empty;

    /// <summary>
    /// Навигация на устройство.
    /// </summary>
    public Device Device { get; set; } = null!;
}
