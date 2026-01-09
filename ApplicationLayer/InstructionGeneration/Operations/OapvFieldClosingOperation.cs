using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Models;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Decision tree for OAPV when field closing is applied.
/// </summary>
public sealed class OapvFieldClosingOperation : IOperation
{
    /// <inheritdoc />
    public OperationDecision Evaluate(LineOperationCriteria criteria)
    {
        if (!criteria.HasOAPV)
        {
            return OperationDecision.None;
        }

        if (!criteria.OAPVEnabled)
        {
            return OperationDecision.None;
        }

        var item = InstructionItemFactory.Create(
            criteria,
            InstructionCodes.OapvDisableFunction,
            InstructionTexts.DisableOapvFunction,
            InstructionOrder.Oapv);

        return new OperationDecision(true, item);
    }
}
