namespace ApplicationLayer.InstructionGeneration.Models;

/// <summary>
/// Contains the generated instruction items.
/// </summary>
/// <param name="Items">The ordered list of items.</param>
public sealed record InstructionGenerationResult(IReadOnlyList<InstructionItem> Items);
