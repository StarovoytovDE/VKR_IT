using System.Threading;
using System.Threading.Tasks;

namespace ApplicationLayer.InstructionGeneration.DeviceParams;

/// <summary>
/// Читатель параметров устройства для формирования «снимка» (DeviceParamsSnapshot).
/// Реализации находятся в Infrastructure (например, EF Core).
/// </summary>
public interface IDeviceParamsReader
{
    /// <summary>
    /// Считывает полный набор параметров устройства, необходимых для формирования критериев операций.
    /// </summary>
    /// <param name="deviceId">Идентификатор устройства.</param>
    /// <param name="ct">Токен отмены.</param>
    Task<DeviceParamsSnapshot> ReadAsync(long deviceId, CancellationToken ct);
}
