using ApplicationLayer.InstructionGeneration.Models;

namespace ApplicationLayer.InstructionGeneration.Criteria;

/// <summary>
/// Defines criteria for a single line operation for a specific device and side.
/// </summary>
public sealed record LineOperationCriteria
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LineOperationCriteria"/> class.
    /// </summary>
    /// <param name="lineCode">The line identifier.</param>
    /// <param name="side">The side of the line.</param>
    /// <param name="deviceObjectId">The device object identifier.</param>
    /// <param name="actionCode">The action code for the operation.</param>
    public LineOperationCriteria(string lineCode, SideOfLine side, int deviceObjectId, ActionCode actionCode)
    {
        LineCode = lineCode;
        Side = side;
        DeviceObjectId = deviceObjectId;
        ActionCode = actionCode;
    }

    /// <summary>
    /// Gets the line identifier.
    /// </summary>
    public string LineCode { get; init; }

    /// <summary>
    /// Gets the side of the line.
    /// </summary>
    public SideOfLine Side { get; init; }

    /// <summary>
    /// Gets the device object identifier.
    /// </summary>
    public int DeviceObjectId { get; init; }

    /// <summary>
    /// Gets the optional device name.
    /// </summary>
    public string? DeviceName { get; init; }

    /// <summary>
    /// Gets the action code.
    /// </summary>
    public ActionCode ActionCode { get; init; }

    /// <summary>
    /// Gets a value indicating whether the device has DFZ.
    /// </summary>
    public bool HasDFZ { get; init; }

    /// <summary>
    /// Gets a value indicating whether DFZ is enabled.
    /// </summary>
    public bool DFZEnabled { get; init; }

    /// <summary>
    /// Gets a value indicating whether DFZ is connected to line CT.
    /// </summary>
    public bool DFZConnectedToLineCT { get; init; }

    /// <summary>
    /// Gets a value indicating whether DFZ is connected to line VT.
    /// </summary>
    public bool DFZConnectedToLineVT { get; init; }

    /// <summary>
    /// Gets a value indicating whether line VT should be switched to reserve.
    /// </summary>
    public bool NeedSwitchLineVTToReserve { get; init; }

    /// <summary>
    /// Gets a value indicating whether bus VT should be switched to reserve.
    /// </summary>
    public bool NeedSwitchBusVTToReserve { get; init; }

    /// <summary>
    /// Gets a value indicating whether both CB CTs are on the reverse side of the line.
    /// </summary>
    public bool BothCbctReverseSide { get; init; }

    /// <summary>
    /// Gets a value indicating whether the function is the only one in the device.
    /// </summary>
    public bool IsOnlyFunctionInDevice { get; init; }

    /// <summary>
    /// Gets a value indicating whether the device has DZL.
    /// </summary>
    public bool HasDZL { get; init; }

    /// <summary>
    /// Gets a value indicating whether DZL is enabled.
    /// </summary>
    public bool DZLEnabled { get; init; }

    /// <summary>
    /// Gets a value indicating whether the device has DZ.
    /// </summary>
    public bool HasDZ { get; init; }

    /// <summary>
    /// Gets a value indicating whether DZ is enabled.
    /// </summary>
    public bool DZEnabled { get; init; }

    /// <summary>
    /// Gets a value indicating whether DZ is connected to line VT.
    /// </summary>
    public bool DZConnectedToLineVT { get; init; }

    /// <summary>
    /// Gets a value indicating whether the device has OAPV.
    /// </summary>
    public bool HasOAPV { get; init; }

    /// <summary>
    /// Gets a value indicating whether OAPV is enabled.
    /// </summary>
    public bool OAPVEnabled { get; init; }

    /// <summary>
    /// Gets a value indicating whether the device has TAPV.
    /// </summary>
    public bool HasTAPV { get; init; }

    /// <summary>
    /// Gets a value indicating whether TAPV is enabled.
    /// </summary>
    public bool TAPVEnabled { get; init; }

    /// <summary>
    /// Gets a value indicating whether UPASK receivers should be disabled.
    /// </summary>
    public bool NeedDisableUpaskReceivers { get; init; }

    /// <summary>
    /// Gets a value indicating whether line CT should be disconnected from DZO.
    /// </summary>
    public bool NeedDisconnectLineCTFromDZO { get; init; }

    /// <summary>
    /// Gets a value indicating whether MTZO shinovka A to B is required.
    /// </summary>
    public bool NeedMtzoShinovkaAtoB { get; init; }

    /// <summary>
    /// Gets a value indicating whether the CT remains energized on this side.
    /// </summary>
    public bool CtRemainsEnergizedOnThisSide { get; init; }

    /// <summary>
    /// Gets a value indicating whether field closing is allowed.
    /// </summary>
    public bool IsFieldClosingAllowed { get; init; }
}
