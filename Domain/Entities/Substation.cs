namespace Domain.Entities;

/// <summary>
/// Подстанция (справочник), используемая для привязки объектов.
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
    /// Объекты, относящиеся к подстанции.
    /// </summary>
    public ICollection<ObjectTable> Objects { get; set; } = [];
}
