namespace ApplicationLayer.InstructionGeneration.DeviceParams;

/// <summary>
/// Снимок состояния ОАПВ: наличие/состояние + дополнительный флаг SwitchOff.
/// </summary>
public sealed class ApvStateSnapshot
{
    /// <summary>Состояние функции ОАПВ (наличие/введена).</summary>
    public FunctionStateSnapshot State { get; init; } = new();

    /// <summary>Признак «ОАПВ отключено» (SwitchOff) — если хранится в БД.</summary>
    public bool SwitchOff { get; init; }
}
