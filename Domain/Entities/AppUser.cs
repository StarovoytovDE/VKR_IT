namespace Domain.Entities;

/// <summary>
/// Пользователь приложения.
/// </summary>
public sealed class AppUser
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// Отображаемое имя.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Логин.
    /// </summary>
    public string Login { get; set; } = string.Empty;

    /// <summary>
    /// Хэш пароля.
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// Роли пользователя.
    /// </summary>
    public ICollection<UserRole> UserRoles { get; set; } = [];

    /// <summary>
    /// Запросы, созданные пользователем.
    /// </summary>
    public ICollection<InstructionRequest> InstructionRequestsCreated { get; set; } = [];
}
