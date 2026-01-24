using ApplicationLayer.InstructionGeneration.Models;

namespace ApplicationLayer.InstructionGeneration.Requests;

/// <summary>
/// Оперативный запрос диспетчера на формирование указаний
/// для выбранной линии, стороны и сценария.
/// </summary>
public sealed class LineOperationRequest
{
    /// <summary>
    /// Код (идентификатор) линии электропередачи.
    /// Задаётся диспетчером.
    /// </summary>
    public required string LineCode { get; init; }

    /// <summary>
    /// Сторона линии (А / B).
    /// Задаётся диспетчером.
    /// </summary>
    public required SideOfLine Side { get; init; }

    /// <summary>
    /// Код действия (сценария).
    /// Задаётся диспетчером.
    /// </summary>
    public required ActionCode ActionCode { get; init; }

    /// <summary>
    /// Оперативные состояния функций РЗ и СА
    /// на момент формирования указаний (задаёт диспетчер).
    /// </summary>
    public required FunctionStatesRequest FunctionStates { get; init; }
}
