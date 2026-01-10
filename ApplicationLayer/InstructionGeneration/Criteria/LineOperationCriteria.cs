using ApplicationLayer.InstructionGeneration.Models;

namespace ApplicationLayer.InstructionGeneration.Criteria;

/// <summary>
/// Контекст (критерии) выполнения действия над линией
/// для конкретного устройства РЗ и СА и стороны линии.
///
/// Содержит агрегированные параметры состояния устройства и схемы,
/// используемые алгоритмами операций при формировании указаний.
/// </summary>
public sealed record LineOperationCriteria
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="LineOperationCriteria"/>.
    /// </summary>
    /// <param name="lineCode">Идентификатор линии.</param>
    /// <param name="side">Сторона линии.</param>
    /// <param name="deviceObjectId">Идентификатор объекта устройства.</param>
    /// <param name="actionCode">Код действия (сценария), определяющий набор операций.</param>
    public LineOperationCriteria(
        string lineCode,
        SideOfLine side,
        int deviceObjectId,
        ActionCode actionCode)
    {
        LineCode = lineCode;
        Side = side;
        DeviceObjectId = deviceObjectId;
        ActionCode = actionCode;
    }

    /// <summary>
    /// Идентификатор линии.
    /// </summary>
    public string LineCode { get; init; }

    /// <summary>
    /// Сторона линии.
    /// </summary>
    public SideOfLine Side { get; init; }

    /// <summary>
    /// Идентификатор объекта устройства РЗ и СА.
    /// </summary>
    public int DeviceObjectId { get; init; }

    /// <summary>
    /// Необязательное имя устройства (для UI/журналирования).
    /// </summary>
    public string? DeviceName { get; init; }

    /// <summary>
    /// Код действия (сценария), определяющий набор операций.
    /// </summary>
    public ActionCode ActionCode { get; init; }

    // =========================
    // Параметры функции ДФЗ
    // =========================

    /// <summary>Есть ли у устройства функция ДФЗ.</summary>
    public bool HasDFZ { get; init; }

    /// <summary>Введена ли функция ДФЗ.</summary>
    public bool DFZEnabled { get; init; }

    /// <summary>Подключено ли Устройсво РЗ и СА к линейному ТТ.</summary>
    public bool DeviceConnectedToLineCT { get; init; }

    /// <summary>Подключен ли ДФЗ к линейному ТН.</summary>
    public bool DFZConnectedToLineVT { get; init; }

    /// <summary>Требуется ли перевод ТН на резерв.</summary>
    public bool NeedSwitchVTToReserve { get; init; }

    /// <summary>Требуется ли перевод линейного ТН на резерв.</summary>
    public bool NeedSwitchFromLineVTToReserve { get; init; }

    /// <summary>Требуется ли перевод шинного ТН на резерв.</summary>
    public bool NeedSwitchFromBusVTToReserve { get; init; }

    /// <summary>Оба ТТ выключателя находятся со стороны подстанции.</summary>
    public bool BothLineBreakerCTsOnSubstationSide { get; init; }

    /// <summary>Является ли функция единственной в устройстве.</summary>
    public bool IsOnlyFunctionInDevice { get; init; }

    /// <summary>
    /// Получает значение, указывающее, есть ли у устройства функция МТЗ ошиновки.
    /// </summary>
    public bool HasMtzoShinovka { get; init; }

    // =========================
    // Параметры функции ДЗЛ
    // =========================

    public bool HasDZL { get; init; }
    public bool DZLEnabled { get; init; }

    // =========================
    // Параметры функции ДЗ
    // =========================

    public bool HasDZ { get; init; }
    public bool DZEnabled { get; init; }
    public bool DZConnectedToLineVT { get; init; }

    // =========================
    // Параметры АПВ / ОАПВ / ТАПВ
    // =========================

    public bool HasOAPV { get; init; }
    public bool OAPVEnabled { get; init; }

    public bool HasTAPV { get; init; }
    public bool TAPVEnabled { get; init; }

    // =========================
    // Прочие параметры
    // =========================

    /// <summary>Необходимо ли отключить приемники УПАСК.</summary>
    public bool NeedDisableUpaskReceivers { get; init; }

    /// <summary>Необходимо ли отключить линейный ТТ от ДЗО.</summary>
    public bool NeedDisconnectLineCTFromDZO { get; init; }

    /// <summary>Требуется ли переключение МТЗО шиновки с A на B.</summary>
    public bool NeedMtzoShinovkaAtoB { get; init; }

    /// <summary>Остается ли ТТ под напряжением на данной стороне.</summary>
    public bool CtRemainsEnergizedOnThisSide { get; init; }

    /// <summary>Разрешено ли замыкание поля.</summary>
    public bool IsFieldClosingAllowed { get; init; }
}
