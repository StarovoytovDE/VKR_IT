using System;
using System.Collections.Generic;

namespace Domain.Entities;

public sealed class ActionVersion
{
    public long ActionVersionId { get; set; }
    public long ActionId { get; set; }
    public string VersionLabel { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTimeOffset ReleasedAt { get; set; }

    public Action Action { get; set; } = null!;
    public ICollection<InstructionRequest> InstructionRequests { get; set; } = new List<InstructionRequest>();
    public ICollection<ActionParamRequirement> ActionParamRequirements { get; set; } = new List<ActionParamRequirement>();
}
