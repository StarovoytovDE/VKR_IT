using System.Collections.Generic;

namespace Domain.Entities;

public sealed class AppUser
{
    public long UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Login { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<InstructionRequest> InstructionRequestsCreated { get; set; } = new List<InstructionRequest>();
    public ICollection<ObjectParamValue> ObjectParamValuesUpdated { get; set; } = new List<ObjectParamValue>();
}
