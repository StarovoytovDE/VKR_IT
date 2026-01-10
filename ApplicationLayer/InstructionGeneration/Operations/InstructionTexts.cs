namespace ApplicationLayer.InstructionGeneration.Operations;

/// <summary>
/// Единые тексты формируемых указаний.
/// </summary>
public static class InstructionTexts
{
    /// <summary>Указание: вывести устройство (когда функция единственная в устройстве).</summary>
    public const string WithdrawDevice = "Вывести устройство";

    /// <summary>
    /// Формирует указание: вывести конкретную функцию (например, ДФЗ/ДЗЛ).
    /// </summary>
    public static string WithdrawFunction(string functionName)
        => $"Вывести функцию {functionName}";

    /// <summary>
    /// Указание: следовать регламенту при переводе цепей напряжения.
    /// </summary>
    public const string FollowVoltageTransferInstructions =
        "Следовать указаниям при переводе цепей напряжения устройства РЗ и СА";
}
