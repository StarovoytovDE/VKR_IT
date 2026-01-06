using System;
using System.Collections.Generic;

namespace Domain.Entities;

public sealed class InstructionRequest
{
    public long InstructionRequestId { get; set; }
    public long ObjectId { get; set; }
    public long ActionId { get; set; }
    public long CreatedByUserId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Comment { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public ObjectTable Object { get; set; } = null!;
    public ActionTable Action { get; set; } = null!;
    public AppUser CreatedByUser { get; set; } = null!;
    public ICollection<RequestParamValue> RequestParamValues { get; set; } = [];
    public InstructionResult? InstructionResult { get; set; }
}
