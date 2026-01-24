using System;
using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Operations.DecisionTree;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Операция: вывод ДЗ для действия "Вывод ВЛ с замыканием поля".
/// </summary>
public sealed class DzFieldClosingOperation : DecisionTreeOperationBase
{
    private const string LineVtPlace = "Линейный ТН";

    /// <inheritdoc />
    public override string Code => OperationCodes.DzFieldClosing;

    /// <inheritdoc />
    protected override Node<LineOperationCriteria> BuildTree()
    {
        // Логика переведена на общие параметры устройства:
        // - считаем, что ДЗ "завязан" на линейный ТН, если основной ТН устройства = "Линейный ТН"
        // - если при этом требуется перевод цепей напряжения на резерв, то выдаём указание "следовать регламенту"
        // - иначе выводим функцию ДЗ
        var enabledBranch =
            Node<LineOperationCriteria>.Decision(
                predicate: IsMainVtLine,
                whenTrue: Node<LineOperationCriteria>.Decision(
                    predicate: IsAnyVtSwitchRequired,
                    whenTrue: Node<LineOperationCriteria>.Action(InstructionTexts.FollowVoltageTransferInstructions),
                    whenFalse: DzNodes.WithdrawFunction()
                ),
                whenFalse: Node<LineOperationCriteria>.Action(null)
            );

        return DzNodes.HasAndEnabled(enabledBranch);
    }

    /// <summary>
    /// Проверяет, что основной ТН устройства является линейным ("Линейный ТН").
    /// </summary>
    private static bool IsMainVtLine(LineOperationCriteria c)
    {
        ArgumentNullException.ThrowIfNull(c);
        return string.Equals(c.MainVtPlace, LineVtPlace, StringComparison.Ordinal);
    }

    /// <summary>
    /// Определяет, требуется ли вообще перевод цепей напряжения устройства на резервный ТН
    /// по общим параметрам устройства.
    /// </summary>
    private static bool IsAnyVtSwitchRequired(LineOperationCriteria c)
    {
        ArgumentNullException.ThrowIfNull(c);

        return c.VtSwitchTrue &&
               !string.IsNullOrWhiteSpace(c.MainVtPlace) &&
               !string.IsNullOrWhiteSpace(c.ReserveVtPlace) &&
               !string.Equals(c.MainVtPlace, c.ReserveVtPlace, StringComparison.Ordinal);
    }
}
