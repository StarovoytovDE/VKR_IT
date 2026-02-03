namespace Domain.Entities;

/// <summary>
/// Трансформатор напряжения (ТН / VT).
/// 
/// В текущей модели ТН не хранит FK на устройство. Вместо этого таблица device
/// хранит два FK: main_vt_id и reserve_vt_id.
/// </summary>
public sealed class Vt
{
    /// <summary>
    /// Идентификатор ТН (PK).
    /// </summary>
    public long VtId { get; set; }

    /// <summary>
    /// Наименование ТН.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Место подключения (русское отображаемое значение).
    /// </summary>
    public string Place { get; set; } = string.Empty;

    /// <summary>
    /// Код места подключения (для алгоритмов).
    /// Например: VT_LINE / VT_BUS / VT_BUSBAR.
    /// </summary>
    public string PlaceCode { get; set; } = string.Empty;
}
