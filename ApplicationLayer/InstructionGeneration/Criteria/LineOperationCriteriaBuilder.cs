using System;
using ApplicationLayer.InstructionGeneration.DeviceParams;
using ApplicationLayer.InstructionGeneration.Models;
using ApplicationLayer.InstructionGeneration.Requests;
using Domain.ReferenceData;

namespace ApplicationLayer.InstructionGeneration.Criteria;

/// <summary>
/// Сборщик критериев операций (LineOperationCriteria) из:
/// - снимка параметров устройства (паспорт/технолог/БД)
/// - оперативного запроса диспетчера (UI)
/// </summary>
public sealed class LineOperationCriteriaBuilder
{
    /// <summary>
    /// Собирает критерии для генерации указаний по выбранному действию и стороне линии.
    /// </summary>
    public LineOperationCriteria Build(LineOperationRequest request, int deviceObjectId, DeviceParamsSnapshot snapshot)
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

        // 1) Has* = паспорт (технолог/БД)
        criteria = criteria with
        {
            HasDFZ = snapshot.Dfz.Has,
            HasDZL = snapshot.Dzl.Has,
            HasDZ = snapshot.Dz.Has,
            HasOAPV = snapshot.Oapv.State.Has,
            HasTAPV = snapshot.Tapv.Has
        };

        // 2) Enabled = оперативно (диспетчер), но «не может быть включено, если функции нет»
        criteria = criteria with
        {
            DFZEnabled = snapshot.Dfz.Has && request.FunctionStates.DfzEnabled,
            DZLEnabled = snapshot.Dzl.Has && request.FunctionStates.DzlEnabled,
            DZEnabled = snapshot.Dz.Has && request.FunctionStates.DzEnabled,
            OAPVEnabled = snapshot.Oapv.State.Has && request.FunctionStates.OapvEnabled,
            TAPVEnabled = snapshot.Tapv.Has && request.FunctionStates.TapvEnabled
        };

        // 3) Технологические/паспортные флаги сценариев
        criteria = criteria with
        {
            IsFieldClosingAllowed = snapshot.IsFieldClosingAllowed,
            NeedDisableUpaskReceivers = snapshot.NeedDisableUpaskReceivers,
            NeedDisconnectLineCTFromDZO = snapshot.NeedDisconnectLineCTFromDzo,
            NeedMtzoShinovkaAtoB = snapshot.NeedMtzoShinovkaAtoB
        };

        // 4) CT
        criteria = FillCt(snapshot, criteria);

        // 5) VT
        criteria = FillVt(snapshot, criteria);

        return criteria;
    }

    /// <summary>
    /// Заполняет параметры CT и вычисляет DeviceConnectedToLineCT по CtPlaceCode.
    /// </summary>
    private static LineOperationCriteria FillCt(DeviceParamsSnapshot snapshot, LineOperationCriteria criteria)
    {
        var ctPlace = snapshot.CtPlace.Place ?? string.Empty;
        var ctCode = snapshot.CtPlace.PlaceCode ?? string.Empty;

        var connectedToLineCt =
            string.Equals(ctCode, PlaceCodes.Ct.LineBeforeLr, StringComparison.Ordinal) ||
            string.Equals(ctCode, PlaceCodes.Ct.LineAfterLr, StringComparison.Ordinal);

        return criteria with
        {
            CtPlace = ctPlace,
            CtPlaceCode = ctCode,
            DeviceConnectedToLineCT = connectedToLineCt
        };
    }

    /// <summary>
    /// Заполняет параметры VT по Vt.PlaceCode.
    /// </summary>
    private static LineOperationCriteria FillVt(DeviceParamsSnapshot snapshot, LineOperationCriteria criteria)
    {
        var mainPlace = snapshot.Vts.Main.Place ?? string.Empty;
        var mainCode = snapshot.Vts.Main.PlaceCode ?? string.Empty;

        var reservePlace = snapshot.Vts.Reserve.Place ?? string.Empty;
        var reserveCode = snapshot.Vts.Reserve.PlaceCode ?? string.Empty;

        return criteria with
        {
            VtSwitchTrue = snapshot.VtSwitchTrue,

            MainVtPlace = mainPlace,
            MainVtPlaceCode = mainCode,

            ReserveVtPlace = reservePlace,
            ReserveVtPlaceCode = reserveCode
        };
    }
}
