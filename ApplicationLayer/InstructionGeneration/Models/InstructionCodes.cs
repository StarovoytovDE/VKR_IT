namespace ApplicationLayer.InstructionGeneration.Models;

/// <summary>
/// Contains instruction code constants.
/// </summary>
public static class InstructionCodes
{
    /// <summary>
    /// Code for disabling the DFZ function.
    /// </summary>
    public const string DfzDisableFunction = "DFZ_DISABLE_FUNCTION";

    /// <summary>
    /// Code for disabling the DFZ device.
    /// </summary>
    public const string DfzDisableDevice = "DFZ_DISABLE_DEVICE";

    /// <summary>
    /// Code for disabling the DZ function.
    /// </summary>
    public const string DzDisableFunction = "DZ_DISABLE_FUNCTION";

    /// <summary>
    /// Code for disabling the DZ device.
    /// </summary>
    public const string DzDisableDevice = "DZ_DISABLE_DEVICE";

    /// <summary>
    /// Code for disabling the DZL device.
    /// </summary>
    public const string DzlDisableDevice = "DZL_DISABLE_DEVICE";

    /// <summary>
    /// Code for disabling the OAPV function.
    /// </summary>
    public const string OapvDisableFunction = "OAPV_DISABLE_FUNCTION";

    /// <summary>
    /// Code for disabling the TAPV function.
    /// </summary>
    public const string TapvDisableFunction = "TAPV_DISABLE_FUNCTION";

    /// <summary>
    /// Code for disabling UPASK receivers.
    /// </summary>
    public const string UpaskDisableReceivers = "UPASK_DISABLE_RECEIVERS";

    /// <summary>
    /// Code for switching line VT to reserve.
    /// </summary>
    public const string SwitchLineVtToReserve = "SWITCH_LINE_VT_TO_RESERVE";

    /// <summary>
    /// Code for switching bus VT to reserve.
    /// </summary>
    public const string SwitchBusVtToReserve = "SWITCH_BUS_VT_TO_RESERVE";

    /// <summary>
    /// Code for disconnecting line CT from DZO.
    /// </summary>
    public const string DisconnectLineCtFromDzo = "DISCONNECT_LINE_CT_FROM_DZO";

    /// <summary>
    /// Code for switching MTZO settings group.
    /// </summary>
    public const string MtzoShinovkaAToB = "MTZO_SHINOVKA_A_TO_B";

    /// <summary>
    /// Code for following voltage switch instructions.
    /// </summary>
    public const string FollowVoltageSwitchInstructions = "FOLLOW_VOLTAGE_SWITCH_INSTRUCTIONS";
}
