namespace Domain.Entities;

public sealed class UserRole
{
    public long UserId { get; set; }
    public long RoleId { get; set; }

    public AppUser AppUser { get; set; } = null!;
    public AppRole AppRole { get; set; } = null!;
}
