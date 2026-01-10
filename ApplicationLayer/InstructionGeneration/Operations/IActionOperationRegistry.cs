using ApplicationLayer.InstructionGeneration.Models;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Реестр соответствия: ActionCode -> набор операций, которые нужно выполнить.
/// </summary>
public interface IActionOperationRegistry
{
    IReadOnlyList<IOperation> GetOperations(ActionCode actionCode);
}
