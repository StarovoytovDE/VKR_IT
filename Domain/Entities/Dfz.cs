namespace Domain.Entities;

/// <summary>
/// Функция ДФЗ, привязанная к устройству.
/// </summary>
public sealed class Dfz
{
    /// <summary>
    /// Идентификатор ДФЗ.
    /// </summary>
    public long DfzId { get; set; }

    /// <summary>
    /// Идентификатор устройства.
    /// </summary>
    public long DeviceId { get; set; }

    /// <summary>
    /// Код функции.
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Наименование функции.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Признак наличия/актуальности (в диаграмме помечено как haz_dfz).
    /// </summary>
    public bool HazDfz { get; set; }

    /// <summary>
    /// Состояние функции (введена/не введена) — как в диаграмме state.
    /// </summary>
    public bool State { get; set; }

    /// <summary>
    /// Навигация на устройство.
    /// </summary>
    public Device Device { get; set; } = null!;
}
