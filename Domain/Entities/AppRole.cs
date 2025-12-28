using System.Collections.Generic;

namespace Domain.Entities;

public sealed class AppRole
{
    public long RoleId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
