namespace Domain.Entities;

/// <summary>
/// Универсальная таблица объектов (линии и т.п.), как в диаграмме object.
/// </summary>
public sealed class ObjectTable
{
    /// <summary>
    /// Идентификатор объекта.
    /// </summary>
    public long ObjectId { get; set; }

    /// <summary>
    /// Идентификатор типа объекта.
    /// </summary>
    public long ObjectTypeId { get; set; }

    /// <summary>
    /// Уникальный UID объекта.
    /// </summary>
    public string Uid { get; set; } = string.Empty;

    /// <summary>
    /// Диспетчерское наименование объекта.
    /// </summary>
    public string DispatchName { get; set; } = string.Empty;

    /// <summary>
    /// Признак активности.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Идентификатор подстанции.
    /// </summary>
    public long SubstationId { get; set; }

    /// <summary>
    /// Навигация на тип объекта.
    /// </summary>
    public ObjectType ObjectType { get; set; } = null!;

    /// <summary>
    /// Навигация на подстанцию.
    /// </summary>
    public Substation Substation { get; set; } = null!;

    /// <summary>
    /// Устройства, относящиеся к объекту.
    /// </summary>
    public ICollection<Device> Devices { get; set; } = [];

    /// <summary>
    /// Запросы формирования указаний по объекту.
    /// </summary>
    public ICollection<InstructionRequest> InstructionRequests { get; set; } = [];
}
