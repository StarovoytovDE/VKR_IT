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
    /// Признак истинности логики переключения ТН (как у тебя в таблице device: VtSwitchTrue).
    /// </summary>
    public required bool VtSwitchTrue { get; init; }

    /// <summary>
    /// Снимок параметров измерительных ТН (ровно 2 штуки: основной и резервный).
    /// </summary>
    public required VtPairSnapshot Vts { get; init; }

    /// <summary>
    /// Место подключения ТТ (для критериев «подключено к линейному ТТ» и т.п.).
    /// </summary>
    public required CtPlaceSnapshot CtPlace { get; init; }

    /// <summary>
    /// Параметры функции ДФЗ (паспорт: наличие).
    /// Оперативное состояние (введено/не введено) задаёт диспетчер через запрос.
    /// </summary>
    public required FunctionStateSnapshot Dfz { get; init; }

    /// <summary>
    /// Параметры функции ДЗЛ (паспорт: наличие).
    /// Оперативное состояние задаёт диспетчер.
    /// </summary>
    public required FunctionStateSnapshot Dzl { get; init; }

    /// <summary>
    /// Параметры функции ДЗ (паспорт: наличие).
    /// Оперативное состояние задаёт диспетчер.
    /// </summary>
    public required FunctionStateSnapshot Dz { get; init; }

    /// <summary>
    /// Снимок ОАПВ (паспорт: наличие; оперативное состояние задаёт диспетчер).
    /// </summary>
    public required OapvStateSnapshot Oapv { get; init; }

    /// <summary>
    /// Параметры функции ТАПВ (паспорт: наличие).
    /// Оперативное состояние задаёт диспетчер.
    /// </summary>
    public required FunctionStateSnapshot Tapv { get; init; }

    // =====================================================================
    // Технологические (паспортные) флаги сценариев / ограничений
    // =====================================================================

    /// <summary>
    /// Допустима ли операция «замыкание поля» для линии/объекта.
    /// Технологический параметр (задаётся заранее).
    /// </summary>
    public required bool IsFieldClosingAllowed { get; init; }

    /// <summary>
    /// Требуется ли вывод приёмников УПАСК при выполнении работ.
    /// Технологический параметр (задаётся заранее).
    /// </summary>
    public required bool NeedDisableUpaskReceivers { get; init; }

    /// <summary>
    /// Требуется ли отключение ТТ линии от ДЗО при выполнении работ.
    /// Технологический параметр (задаётся заранее).
    /// </summary>
    public required bool NeedDisconnectLineCTFromDzo { get; init; }

    /// <summary>
    /// Требуется ли перевод МТЗО ошиновки А→Б при выполнении работ.
    /// Технологический параметр (задаётся заранее).
    /// </summary>
    public required bool NeedMtzoShinovkaAtoB { get; init; }
}

/// <summary>
/// Снимок состояния «обычной» функции: наличие + введена/включена.
/// Наличие — паспорт (технолог), введена/включена — оперативно (диспетчер).
/// </summary>
public sealed class FunctionStateSnapshot
{
    /// <summary>
    /// Признак наличия функции на устройстве (в твоей БД это HazXxx).
    /// </summary>
    public required bool Has { get; init; }

    /// <summary>
    /// Признак, что функция введена/включена.
    /// В итоговых критериях берётся из оперативного запроса диспетчера.
    /// </summary>
    public required bool Enabled { get; init; }
}

/// <summary>
/// Снимок состояния ОАПВ.
/// Содержит общее состояние функции (наличие/включена) и специфический признак SwitchOff.
/// </summary>
public sealed class OapvStateSnapshot
{
    /// <summary>
    /// Общее состояние функции (наличие/введена).
    /// </summary>
    public required FunctionStateSnapshot State { get; init; }

    /// <summary>
    /// Признак «ОАПВ выведено переключателем/ключом» (в твоей БД: SwitchOff).
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
    /// Код/описание места (как в CtPlace.Place).
    /// </summary>
    public required string Place { get; init; }
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
    /// Место подключения (например: LINE / BUS / BUS_RESERVE и т.п.).
    /// </summary>
    public required string Place { get; init; }
}
