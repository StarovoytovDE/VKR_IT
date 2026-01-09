namespace ApplicationLayer.InstructionGeneration.Models;

/// <summary>
/// Describes the action scenario for line operations.
/// </summary>
public enum ActionCode
{
    /// <summary>
    /// Output the line with field closing.
    /// </summary>
    VlOutFieldClosing,

    /// <summary>
    /// Output the line without field closing.
    /// </summary>
    VlOutNoFieldClosing,

    /// <summary>
    /// Output the line when line disconnectors are open and bus connection remains.
    /// </summary>
    VlOutLineDisconnectorsOpenBusIsOn,

    /// <summary>
    /// Output the line from one side only.
    /// </summary>
    VlOutOneSide
}
