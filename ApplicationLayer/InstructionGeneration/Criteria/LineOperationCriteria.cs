using ApplicationLayer.InstructionGeneration.Models;

namespace ApplicationLayer.InstructionGeneration.Criteria;

/// <summary>
/// Определяет критерии для одной операции линии для конкретного устройства и стороны.
/// </summary>
public sealed record LineOperationCriteria
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="LineOperationCriteria"/>.
    /// </summary>
    /// <param name="lineCode">Идентификатор линии.</param>
    /// <param name="side">Сторона линии.</param>
    /// <param name="deviceObjectId">Идентификатор объекта устройства.</param>
    /// <param name="actionCode">Код действия для операции.</param>
    public LineOperationCriteria(string lineCode, SideOfLine side, int deviceObjectId, ActionCode actionCode)
    {
        LineCode = lineCode;
        Side = side;
        DeviceObjectId = deviceObjectId;
        ActionCode = actionCode;
    }

    /// <summary>
    /// Получает идентификатор линии.
    /// </summary>
    public string LineCode { get; init; }

    /// <summary>
    /// Получает сторону линии.
    /// </summary>
    public SideOfLine Side { get; init; }

    /// <summary>
    /// Получает идентификатор объекта устройства.
    /// </summary>
    public int DeviceObjectId { get; init; }

    /// <summary>
    /// Получает необязательное имя устройства.
    /// </summary>
    public string? DeviceName { get; init; }

    /// <summary>
    /// Получает код действия.
    /// </summary>
    public ActionCode ActionCode { get; init; }

    /// <summary>
    /// Получает значение, указывающее, есть ли у устройства DFZ.
    /// </summary>
    public bool HasDFZ { get; init; }

    /// <summary>
    /// Получает значение, указывающее, включен ли DFZ.
    /// </summary>
    public bool DFZEnabled { get; init; }

    /// <summary>
    /// Получает значение, указывающее, подключен ли DFZ к трансформатору тока линии.
    /// </summary>
    public bool DFZConnectedToLineCT { get; init; }

    /// <summary>
    /// Получает значение, указывающее, подключен ли DFZ к трансформатору напряжения линии.
    /// </summary>
    public bool DFZConnectedToLineVT { get; init; }

    /// <summary>
    /// Получает значение, указывающее, нужно ли переключить трансформатор напряжения линии на резерв.
    /// </summary>
    public bool NeedSwitchLineVTToReserve { get; init; }

    /// <summary>
    /// Получает значение, указывающее, нужно ли переключить трансформатор напряжения шины на резерв.
    /// </summary>
    public bool NeedSwitchBusVTToReserve { get; init; }

    /// <summary>
    /// Получает значение, указывающее, находятся ли оба трансформатора тока выключателя на обратной стороне линии.
    /// </summary>
    public bool BothCbctReverseSide { get; init; }

    /// <summary>
    /// Получает значение, указывающее, является ли функция единственной в устройстве.
    /// </summary>
    public bool IsOnlyFunctionInDevice { get; init; }

    /// <summary>
    /// Получает значение, указывающее, есть ли у устройства DZL.
    /// </summary>
    public bool HasDZL { get; init; }

    /// <summary>
    /// Получает значение, указывающее, включен ли DZL.
    /// </summary>
    public bool DZLEnabled { get; init; }

    /// <summary>
    /// Получает значение, указывающее, есть ли у устройства DZ.
    /// </summary>
    public bool HasDZ { get; init; }

    /// <summary>
    /// Получает значение, указывающее, включен ли DZ.
    /// </summary>
    public bool DZEnabled { get; init; }

    /// <summary>
    /// Получает значение, указывающее, подключен ли DZ к трансформатору напряжения линии.
    /// </summary>
    public bool DZConnectedToLineVT { get; init; }

    /// <summary>
    /// Получает значение, указывающее, есть ли у устройства OAPV.
    /// </summary>
    public bool HasOAPV { get; init; }

    /// <summary>
    /// Получает значение, указывающее, включен ли OAPV.
    /// </summary>
    public bool OAPVEnabled { get; init; }

    /// <summary>
    /// Получает значение, указывающее, есть ли у устройства TAPV.
    /// </summary>
    public bool HasTAPV { get; init; }

    /// <summary>
    /// Получает значение, указывающее, включен ли TAPV.
    /// </summary>
    public bool TAPVEnabled { get; init; }

    /// <summary>
    /// Получает значение, указывающее, должны ли быть отключены приемники UPASK.
    /// </summary>
    public bool NeedDisableUpaskReceivers { get; init; }

    /// <summary>
    /// Получает значение, указывающее, должен ли трансформатор тока линии быть отключен от DZO.
    /// </summary>
    public bool NeedDisconnectLineCTFromDZO { get; init; }

    /// <summary>
    /// Получает значение, указывающее, требуется ли переключение MTZO шиновки с A на B.
    /// </summary>
    public bool NeedMtzoShinovkaAtoB { get; init; }

    /// <summary>
    /// Получает значение, указывающее, остается ли трансформатор тока под напряжением на этой стороне.
    /// </summary>
    public bool CtRemainsEnergizedOnThisSide { get; init; }

    /// <summary>
    /// Получает значение, указывающее, разрешено ли включение поля.
    /// </summary>
    public bool IsFieldClosingAllowed { get; init; }
}
