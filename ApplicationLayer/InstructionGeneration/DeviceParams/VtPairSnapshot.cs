namespace ApplicationLayer.InstructionGeneration.DeviceParams;

/// <summary>
/// Пара ТН (VT): основной и резервный.
/// </summary>
public sealed class VtPairSnapshot
{
    /// <summary>Основной ТН.</summary>
    public VtSnapshot Main { get; init; } = new();

    /// <summary>Резервный ТН.</summary>
    public VtSnapshot Reserve { get; init; } = new();
}
