namespace ApplicationLayer.InstructionGeneration.Models;

/// <summary>
/// Represents a single generated instruction item.
/// </summary>
/// <param name="Code">The instruction code.</param>
/// <param name="Title">The short title for display.</param>
/// <param name="Text">The full instruction text.</param>
/// <param name="Order">The ordering index for sorting.</param>
/// <param name="Type">The item type.</param>
/// <param name="DeviceId">Optional device identifier.</param>
/// <param name="Side">Optional side of the line.</param>
public sealed record InstructionItem(
    string Code,
    string Title,
    string Text,
    int Order,
    InstructionItemType Type,
    int? DeviceId,
    SideOfLine? Side);
