namespace Domain.Entities;

using System.Collections.Generic;

/// <summary>
/// Конец линии (привязка линии к конкретной подстанции).
/// Нужен, чтобы у одной линии было два конца (A и B), и устройства привязывались к конкретному концу.
/// </summary>
public sealed class LineEnd
{
    /// <summary>
    /// Идентификатор конца линии.
    /// </summary>
    public long LineEndId { get; set; }

    /// <summary>
    /// Идентификатор линии (object).
    /// </summary>
    public long ObjectId { get; set; }

    /// <summary>
    /// Идентификатор подстанции, на которой расположен данный конец линии.
    /// </summary>
    public long SubstationId { get; set; }

    /// <summary>
    /// Обозначение конца линии: "A" или "B".
    /// </summary>
    public string SideCode { get; set; } = "A";

    /// <summary>
    /// Навигация на линию (object).
    /// </summary>
    public ObjectTable Object { get; set; } = null!;

    /// <summary>
    /// Навигация на подстанцию.
    /// </summary>
    public Substation Substation { get; set; } = null!;

    /// <summary>
    /// Устройства, установленные на данном конце линии.
    /// </summary>
    public ICollection<Device> Devices { get; set; } = [];
}
