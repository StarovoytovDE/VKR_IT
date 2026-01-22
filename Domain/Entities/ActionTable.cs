namespace Domain.Entities;

/// <summary>
/// Действие (сценарий) для формирования указаний.
/// </summary>
public sealed class ActionTable
{
    /// <summary>
    /// Идентификатор действия.
    /// </summary>
    public long ActionId { get; set; }

    /// <summary>
    /// Код действия.
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Наименование действия.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Описание действия.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Запросы формирования указаний, созданные по данному действию.
    /// </summary>
    public ICollection<InstructionRequest> InstructionRequests { get; set; } = [];
}
