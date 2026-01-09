namespace ApplicationLayer.InstructionGeneration.Models;

/// <summary>
/// Represents the result of a single operation evaluation.
/// </summary>
/// <param name="AddItem">Indicates whether an item should be added.</param>
/// <param name="Item">The generated item, if any.</param>
/// <param name="StopOperation">Indicates whether the operation should stop.</param>
/// <param name="IsVoltageSwitchInstruction">Indicates whether the item is a voltage switch instruction.</param>
public sealed record OperationDecision(
    bool AddItem,
    InstructionItem? Item,
    bool StopOperation = true,
    bool IsVoltageSwitchInstruction = false)
{
    /// <summary>
    /// Represents an empty decision with no item.
    /// </summary>
    public static OperationDecision None => new(false, null, true, false);
}
