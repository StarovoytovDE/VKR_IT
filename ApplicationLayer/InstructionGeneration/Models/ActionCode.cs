namespace ApplicationLayer.InstructionGeneration.Models;

/// <summary>
/// Код действия (сценария), определяющий набор операций.
/// </summary>
public enum ActionCode
{
    /// <summary>Вывод линии с замыканием поля.</summary>
    LineWithdrawalWithFieldClosing = 1,

    /// <summary>Вывод линии без замыкания поля.</summary>
    LineWithdrawalWithoutFieldClosing = 2,

    /// <summary>
    /// Создаётся схема, когда отключены линейные разъединители выключателей
    /// ВЛ и остаётся включен хотя бы один из разъединителей выключателей ВЛ,
    /// примыкающий к системе шин
    /// </summary>
    LineWithdrawalWithBusSideDisconnector = 3,

    /// <summary>Вывод ВЛ с одной стороны.</summary>
    LineSingleSideWithdrawal = 4
}
