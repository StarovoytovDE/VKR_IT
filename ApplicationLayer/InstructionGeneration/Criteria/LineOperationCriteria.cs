using ApplicationLayer.InstructionGeneration.Models;

namespace ApplicationLayer.InstructionGeneration.Criteria;

/// <summary>
/// Контекст (критерии) выполнения действия над линией
/// для конкретного устройства РЗ и СА и стороны линии.
/// </summary>
public sealed record LineOperationCriteria
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="LineOperationCriteria"/>.
    /// </summary>
    public LineOperationCriteria(string lineCode, SideOfLine side, int deviceObjectId, ActionCode actionCode)
    {
        LineCode = lineCode;
        Side = side;
        DeviceObjectId = deviceObjectId;
        ActionCode = actionCode;
    }

    /// <summary>Идентификатор линии.</summary>
    public string LineCode { get; init; }

    /// <summary>Сторона линии.</summary>
    public SideOfLine Side { get; init; }

    /// <summary>Идентификатор объекта устройства.</summary>
    public int DeviceObjectId { get; init; }

    /// <summary>Необязательное имя устройства (UI/логи).</summary>
    public string? DeviceName { get; init; }

    /// <summary>Код действия.</summary>
    public ActionCode ActionCode { get; init; }

    // =========================
    // CT / VT (общие параметры устройства)
    // =========================

    /// <summary>Русское место подключения ТТ (для UI/логов).</summary>
    public string CtPlace { get; init; } = string.Empty;

    /// <summary>Код места подключения ТТ (для алгоритмов).</summary>
    public string CtPlaceCode { get; init; } = string.Empty;

    /// <summary>
    /// Подключено ли устройство к линейному ТТ.
    /// Вычисляется по <see cref="CtPlaceCode"/>.
    /// </summary>
    public bool DeviceConnectedToLineCT { get; init; }

    /// <summary>Признак истинности логики перевода цепей напряжения на резерв.</summary>
    public bool VtSwitchTrue { get; init; }

    /// <summary>Русское место подключения основного ТН (для UI/логов).</summary>
    public string MainVtPlace { get; init; } = string.Empty;

    /// <summary>Код места подключения основного ТН (для алгоритмов).</summary>
    public string MainVtPlaceCode { get; init; } = string.Empty;

    /// <summary>Русское место подключения резервного ТН (для UI/логов).</summary>
    public string ReserveVtPlace { get; init; } = string.Empty;

    /// <summary>Код места подключения резервного ТН (для алгоритмов).</summary>
    public string ReserveVtPlaceCode { get; init; } = string.Empty;

    // =========================
    // Функции (паспорт + оперативное состояние)
    // =========================

    public bool HasDFZ { get; init; }
    public bool DFZEnabled { get; init; }

    public bool BothLineBreakerCTsOnSubstationSide { get; init; }
    public bool IsOnlyFunctionInDevice { get; init; }
    public bool HasMtzoShinovka { get; init; }

    public bool HasDZL { get; init; }
    public bool DZLEnabled { get; init; }

    public bool HasDZ { get; init; }
    public bool DZEnabled { get; init; }

    public bool HasOAPV { get; init; }
    public bool OAPVEnabled { get; init; }

    public bool HasTAPV { get; init; }
    public bool TAPVEnabled { get; init; }

    // =========================
    // Технологические флаги (device)
    // =========================

    public bool NeedDisableUpaskReceivers { get; init; }
    public bool NeedDisconnectLineCTFromDZO { get; init; }

    /// <summary>
    /// Остаются ли токовые цепи (CT) энергизированными на данной стороне.
    /// Используется ветками логики, где важно понимать, остаётся ли питание/энергия в токовых цепях на стороне.
    /// </summary>
    public bool CtRemainsEnergizedOnThisSide { get; init; }

    public bool IsFieldClosingAllowed { get; init; }
}
