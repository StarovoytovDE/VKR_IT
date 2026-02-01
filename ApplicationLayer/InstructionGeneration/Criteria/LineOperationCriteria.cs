using ApplicationLayer.InstructionGeneration.Models;

namespace ApplicationLayer.InstructionGeneration.Criteria;

/// <summary>
/// Критерии для генерации указаний по операциям с устройством на линии.
/// Содержит параметры устройства (из snapshot БД) + ввод диспетчера (из request).
/// </summary>
public sealed class LineOperationCriteria
{
    /// <summary>
    /// Создаёт критерии.
    /// </summary>
    /// <param name="lineCode">Код/диспетчерское имя линии.</param>
    /// <param name="side">Сторона линии.</param>
    /// <param name="deviceObjectId">Идентификатор устройства в критериях/логах.</param>
    /// <param name="actionCode">Код действия (сценария).</param>
    public LineOperationCriteria(string lineCode, SideOfLine side, int deviceObjectId, ActionCode actionCode)
    {
        LineCode = lineCode;
        Side = side;
        DeviceObjectId = deviceObjectId;
        ActionCode = actionCode;
    }

    /// <summary>Код/диспетчерское имя линии.</summary>
    public string LineCode { get; init; }

    /// <summary>Сторона линии.</summary>
    public SideOfLine Side { get; init; }

    /// <summary>Идентификатор устройства (для логов/трассировки).</summary>
    public int DeviceObjectId { get; init; }

    /// <summary>Имя устройства (для UI/логов).</summary>
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

    /// <summary>Имя основного ТН (для подстановки в текст указания).</summary>
    public string MainVtName { get; init; } = string.Empty;

    /// <summary>Русское место подключения основного ТН (для UI/логов).</summary>
    public string MainVtPlace { get; init; } = string.Empty;

    /// <summary>Код места подключения основного ТН (для алгоритмов).</summary>
    public string MainVtPlaceCode { get; init; } = string.Empty;

    /// <summary>Имя резервного ТН (для подстановки в текст указания).</summary>
    public string ReserveVtName { get; init; } = string.Empty;

    /// <summary>Русское место подключения резервного ТН (для UI/логов).</summary>
    public string ReserveVtPlace { get; init; } = string.Empty;

    /// <summary>Код места подключения резервного ТН (для алгоритмов).</summary>
    public string ReserveVtPlaceCode { get; init; } = string.Empty;

    // =========================
    // Функции (оперативное состояние + флаги из БД)
    // =========================

    /// <summary>Наличие ДФЗ в устройстве (паспорт/конфигурация).</summary>
    public bool HasDFZ { get; init; }

    /// <summary>Ввод диспетчера: ДФЗ введена.</summary>
    public bool DFZEnabled { get; init; }

    /// <summary>Оба выключателя линии имеют ТТ со стороны ПС (для частных веток логики).</summary>
    public bool BothLineBreakerCTsOnSubstationSide { get; init; }

    /// <summary>Функция единственная в устройстве (для правила "вывести устройство").</summary>
    public bool IsOnlyFunctionInDevice { get; init; }

    /// <summary>Наличие МТЗ ошиновки.</summary>
    public bool HasMtzoShinovka { get; init; }

    /// <summary>Наличие ДЗЛ в устройстве.</summary>
    public bool HasDZL { get; init; }

    /// <summary>Ввод диспетчера: ДЗЛ введена.</summary>
    public bool DZLEnabled { get; init; }

    /// <summary>Наличие ДЗ в устройстве.</summary>
    public bool HasDZ { get; init; }

    /// <summary>Ввод диспетчера: ДЗ введена.</summary>
    public bool DZEnabled { get; init; }

    /// <summary>
    /// Ввод диспетчера: ОАПВ выбрана/учитывается в сценарии.
    /// </summary>
    public bool OAPVEnabled { get; init; }

    /// <summary>
    /// Состояние ОАПВ из БД (state).
    /// True — функция находится во включенном/активном состоянии.
    /// </summary>
    public bool OAPVState { get; init; }

    /// <summary>
    /// Флаг ОАПВ из БД (switch_off).
    /// True — требуется вывод (ОАПВ отключено / подлежит выводу по принятой логике).
    /// </summary>
    public bool OAPVSwitchOff { get; init; }

    /// <summary>
    /// Ввод диспетчера: ТАПВ выбрана/учитывается в сценарии.
    /// </summary>
    public bool TAPVEnabled { get; init; }

    /// <summary>
    /// Состояние ТАПВ из БД (state).
    /// </summary>
    public bool TAPVState { get; init; }

    /// <summary>
    /// Флаг ТАПВ из БД (switch_off).
    /// </summary>
    public bool TAPVSwitchOff { get; init; }

    // =========================
    // Технологические флаги (device)
    // =========================

    /// <summary>Требуется ли вывод приёмников УПАСК.</summary>
    public bool NeedDisableUpaskReceivers { get; init; }

    /// <summary>Требуется ли отключение линейного ТТ от ДЗО.</summary>
    public bool NeedDisconnectLineCTFromDZO { get; init; }

    /// <summary>
    /// Остаются ли токовые цепи (CT) энергизированными на данной стороне.
    /// Используется ветками логики, где важно понимать, остаётся ли питание/энергия в токовых цепях на стороне.
    /// </summary>
    public bool CtRemainsEnergizedOnThisSide { get; init; }

    /// <summary>Разрешено ли замыкание поля.</summary>
    public bool IsFieldClosingAllowed { get; init; }
}
