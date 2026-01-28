namespace Domain.Entities;

using System.Collections.Generic;

/// <summary>
/// Подстанция (справочник).
/// </summary>
public sealed class Substation
{
    /// <summary>
    /// Идентификатор подстанции.
    /// </summary>
    public long SubstationId { get; set; }

    /// <summary>
    /// Диспетчерское наименование подстанции.
    /// </summary>
    public string DispatchName { get; set; } = string.Empty;

    /// <summary>
    /// Концы линий, относящиеся к подстанции.
    /// </summary>
    public ICollection<LineEnd> LineEnds { get; set; } = [];
}
