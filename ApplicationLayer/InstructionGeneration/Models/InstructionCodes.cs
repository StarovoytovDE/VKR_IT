namespace ApplicationLayer.InstructionGeneration.Models;

public static class InstructionCodes
{
    public const string FieldClosingNotAllowed = "FIELD_CLOSING_NOT_ALLOWED";

    public const string DfzHasFunction = "DFZ_HAS_FUNCTION";
    public const string DfzNormalEnabled = "DFZ_NORMAL_ENABLED";
    public const string DfzEnabled = "DFZ_ENABLED";
    public const string DfzConnectedToLineCt = "DFZ_CONNECTED_TO_LINE_CT";
    public const string DfzConnectedToLineVt = "DFZ_CONNECTED_TO_LINE_VT";
    public const string DfzSwitchVtToReserve = "DFZ_SWITCH_VT_TO_RESERVE";

    public const string DzlHasFunction = "DZL_HAS_FUNCTION";
    public const string DzlNormalEnabled = "DZL_NORMAL_ENABLED";
    public const string DzlEnabled = "DZL_ENABLED";

    public const string DzHasFunction = "DZ_HAS_FUNCTION";
    public const string DzNormalEnabled = "DZ_NORMAL_ENABLED";
    public const string DzEnabled = "DZ_ENABLED";
    public const string DzConnectedToLineVt = "DZ_CONNECTED_TO_LINE_VT";
    public const string DzSwitchVtToReserve = "DZ_SWITCH_VT_TO_RESERVE";

    public const string OapvDisable = "OAPV_DISABLE";
    public const string TapvDisable = "TAPV_DISABLE";
    public const string UpaskReceiversDisable = "UPASK_RECEIVERS_DISABLE";
    public const string SwitchLineVtToReserve = "SWITCH_LINE_VT_TO_RESERVE";
    public const string DisconnectLineCtFromDzo = "DISCONNECT_LINE_CT_FROM_DZO";
    public const string MtzoShinovkaAtoB = "MTZO_SHINOVKA_A_TO_B";
    public const string SwitchBusVtToReserve = "SWITCH_BUS_VT_TO_RESERVE";
}
