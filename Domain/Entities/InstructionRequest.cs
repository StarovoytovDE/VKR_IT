namespace Domain.Entities;

/// <summary>
/// Запрос на формирование указаний.
/// </summary>
public sealed class InstructionRequest
{
    /// <summary>
    /// Идентификатор запроса.
    /// </summary>
    public long InstructionRequestId { get; set; }

    /// <summary>
    /// Идентификатор объекта.
    /// </summary>
    public long ObjectId { get; set; }

    /// <summary>
    /// Идентификатор действия.
    /// </summary>
    public long ActionId { get; set; }

    /// <summary>
    /// Идентификатор пользователя, создавшего запрос.
    /// </summary>
    public long CreatedByUserId { get; set; }

    /// <summary>
    /// Статус запроса.
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Комментарий.
    /// </summary>
    public string? Comment { get; set; }

    /// <summary>
    /// Дата и время создания.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Навигация на объект.
    /// </summary>
    public ObjectTable Object { get; set; } = null!;

    /// <summary>
    /// Навигация на действие.
    /// </summary>
    public ActionTable Action { get; set; } = null!;

    /// <summary>
    /// Навигация на пользователя-создателя.
    /// </summary>
    public AppUser CreatedByUser { get; set; } = null!;

    /// <summary>
    /// Значения/привязки параметров запроса (таблица request_param_value).
    /// </summary>
    public ICollection<RequestParamValue> RequestParamValues { get; set; } = [];

    /// <summary>
    /// Результат формирования указаний.
    /// </summary>
    public InstructionResult? InstructionResult { get; set; }
}
