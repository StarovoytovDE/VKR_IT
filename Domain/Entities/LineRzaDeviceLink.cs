namespace Domain.Entities;

/// <summary>
/// Связь между линией и устройством РЗА с указанием стороны линии.
/// </summary>
public sealed class LineRzaDeviceLink
{
    /// <summary>
    /// Идентификатор объекта линии.
    /// </summary>
    public long LineObjectId { get; set; }

    /// <summary>
    /// Идентификатор объекта устройства РЗА.
    /// </summary>
    public long DeviceObjectId { get; set; }

    /// <summary>
    /// Сторона линии, к которой относится устройство.
    /// </summary>
    public LineSide LineSide { get; set; }

    /// <summary>
    /// Навигация на объект, выступающий линией.
    /// </summary>
    public ObjectTable LineObject { get; set; } = null!;

    /// <summary>
    /// Навигация на объект, выступающий устройством РЗА.
    /// </summary>
    public ObjectTable DeviceObject { get; set; } = null!;
}
