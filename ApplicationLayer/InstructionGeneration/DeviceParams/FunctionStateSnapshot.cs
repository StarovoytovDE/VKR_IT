namespace ApplicationLayer.InstructionGeneration.DeviceParams;

/// <summary>
/// Состояние функции: наличие (паспорт) и текущее состояние в БД (если используется).
/// В генерации «введено/не введено» обычно задаёт диспетчер через LineOperationRequest,
/// но поле State оставляем для диагностики/проверок и будущих сценариев.
/// </summary>
public sealed class FunctionStateSnapshot
{
    /// <summary>Есть ли функция в устройстве (паспортный признак).</summary>
    public bool Has { get; init; }

    /// <summary>Состояние в БД (введена/не введена), если читаем из БД.</summary>
    public bool State { get; init; }
}
