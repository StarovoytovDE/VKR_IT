using System;
using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.Operations.DecisionTree;
using Domain.ReferenceData;

namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Операция: вывод ДЗ для действия "Вывод ВЛ с замыканием поля".
/// </summary>
public sealed class DzFieldClosingOperation : DecisionTreeOperationBase
{
    /// <inheritdoc />
    public override string Code => OperationCodes.DzFieldClosing;

    /// <inheritdoc />
    protected override Node<LineOperationCriteria> BuildTree()
    {
        // Логика переведена на общие параметры устройства:
        // - считаем, что ДЗ "завязан" на линейный ТН, если основной ТН устройства имеет place_code = VT_LINE
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
    /// Проверяет, что основной ТН устройства является линейным (place_code = VT_LINE).
    /// </summary>
    private static bool IsMainVtLine(LineOperationCriteria c)
    {
        ArgumentNullException.ThrowIfNull(c);
        return string.Equals(c.MainVtPlaceCode, PlaceCodes.Vt.Line, StringComparison.Ordinal);
    }

    /// <summary>
    /// Определяет, требуется ли перевод цепей напряжения устройства на резервный ТН
    /// по общим параметрам устройства (работаем по place_code, а не по русским строкам).
    /// </summary>
    private static bool IsAnyVtSwitchRequired(LineOperationCriteria c)
    {
        ArgumentNullException.ThrowIfNull(c);

        return c.VtSwitchTrue &&
               !string.IsNullOrWhiteSpace(c.MainVtPlaceCode) &&
               !string.IsNullOrWhiteSpace(c.ReserveVtPlaceCode) &&
               !string.Equals(c.MainVtPlaceCode, c.ReserveVtPlaceCode, StringComparison.Ordinal);
    }
}
