using System;
using ApplicationLayer.InstructionGeneration.DeviceParams;
using ApplicationLayer.InstructionGeneration.Models;
using ApplicationLayer.InstructionGeneration.Requests;

namespace ApplicationLayer.InstructionGeneration.Criteria;

/// <summary>
/// Сборщик критериев операций (LineOperationCriteria) из:
/// - «снимка» параметров устройства (паспорт/конфигурация, технолог/БД)
/// - оперативного запроса диспетчера (UI)
/// </summary>
public sealed class LineOperationCriteriaBuilder
{
    private const string LineVtPlace = "Линейный ТН";

    private const string LineCtBeforeLrPlace = "Линейный ТТ до ЛР";
    private const string LineCtAfterLrPlace = "Линейный ТТ после ЛР";
    private const string SumOfBreakerCurrentsPlace = "Сумма токов выключателей линии";

    /// <summary>
    /// Собирает критерии для генерации указаний по выбранному действию и стороне линии.
    /// </summary>
    /// <param name="request">Оперативный запрос диспетчера (UI).</param>
    /// <param name="deviceObjectId">Идентификатор объекта устройства (как в LineOperationCriteria).</param>
    /// <param name="snapshot">Снимок параметров устройства (БД/технолог).</param>
    public LineOperationCriteria Build(
        LineOperationRequest request,
        int deviceObjectId,
        DeviceParamsSnapshot snapshot)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(snapshot);
        ArgumentException.ThrowIfNullOrWhiteSpace(request.LineCode);

        var criteria = new LineOperationCriteria(
            lineCode: request.LineCode,
            side: request.Side,
            deviceObjectId: deviceObjectId,
            actionCode: request.ActionCode)
        {
            DeviceName = snapshot.DeviceName
        };

        // 1) Has* = паспорт (технолог)
        criteria = criteria with
        {
            HasDFZ = snapshot.Dfz.Has,
            HasDZL = snapshot.Dzl.Has,
            HasDZ = snapshot.Dz.Has,
            HasOAPV = snapshot.Oapv.State.Has,
            HasTAPV = snapshot.Tapv.Has
        };

        // 2) Enabled = оперативно (диспетчер), но строго с защитой "не может быть включено, если нет"
        criteria = criteria with
        {
            DFZEnabled = snapshot.Dfz.Has && request.FunctionStates.DfzEnabled,
            DZLEnabled = snapshot.Dzl.Has && request.FunctionStates.DzlEnabled,
            DZEnabled = snapshot.Dz.Has && request.FunctionStates.DzEnabled,
            OAPVEnabled = snapshot.Oapv.State.Has && request.FunctionStates.OapvEnabled,
            TAPVEnabled = snapshot.Tapv.Has && request.FunctionStates.TapvEnabled
        };

        // 3) Технологические флаги сценария/ограничений (пока reader даёт дефолты, в ConsoleTest можно переопределять)
        criteria = criteria with
        {
            IsFieldClosingAllowed = snapshot.IsFieldClosingAllowed,
            NeedDisableUpaskReceivers = snapshot.NeedDisableUpaskReceivers,
            NeedDisconnectLineCTFromDZO = snapshot.NeedDisconnectLineCTFromDzo,
            NeedMtzoShinovkaAtoB = snapshot.NeedMtzoShinovkaAtoB
        };

        // 4) CT-флаги (новая модель: CtPlace.Place из утверждённого словаря)
        criteria = FillCtFlags(snapshot, criteria);

        // 5) VT-флаги (новая модель: 2 VT у устройства + Place)
        criteria = FillVoltageFlags(snapshot, criteria);

        return criteria;
    }

    /// <summary>
    /// Заполняет флаги, связанные с подключением к линейному ТТ, по словарю значений CtPlace.Place:
    /// - "Линейный ТТ до ЛР" и "Линейный ТТ после ЛР" считаются линейными ТТ;
    /// - "Сумма токов выключателей линии" считается НЕ линейным ТТ.
    /// </summary>
    private static LineOperationCriteria FillCtFlags(DeviceParamsSnapshot snapshot, LineOperationCriteria criteria)
    {
        var ctPlace = snapshot.CtPlace.Place ?? string.Empty;

        var connectedToLineCt =
            string.Equals(ctPlace, LineCtBeforeLrPlace, StringComparison.Ordinal) ||
            string.Equals(ctPlace, LineCtAfterLrPlace, StringComparison.Ordinal);

        // Явно фиксируем: "Сумма токов выключателей линии" = НЕ линейный ТТ
        // (connectedToLineCt останется false, это и требуется).
        // Если встретится незнакомое значение — считаем НЕ линейным (false),
        // чтобы не ошибиться в сторону «да».
        _ = string.Equals(ctPlace, SumOfBreakerCurrentsPlace, StringComparison.Ordinal);

        return criteria with
        {
            DeviceConnectedToLineCT = connectedToLineCt
        };
    }

    /// <summary>
    /// Заполняет флаги, связанные с подключением к линейному ТН и переводом цепей напряжения.
    /// Правило: линейный ТН определяется строго по Place == "Линейный ТН".
    /// </summary>
    private static LineOperationCriteria FillVoltageFlags(DeviceParamsSnapshot snapshot, LineOperationCriteria criteria)
    {
        var mainPlace = snapshot.Vts.Main.Place ?? string.Empty;
        var reservePlace = snapshot.Vts.Reserve.Place ?? string.Empty;

        var mainIsLineVt = string.Equals(mainPlace, LineVtPlace, StringComparison.Ordinal);
        var reserveIsLineVt = string.Equals(reservePlace, LineVtPlace, StringComparison.Ordinal);

        // Подключение функций к линейному ТН:
        // пока считаем, что если основной VT устройства = линейный, то функции, использующие линейный ТН, "подключены".
        // (При необходимости потом разделим это на отдельные паспортные привязки по функциям.)
        var dfzConnectedToLineVt = mainIsLineVt;
        var dzConnectedToLineVt = mainIsLineVt;

        // Нужен ли перевод на резерв:
        var needSwitchVtToReserve =
            snapshot.VtSwitchTrue &&
            !string.Equals(mainPlace, reservePlace, StringComparison.Ordinal);

        var needSwitchFromLineVtToReserve =
            needSwitchVtToReserve &&
            mainIsLineVt &&
            !reserveIsLineVt;

        // Пока не формализуем вычисление "шинный->резерв" по словарю Place,
        // потому что у тебя три типа ("Линейный ТН", "Шинный ТН", "ТН ошиновки"),
        // а варианты переключений для "шинного" и "ошиновки" будем уточнять отдельно.
        var needSwitchFromBusVtToReserve = false;

        return criteria with
        {
            DFZConnectedToLineVT = dfzConnectedToLineVt,
            DZConnectedToLineVT = dzConnectedToLineVt,

            NeedSwitchVTToReserve = needSwitchVtToReserve,
            NeedSwitchFromLineVTToReserve = needSwitchFromLineVtToReserve,
            NeedSwitchFromBusVTToReserve = needSwitchFromBusVtToReserve
        };
    }
}
