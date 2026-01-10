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
    /// Вывод линии при отключённых линейных разъединителях
    /// и включённом примыкающем к шинам.
    /// </summary>
    LineWithdrawalWithBusSideDisconnector = 3,

    /// <summary>Вывод ВЛ с одной стороны.</summary>
    LineSingleSideWithdrawal = 4
}
