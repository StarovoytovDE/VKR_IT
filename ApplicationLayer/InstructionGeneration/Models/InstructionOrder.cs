namespace ApplicationLayer.InstructionGeneration.Models;

/// <summary>
/// Defines base order values for instruction items.
/// </summary>
public static class InstructionOrder
{
    /// <summary>
    /// Base order for MTZO operations.
    /// </summary>
    public const int Mtzo = 10;

    /// <summary>
    /// Base order for DFZ operations.
    /// </summary>
    public const int Dfz = 100;

    /// <summary>
    /// Base order for DZL operations.
    /// </summary>
    public const int Dzl = 200;

    /// <summary>
    /// Base order for DZ operations.
    /// </summary>
    public const int Dz = 300;

    /// <summary>
    /// Base order for OAPV operations.
    /// </summary>
    public const int Oapv = 400;

    /// <summary>
    /// Base order for TAPV operations.
    /// </summary>
    public const int Tapv = 500;

    /// <summary>
    /// Base order for UPASK operations.
    /// </summary>
    public const int Upask = 600;

    /// <summary>
    /// Base order for switching VT operations.
    /// </summary>
    public const int SwitchVt = 700;

    /// <summary>
    /// Base order for disconnecting CT from DZO operations.
    /// </summary>
    public const int DisconnectCtDzo = 800;
}
