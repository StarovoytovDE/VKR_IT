using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Models;
using ApplicationLayer.InstructionGeneration.Operations;

namespace ApplicationLayer.InstructionGeneration.Generation;

/// <summary>
/// Generates instruction items for a specific action code.
/// </summary>
public sealed class InstructionGenerator : IInstructionGenerator
{
    private readonly ActionCode _actionCode;
    private readonly IReadOnlyList<IOperation> _operations;

    /// <summary>
    /// Initializes a new instance of the <see cref="InstructionGenerator"/> class.
    /// </summary>
    /// <param name="actionCode">The action code to generate for.</param>
    public InstructionGenerator(ActionCode actionCode)
    {
        _actionCode = actionCode;
        _operations = BuildOperations(actionCode);
    }

    /// <summary>
    /// Gets the action code used by this generator.
    /// </summary>
    public ActionCode ActionCode => _actionCode;

    /// <inheritdoc />
    public InstructionGenerationResult Generate(IEnumerable<LineOperationCriteria> deviceCriteria)
    {
        var criteriaList = deviceCriteria
            .Where(c => c.ActionCode == _actionCode)
            .ToList();

        var items = new List<InstructionItem>();

        var grouped = criteriaList
            .GroupBy(c => new { c.Side, c.DeviceObjectId })
            .OrderBy(g => g.Key.Side)
            .ThenBy(g => g.Key.DeviceObjectId);

        foreach (var group in grouped)
        {
            var criteria = group.First();

            foreach (var operation in _operations)
            {
                var decision = operation.Evaluate(criteria);

                if (decision.AddItem && decision.Item is not null)
                {
                    items.Add(decision.Item);
                }
            }
        }

        if (items.Any(item => item.Code is InstructionCodes.SwitchLineVtToReserve or InstructionCodes.SwitchBusVtToReserve))
        {
            items = items
                .Where(item => item.Code != InstructionCodes.FollowVoltageSwitchInstructions)
                .ToList();
        }

        var orderedItems = items
            .OrderBy(item => item.Order)
            .ThenBy(item => item.Side)
            .ThenBy(item => item.DeviceId)
            .ToList();

        return new InstructionGenerationResult(orderedItems);
    }

    private static IReadOnlyList<IOperation> BuildOperations(ActionCode actionCode)
    {
        return actionCode switch
        {
            ActionCode.VlOutFieldClosing => new IOperation[]
            {
                new DfzFieldClosingOperation(),
                new DzlFieldClosingOperation(),
                new DzFieldClosingOperation(),
                new OapvFieldClosingOperation(),
                new TapvGenericOperation(),
                new UpaskFieldClosingOperation(),
                new SwitchLineVtToReserveOperation(),
                new SwitchBusVtToReserveOperation(),
                new DisconnectLineCtFromDzoOperation(),
                new MtzoShinovkaAtoBOperation(),
            },
            ActionCode.VlOutNoFieldClosing => new IOperation[]
            {
                new MtzoShinovkaAtoBOperation(),
                new DfzNoFieldClosingOperation(),
                new DzNoFieldClosingOperation(),
                new OapvNoFieldClosingOperation(),
                new TapvNoFieldClosingOperation(),
            },
            ActionCode.VlOutLineDisconnectorsOpenBusIsOn => new IOperation[]
            {
                new OapvFieldClosingOperation(),
                new TapvGenericOperation(),
                new SwitchBusVtToReserveOperation(),
                new DisconnectLineCtFromDzoOperation(),
            },
            ActionCode.VlOutOneSide => new IOperation[]
            {
                new DfzOneSideOperation(),
            },
            _ => Array.Empty<IOperation>(),
        };
    }
}
