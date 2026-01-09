namespace ApplicationLayer.InstructionGeneration.Models;

/// <summary>
/// Defines the type of an instruction item.
/// </summary>
public enum InstructionItemType
{
    /// <summary>
    /// Represents an actionable instruction.
    /// </summary>
    Action,

    /// <summary>
    /// Represents a warning instruction.
    /// </summary>
    Warning,

    /// <summary>
    /// Represents an informational instruction.
    /// </summary>
    Info
}
