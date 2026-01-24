namespace ApplicationLayer.InstructionGeneration.DeviceParams;

/// <summary>
/// Плоский снимок параметров устройства РЗ и СА, используемый генерацией указаний.
/// Содержит паспортные параметры, признаки наличия функций и технологические флаги сценариев.
/// </summary>
public sealed class DeviceParamsSnapshot
{
    /// <summary>Идентификатор устройства.</summary>
    public long DeviceId { get; init; }

    /// <summary>Идентификатор объекта (Object) устройства.</summary>
    public long ObjectId { get; init; }

    /// <summary>Наименование устройства (для UI/логов).</summary>
    public string DeviceName { get; init; } = string.Empty;

    /// <summary>Признак истинности логики перевода цепей напряжения на резерв.</summary>
    public bool VtSwitchTrue { get; init; }

    /// <summary>Снимок места подключения ТТ (CT).</summary>
    public CtPlaceSnapshot CtPlace { get; init; } = new();

    /// <summary>Пара ТН (VT): основной и резервный.</summary>
    public VtPairSnapshot Vts { get; init; } = new();

    /// <summary>ДФЗ: паспорт (наличие) и состояние в БД (если нужно).</summary>
    public FunctionStateSnapshot Dfz { get; init; } = new();

    /// <summary>ДЗЛ: паспорт (наличие) и состояние в БД (если нужно).</summary>
    public FunctionStateSnapshot Dzl { get; init; } = new();

    /// <summary>ДЗ: паспорт (наличие) и состояние в БД (если нужно).</summary>
    public FunctionStateSnapshot Dz { get; init; } = new();

    /// <summary>ОАПВ: паспорт/состояние + признак отключения (SwitchOff).</summary>
    public OapvStateSnapshot Oapv { get; init; } = new();

    /// <summary>ТАПВ: паспорт (наличие) и состояние в БД (если нужно).</summary>
    public FunctionStateSnapshot Tapv { get; init; } = new();

    // =========================
    // Технологические флаги сценариев (пока могут задаваться вручную / позже из БД)
    // =========================

    /// <summary>Разрешено ли замыкание поля (технологический флаг).</summary>
    public bool IsFieldClosingAllowed { get; init; }

    /// <summary>Нужно ли выводить приёмники УПАСК (технологический флаг).</summary>
    public bool NeedDisableUpaskReceivers { get; init; }

    /// <summary>Нужно ли отключать токовые цепи линейного ТТ от ДЗО данной ВЛ (технологический флаг).</summary>
    public bool NeedDisconnectLineCTFromDzo { get; init; }

    /// <summary>Нужно ли переключать уставки МТЗ ошиновки A→B (технологический флаг).</summary>
    public bool NeedMtzoShinovkaAtoB { get; init; }
}
