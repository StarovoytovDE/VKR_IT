namespace Domain.Entities;

/// <summary>
/// Устройство РЗА, привязанное к объекту линии (object).
/// </summary>
public sealed class Device
{
    /// <summary>
    /// Идентификатор устройства.
    /// </summary>
    public long DeviceId { get; set; }

    /// <summary>
    /// Идентификатор объекта (как правило, линии), к которому относится устройство.
    /// </summary>
    public long ObjectId { get; set; }

    /// <summary>
    /// Наименование устройства.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Признак "переключение ТН выполнено/истина" (как в диаграмме vt_switch_true).
    /// </summary>
    public bool VtSwitchTrue { get; set; }

    /// <summary>
    /// Технологический признак необходимости отключения токовых цепей линейного ТТ от ДЗО данной ВЛ.
    /// Хранится в device.dzo_switch_true.
    /// </summary>
    public bool DzoSwitchTrue { get; set; }

    /// <summary>
    /// Технологический признак необходимости вывода приёмников УПАСК,
    /// по которым организована передача команд РЗ.
    /// Хранится в device.upask_switch_true.
    /// </summary>
    public bool UpaskSwitchTrue { get; set; }

    /// <summary>
    /// Технологический признак допустимости замыкания поля
    /// при выполнении операций с ВЛ.
    /// Хранится в device.field_closing_allowed.
    /// </summary>
    public bool FieldClosingAllowed { get; set; }

    /// <summary>
    /// ТТ выключателей ВЛ оба остаются под навряжением
    /// </summary>
    public bool CtRemainsEnergized { get; set; }

    /// <summary>
    /// Навигация на объект.
    /// </summary>
    public ObjectTable Object { get; set; } = null!;

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
    /// МТЗ ошиновки.
    /// </summary>
    public ICollection<MtzBusbar> MtzBusbars { get; set; } = [];

    /// <summary>
    /// Параметры запроса (привязки), введённые/использованные при формировании указаний.
    /// </summary>
    public ICollection<RequestParamValue> RequestParamValues { get; set; } = [];
}
