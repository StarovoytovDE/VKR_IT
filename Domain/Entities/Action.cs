using System.Collections.Generic;

namespace Domain.Entities;

public sealed class Action
{
    public long ActionId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public ICollection<ActionVersion> ActionVersions { get; set; } = new List<ActionVersion>();
}
