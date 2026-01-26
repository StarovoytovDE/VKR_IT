using System;
using ApplicationLayer.InstructionGeneration.DeviceParams;
using ApplicationLayer.InstructionGeneration.Models;
using ApplicationLayer.InstructionGeneration.Requests;
using Domain.ReferenceData;

namespace ApplicationLayer.InstructionGeneration.Criteria;

/// <summary>
/// Сборщик критериев операций по линии.
/// Преобразует запрос диспетчера (LineOperationRequest) и snapshot устройства в LineOperationCriteria.
/// </summary>
public sealed class LineOperationCriteriaBuilder
{
    /// <summary>
    /// Строит критерии операций по линии на основании запроса диспетчера и параметров устройства.
    /// </summary>
    /// <param name="request">Запрос диспетчера (что делаем).</param>
    /// <param name="deviceObjectId">Идентификатор объекта устройства (ObjectId).</param>
    /// <param name="snapshot">Снимок параметров устройства.</param>
    /// <returns>Критерии для генератора указаний.</returns>
    public LineOperationCriteria Build(LineOperationRequest request, int deviceObjectId, DeviceParamsSnapshot snapshot)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(snapshot);

        var ctPlaceCode = snapshot.CtPlace.PlaceCode ?? string.Empty;

        var deviceConnectedToLineCt =
            string.Equals(ctPlaceCode, PlaceCodes.Ct.LineBeforeLr, StringComparison.Ordinal) ||
            string.Equals(ctPlaceCode, PlaceCodes.Ct.LineAfterLr, StringComparison.Ordinal);

        var criteria = new LineOperationCriteria(
            lineCode: request.LineCode,
            side: request.Side,
            deviceObjectId: deviceObjectId,
            actionCode: request.ActionCode)
        {
            DeviceName = snapshot.DeviceName,

            CtPlace = snapshot.CtPlace.Place ?? string.Empty,
            CtPlaceCode = ctPlaceCode,
            DeviceConnectedToLineCT = deviceConnectedToLineCt,

            VtSwitchTrue = snapshot.VtSwitchTrue,

            MainVtPlace = snapshot.Vts.Main.Place ?? string.Empty,
            MainVtPlaceCode = snapshot.Vts.Main.PlaceCode ?? string.Empty,

            ReserveVtPlace = snapshot.Vts.Reserve.Place ?? string.Empty,
            ReserveVtPlaceCode = snapshot.Vts.Reserve.PlaceCode ?? string.Empty,

            // ПАСПОРТ (наличие функций)
            HasDFZ = snapshot.Dfz.Has,
            HasDZL = snapshot.Dzl.Has,
            HasDZ = snapshot.Dz.Has,
            HasOAPV = snapshot.Oapv.State.Has,
            HasTAPV = snapshot.Tapv.Has,

            // ОПЕРАТИВНОЕ СОСТОЯНИЕ (введено/не введено) — только из запроса диспетчера
            DFZEnabled = request.FunctionStates.DfzEnabled,
            DZLEnabled = request.FunctionStates.DzlEnabled,
            DZEnabled = request.FunctionStates.DzEnabled,
            OAPVEnabled = request.FunctionStates.OapvEnabled,
            TAPVEnabled = request.FunctionStates.TapvEnabled,

            // Технологические флаги (device)
            IsFieldClosingAllowed = snapshot.IsFieldClosingAllowed,
            NeedDisableUpaskReceivers = snapshot.NeedDisableUpaskReceivers,
            NeedDisconnectLineCTFromDZO = snapshot.NeedDisconnectLineCTFromDzo,
            CtRemainsEnergizedOnThisSide = snapshot.CtRemainsEnergizedOnThisSide
        };

        return criteria;
    }
}
