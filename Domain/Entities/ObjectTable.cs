namespace Domain.Entities;

using System.Collections.Generic;

/// <summary>
/// Универсальная таблица объектов (в текущей модели — линия), как в диаграмме object.
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
    /// Навигация на тип объекта.
    /// </summary>
    public ObjectType ObjectType { get; set; } = null!;

    /// <summary>
    /// Концы линии (A и B) — привязки к подстанциям.
    /// </summary>
    public ICollection<LineEnd> LineEnds { get; set; } = [];

    /// <summary>
    /// Запросы формирования указаний по объекту (линии).
    /// </summary>
    public ICollection<InstructionRequest> InstructionRequests { get; set; } = [];
}
