using ApplicationLayer.InstructionGeneration.Criteria;
using ApplicationLayer.InstructionGeneration.DeviceParams;
using ApplicationLayer.InstructionGeneration.Models;
using ApplicationLayer.InstructionGeneration.Operations;
using ApplicationLayer.InstructionGeneration.Requests;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WinForms.Services
{
    /// <summary>
    /// Реальная реализация IInstructionUiService:
    /// - читает агрегированный снимок параметров устройства (DeviceParamsSnapshot) через IDeviceParamsReader,
    /// - строит критерии (LineOperationCriteria) через LineOperationCriteriaBuilder,
    /// - прогоняет операции через InstructionGenerator,
    /// - возвращает список сформированных указаний для отображения в UI.
    /// </summary>
    public sealed class EfCoreInstructionUiService : IInstructionUiService
    {
        private readonly IDeviceParamsReader _deviceParamsReader;
        private readonly LineOperationCriteriaBuilder _criteriaBuilder;
        private readonly InstructionGenerator _generator;

        /// <summary>
        /// Создаёт сервис генерации указаний.
        /// </summary>
        public EfCoreInstructionUiService(
            IDeviceParamsReader deviceParamsReader,
            LineOperationCriteriaBuilder criteriaBuilder,
            InstructionGenerator generator)
        {
            _deviceParamsReader = deviceParamsReader ?? throw new ArgumentNullException(nameof(deviceParamsReader));
            _criteriaBuilder = criteriaBuilder ?? throw new ArgumentNullException(nameof(criteriaBuilder));
            _generator = generator ?? throw new ArgumentNullException(nameof(generator));
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<DeviceInstructionResult>> GenerateAsync(GenerateInstructionsCommand command)
        {
            ArgumentNullException.ThrowIfNull(command);

            var results = new List<DeviceInstructionResult>();

            await AppendForSideAsync(results, command, command.SideA, CancellationToken.None);
            await AppendForSideAsync(results, command, command.SideB, CancellationToken.None);

            return results;
        }

        private async Task AppendForSideAsync(
            List<DeviceInstructionResult> results,
            GenerateInstructionsCommand command,
            SideDispatcherInput sideInput,
            CancellationToken ct)
        {
            foreach (var device in sideInput.Devices)
            {
                // 1) Snapshot из БД.
                var snapshot = await _deviceParamsReader.ReadAsync(device.DeviceId, ct);

                // 2) Запрос диспетчера -> request.
                var request = new LineOperationRequest
                {
                    LineCode = command.LineDisplayName,
                    Side = MapSide(sideInput.Side),
                    ActionCode = MapAction(sideInput.Action),
                    FunctionStates = new FunctionStatesRequest
                    {
                        DfzEnabled = device.dfzEnabled,
                        DzlEnabled = device.dzlEnabled,
                        DzEnabled = device.dzEnabled,

                        // В текущем UI нет ввода по ОАПВ/ТАПВ — считаем "не введены".
                        OapvEnabled = false,
                        TapvEnabled = false
                    }
                };

                // 3) Criteria.
                // deviceObjectId в текущей модели не существует (device не является object),
                // поэтому используем DeviceId как идентификатор в критериях/логах.
                var criteria = _criteriaBuilder.Build(request, deviceObjectId: device.DeviceId, snapshot);

                // 4) Генерация.
                var instructions = _generator.Generate(criteria);

                results.Add(new DeviceInstructionResult(
                    sideInput.Side,
                    device.DeviceId,
                    snapshot.DeviceName,
                    instructions));
            }
        }

        private static SideOfLine MapSide(UiSide side) =>
            side switch
            {
                UiSide.A => SideOfLine.A,
                UiSide.B => SideOfLine.B,
                _ => throw new ArgumentOutOfRangeException(nameof(side), side, "Неизвестная сторона линии.")
            };

        private static ActionCode MapAction(UiActionCode action) =>
            action switch
            {
                UiActionCode.LineWithdrawalWithFieldClosing => ActionCode.LineWithdrawalWithFieldClosing,
                UiActionCode.LineWithdrawalWithoutFieldClosing => ActionCode.LineWithdrawalWithoutFieldClosing,
                UiActionCode.LineWithdrawalWithBusSideDisconnector => ActionCode.LineWithdrawalWithBusSideDisconnector,
                UiActionCode.LineSingleSideWithdrawal => ActionCode.LineSingleSideWithdrawal,
                _ => throw new ArgumentOutOfRangeException(nameof(action), action, "Неизвестный код действия.")
            };
    }
}
