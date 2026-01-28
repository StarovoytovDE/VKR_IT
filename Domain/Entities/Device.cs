using System.Collections.Generic;

namespace Domain.Entities;

/// <summary>
/// Устройство РЗА (привязано к концу линии).
/// Содержит как паспортные/технологические флаги устройства, так и связи на функции и места измерений.
/// </summary>
public sealed class Device
{
    /// <summary>
    /// Идентификатор устройства.
    /// </summary>
    public long DeviceId { get; set; }

    /// <summary>
    /// Идентификатор конца линии (line_end), к которому относится устройство.
    /// </summary>
    public long LineEndId { get; set; }

    /// <summary>
    /// Наименование устройства.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Признак истинности логики перевода цепей напряжения на резерв (vt_switch_true).
    /// </summary>
    public bool VtSwitchTrue { get; set; }

    /// <summary>
    /// Нужно ли отключать токовые цепи линейного ТТ от ДЗО данной ВЛ (dzo_switch_true).
    /// Технологический параметр, задаётся технологом.
    /// </summary>
    public bool DzoSwitchTrue { get; set; }

    /// <summary>
    /// Нужно ли выводить приёмники УПАСК (upask_switch_true).
    /// Технологический параметр, задаётся технологом.
    /// </summary>
    public bool UpaskSwitchTrue { get; set; }

    /// <summary>
    /// Разрешено ли замыкание поля (field_closing_allowed).
    /// Технологический параметр, задаётся технологом.
    /// </summary>
    public bool FieldClosingAllowed { get; set; }

    /// <summary>
    /// Остаются ли токовые цепи (CT) под напряжением/энергизированными на данной стороне (ct_remains_energized).
    /// Технологический параметр, задаётся технологом.
    /// </summary>
    public bool CtRemainsEnergized { get; set; }

    /// <summary>
    /// Навигация на конец линии.
    /// </summary>
    public LineEnd LineEnd { get; set; } = null!;

    /// <summary>
    /// Связанные записи ТН (VT).
    /// </summary>
    public ICollection<Vt> Vts { get; set; } = [];

    /// <summary>
    /// Возможные места подключения ТТ (CT place).
    /// </summary>
    public ICollection<CtPlace> CtPlaces { get; set; } = [];

    /// <summary>
    /// Функции ДФЗ.
    /// </summary>
    public ICollection<Dfz> Dfzs { get; set; } = [];

    /// <summary>
    /// Функции ДЗЛ.
    /// </summary>
    public ICollection<Dzl> Dzls { get; set; } = [];

    /// <summary>
    /// Функции ДЗ.
    /// </summary>
    public ICollection<Dz> Dzs { get; set; } = [];

    /// <summary>
    /// Функции ОАПВ.
    /// </summary>
    public ICollection<Oapv> Oapvs { get; set; } = [];

    /// <summary>
    /// Функции ТАПВ.
    /// </summary>
    public ICollection<Tapv> Tapvs { get; set; } = [];

    /// <summary>
    /// МТЗ ошиновки (как функция/запись, без технологического флага A→B).
    /// </summary>
    public ICollection<MtzBusbar> MtzBusbars { get; set; } = [];

    /// <summary>
    /// Параметры запроса (привязки), введённые/использованные при формировании указаний.
    /// </summary>
    public ICollection<RequestParamValue> RequestParamValues { get; set; } = [];
}
