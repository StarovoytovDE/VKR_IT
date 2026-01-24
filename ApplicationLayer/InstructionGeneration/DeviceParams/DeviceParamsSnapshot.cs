namespace ApplicationLayer.InstructionGeneration.DeviceParams;

/// <summary>
/// Снимок параметров устройства РЗА, необходимый для формирования критериев операций.
/// Это «плоская» DTO-модель, чтобы генератор критериев не зависел от EF Core / DbContext.
/// </summary>
public sealed class DeviceParamsSnapshot
{
    /// <summary>
    /// Идентификатор устройства (PK таблицы device).
    /// </summary>
    public required long DeviceId { get; init; }

    /// <summary>
    /// Идентификатор объекта энергосистемы, к которому относится устройство (ObjectId).
    /// </summary>
    public required long ObjectId { get; init; }

    /// <summary>
    /// Диспетчерское/понятное имя устройства.
    /// </summary>
    public required string DeviceName { get; init; }

    /// <summary>
    /// Признак истинности логики переключения ТН (как в таблице device: VtSwitchTrue).
    /// </summary>
    public required bool VtSwitchTrue { get; init; }

    /// <summary>
    /// Снимок параметров измерительных ТН (ровно 2 штуки: основной и резервный).
    /// </summary>
    public required VtPairSnapshot Vts { get; init; }

    /// <summary>
    /// Место подключения ТТ (1:1 запись ct_place).
    /// </summary>
    public required CtPlaceSnapshot CtPlace { get; init; }

    /// <summary>
    /// Параметры функции ДФЗ (паспорт: наличие).
    /// Оперативное состояние (введено/не введено) задаёт диспетчер через запрос.
    /// </summary>
    public required FunctionStateSnapshot Dfz { get; init; }

    /// <summary>
    /// Параметры функции ДЗЛ (паспорт: наличие).
    /// </summary>
    public required FunctionStateSnapshot Dzl { get; init; }

    /// <summary>
    /// Параметры функции ДЗ (паспорт: наличие).
    /// </summary>
    public required FunctionStateSnapshot Dz { get; init; }

    /// <summary>
    /// Параметры ОАПВ.
    /// </summary>
    public required OapvSnapshot Oapv { get; init; }

    /// <summary>
    /// Параметры ТАПВ.
    /// </summary>
    public required FunctionStateSnapshot Tapv { get; init; }

    /// <summary>
    /// Технологический флаг: разрешено ли замыкание поля (паспорт/технолог).
    /// </summary>
    public bool IsFieldClosingAllowed { get; init; }

    /// <summary>
    /// Технологический флаг: требуется ли вывести приёмники УПАСК.
    /// </summary>
    public bool NeedDisableUpaskReceivers { get; init; }

    /// <summary>
    /// Технологический флаг: требуется ли отключить линейный ТТ от ДЗО.
    /// </summary>
    public bool NeedDisconnectLineCTFromDzo { get; init; }

    /// <summary>
    /// Технологический флаг: требуется ли переключение МТЗО ошиновки с A на B.
    /// </summary>
    public bool NeedMtzoShinovkaAtoB { get; init; }
}

/// <summary>
/// Снимок состояния/наличия функции.
/// </summary>
public sealed class FunctionStateSnapshot
{
    /// <summary>
    /// Признак наличия функции в устройстве (паспорт).
    /// </summary>
    public required bool Has { get; init; }
}

/// <summary>
/// Снимок параметров ОАПВ.
/// </summary>
public sealed class OapvSnapshot
{
    /// <summary>
    /// Параметры наличия (паспорт).
    /// </summary>
    public required FunctionStateSnapshot State { get; init; }

    /// <summary>
    /// Признак «отключено переключателем/ключом» (в БД: SwitchOff).
    /// </summary>
    public required bool SwitchOff { get; init; }
}

/// <summary>
/// Снимок места подключения ТТ.
/// </summary>
public sealed class CtPlaceSnapshot
{
    /// <summary>
    /// Наименование (как в CtPlace.Name).
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Место подключения (русское значение для UI/выводов), как в CtPlace.Place.
    /// </summary>
    public required string Place { get; init; }

    /// <summary>
    /// Код места подключения (для алгоритмов), как в CtPlace.PlaceCode.
    /// </summary>
    public required string PlaceCode { get; init; }
}

/// <summary>
/// Пара ТН: основной + резервный.
/// </summary>
public sealed class VtPairSnapshot
{
    /// <summary>
    /// Основной ТН (Main=true).
    /// </summary>
    public required VtSnapshot Main { get; init; }

    /// <summary>
    /// Резервный ТН (Main=false).
    /// </summary>
    public required VtSnapshot Reserve { get; init; }
}

/// <summary>
/// Снимок одного ТН.
/// </summary>
public sealed class VtSnapshot
{
    /// <summary>
    /// Наименование ТН.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Место подключения (русское значение для UI/выводов), как в Vt.Place.
    /// </summary>
    public required string Place { get; init; }

    /// <summary>
    /// Код места подключения (для алгоритмов), как в Vt.PlaceCode.
    /// </summary>
    public required string PlaceCode { get; init; }
}
