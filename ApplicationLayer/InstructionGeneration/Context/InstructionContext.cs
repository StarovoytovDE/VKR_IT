namespace ApplicationLayer.InstructionGeneration.Context;

public sealed class InstructionContext
{
    public InstructionContext(
        string actionCode,
        bool isFieldClosing,
        bool isFieldClosingAllowed,
        bool hasDfz,
        bool dfzNormalEnabled,
        bool dfzEnabled,
        bool connectedToLineCt,
        bool connectedToLineVt,
        bool needSwitchVtToReserve,
        bool bothCbctReverseSide,
        bool hasDzl,
        bool dzlNormalEnabled,
        bool dzlEnabled,
        bool hasDz,
        bool dzNormalEnabled,
        bool dzEnabled,
        bool needDisableOapv,
        bool needDisableOapvVl,
        bool needDisableTapv,
        bool needDisableTapvInDevice,
        bool needDisableUpaskReceivers,
        bool needDisconnectLineCtFromDzo,
        bool needMtzoShinovkaAtoB,
        bool needSwitchBusVtToReserve)
    {
        ActionCode = actionCode;
        IsFieldClosing = isFieldClosing;
        IsFieldClosingAllowed = isFieldClosingAllowed;
        HasDFZ = hasDfz;
        DFZNormalEnabled = dfzNormalEnabled;
        DFZEnabled = dfzEnabled;
        ConnectedToLineCT = connectedToLineCt;
        ConnectedToLineVT = connectedToLineVt;
        NeedSwitchVTToReserve = needSwitchVtToReserve;
        BothCBCTReverseSide = bothCbctReverseSide;
        HasDZL = hasDzl;
        DZLNormalEnabled = dzlNormalEnabled;
        DZLEnabled = dzlEnabled;
        HasDZ = hasDz;
        DZNormalEnabled = dzNormalEnabled;
        DZEnabled = dzEnabled;
        NeedDisableOAPV = needDisableOapv;
        NeedDisableOAPVVL = needDisableOapvVl;
        NeedDisableTAPV = needDisableTapv;
        NeedDisableTAPVInDevice = needDisableTapvInDevice;
        NeedDisableUPASKReceivers = needDisableUpaskReceivers;
        NeedDisconnectLineCTFromDZO = needDisconnectLineCtFromDzo;
        NeedMTZOshinovkaAtoB = needMtzoShinovkaAtoB;
        NeedSwitchBusVTToReserve = needSwitchBusVtToReserve;
    }

    public string ActionCode { get; }
    public bool IsFieldClosing { get; }
    public bool IsFieldClosingAllowed { get; }
    public bool HasDFZ { get; }
    public bool DFZNormalEnabled { get; }
    public bool DFZEnabled { get; }
    public bool ConnectedToLineCT { get; }
    public bool ConnectedToLineVT { get; }
    public bool NeedSwitchVTToReserve { get; }
    public bool BothCBCTReverseSide { get; }
    public bool HasDZL { get; }
    public bool DZLNormalEnabled { get; }
    public bool DZLEnabled { get; }
    public bool HasDZ { get; }
    public bool DZNormalEnabled { get; }
    public bool DZEnabled { get; }
    public bool NeedDisableOAPV { get; }
    public bool NeedDisableOAPVVL { get; }
    public bool NeedDisableTAPV { get; }
    public bool NeedDisableTAPVInDevice { get; }
    public bool NeedDisableUPASKReceivers { get; }
    public bool NeedDisconnectLineCTFromDZO { get; }
    public bool NeedMTZOshinovkaAtoB { get; }
    public bool NeedSwitchBusVTToReserve { get; }

    public static InstructionContext CreateSample() =>
        new InstructionContext(
            actionCode: "VL_OUT",
            isFieldClosing: true,
            isFieldClosingAllowed: true,
            hasDfz: true,
            dfzNormalEnabled: true,
            dfzEnabled: true,
            connectedToLineCt: true,
            connectedToLineVt: true,
            needSwitchVtToReserve: true,
            bothCbctReverseSide: false,
            hasDzl: true,
            dzlNormalEnabled: true,
            dzlEnabled: true,
            hasDz: true,
            dzNormalEnabled: true,
            dzEnabled: true,
            needDisableOapv: true,
            needDisableOapvVl: true,
            needDisableTapv: true,
            needDisableTapvInDevice: true,
            needDisableUpaskReceivers: true,
            needDisconnectLineCtFromDzo: true,
            needMtzoShinovkaAtoB: true,
            needSwitchBusVtToReserve: true);
}
