namespace Domain.Entities;

/// <summary>
/// Привязки параметров запроса к конкретным сущностям устройства (как в диаграмме request_param_value).
/// </summary>
public sealed class RequestParamValue
{
    /// <summary>
    /// Идентификатор записи.
    /// </summary>
    public long RequestParamValueId { get; set; }

    /// <summary>
    /// Идентификатор устройства.
    /// </summary>
    public long DeviceId { get; set; }

    /// <summary>
    /// Идентификатор запроса формирования указаний.
    /// </summary>
    public long InstructionRequestId { get; set; }

    /// <summary>
    /// Идентификатор ДФЗ (если выбран/использован).
    /// </summary>
    public long? DfzId { get; set; }

    /// <summary>
    /// Идентификатор ДЗЛ (если выбран/использован).
    /// </summary>
    public long? DzlId { get; set; }

    /// <summary>
    /// Идентификатор ДЗ (если выбран/использован).
    /// </summary>
    public long? DzId { get; set; }

    /// <summary>
    /// Идентификатор ОАПВ (если выбран/использован).
    /// </summary>
    public long? OapvId { get; set; }

    /// <summary>
    /// Идентификатор ТАПВ (если выбран/использован).
    /// </summary>
    public long? TapvId { get; set; }

    /// <summary>
    /// Идентификатор МТЗ ошиновки (если выбран/использован).
    /// </summary>
    public long? MtzBusbarId { get; set; }

    /// <summary>
    /// Навигация на устройство.
    /// </summary>
    public Device Device { get; set; } = null!;

    /// <summary>
    /// Навигация на запрос.
    /// </summary>
    public InstructionRequest InstructionRequest { get; set; } = null!;

    /// <summary>
    /// Навигация на ДФЗ.
    /// </summary>
    public Dfz? Dfz { get; set; }

    /// <summary>
    /// Навигация на ДЗЛ.
    /// </summary>
    public Dzl? Dzl { get; set; }

    /// <summary>
    /// Навигация на ДЗ.
    /// </summary>
    public Dz? Dz { get; set; }

    /// <summary>
    /// Навигация на ОАПВ.
    /// </summary>
    public Oapv? Oapv { get; set; }

    /// <summary>
    /// Навигация на ТАПВ.
    /// </summary>
    public Tapv? Tapv { get; set; }

    /// <summary>
    /// Навигация на МТЗ ошиновки.
    /// </summary>
    public MtzBusbar? MtzBusbar { get; set; }
}
