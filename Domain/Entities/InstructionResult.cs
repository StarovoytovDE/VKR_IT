using System;

namespace Domain.Entities;

public sealed class InstructionResult
{
    public long InstructionRequestId { get; set; }
    public string GeneratedText { get; set; } = string.Empty;
    public DateTimeOffset GeneratedAt { get; set; }

    public InstructionRequest InstructionRequest { get; set; } = null!;
}
