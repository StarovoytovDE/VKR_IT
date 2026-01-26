using ApplicationLayer.InstructionGeneration.DeviceParams;
using ApplicationLayer.InstructionGeneration.Requests;
using Domain.Entities;
using Domain.ReferenceData;
using System;

namespace ApplicationLayer.InstructionGeneration.Criteria;

/// <summary>
/// Строит критерии генерации (LineOperationCriteria) из запроса диспетчера (LineOperationRequest)
/// и снимка параметров устройства (DeviceParamsSnapshot).
/// </summary>
public sealed class LineOperationCriteriaBuilder
{
    /// <summary>
    /// Формирует критерии генерации указаний для операции по линии.
    /// </summary>
    /// <param name="request">Запрос диспетчера (линия/сторона/код операции + оперативные состояния функций).</param>
    /// <param name="deviceObjectId">ObjectId устройства (в домене хранится как long, в критериях используется int).</param>
    /// <param name="snapshot">Снимок параметров устройства, считанный из БД.</param>
    /// <returns>Набор критериев для генератора указаний.</returns>
    /// <exception cref="ArgumentNullException">Если request или snapshot равны null.</exception>
    public LineOperationCriteria Build(LineOperationRequest request, int deviceObjectId, DeviceParamsSnapshot snapshot)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));
        if (snapshot is null) throw new ArgumentNullException(nameof(snapshot));

        // CT: вычисляем "линейный/нелинейный" по place_code.
        // PlaceCode может быть null, поэтому нормализуем в пустую строку.
        var ctPlaceCode = snapshot.CtPlace.PlaceCode ?? string.Empty;
        var deviceConnectedToLineCt = IsLineCt(ctPlaceCode);

        // ВАЖНО:
        // LineOperationCriteria сконструирован как тип с позиционным конструктором.
        // Поэтому сначала создаём базовый экземпляр через конструктор, затем заполняем init-only свойства через with.
        var criteria = new LineOperationCriteria(
            lineCode: request.LineCode,
            side: request.Side,
            deviceObjectId: deviceObjectId,
            actionCode: request.ActionCode);

        return criteria with
        {
            // --- Общие параметры устройства (из snapshot) ---
            CtPlace = snapshot.CtPlace.Place,
            CtPlaceCode = ctPlaceCode,
            DeviceConnectedToLineCT = deviceConnectedToLineCt,

            VtSwitchTrue = snapshot.VtSwitchTrue,

            MainVtPlace = snapshot.Vts.Main.Place,
            MainVtPlaceCode = snapshot.Vts.Main.PlaceCode,

            ReserveVtPlace = snapshot.Vts.Reserve.Place,
            ReserveVtPlaceCode = snapshot.Vts.Reserve.PlaceCode,

            // --- Паспорт (наличие функций) из БД ---
            HasDFZ = snapshot.Dfz.Has,
            HasDZL = snapshot.Dzl.Has,
            HasDZ = snapshot.Dz.Has,
            HasOAPV = snapshot.Oapv.State.Has,
            HasTAPV = snapshot.Tapv.Has,

            // --- Оперативное состояние (введено/не введено) задаёт диспетчер ---
            DFZEnabled = request.FunctionStates.DfzEnabled,
            DZLEnabled = request.FunctionStates.DzlEnabled,
            DZEnabled = request.FunctionStates.DzEnabled,
            OAPVEnabled = request.FunctionStates.OapvEnabled,
            TAPVEnabled = request.FunctionStates.TapvEnabled,

            // --- Технологические параметры (задаёт технолог; читаем из БД -> snapshot) ---
            IsFieldClosingAllowed = snapshot.IsFieldClosingAllowed,
            NeedDisableUpaskReceivers = snapshot.NeedDisableUpaskReceivers,

            // В snapshot: NeedDisconnectLineCTFromDzo (Dzo), в criteria: NeedDisconnectLineCTFromDZO (DZO).
            NeedDisconnectLineCTFromDZO = snapshot.NeedDisconnectLineCTFromDzo,
            CtRemainsEnergizedOnThisSide = snapshot.CtRemainsEnergizedOnThisSide,
        };
    }

    /// <summary>
    /// Определяет, является ли место установки ТТ линейным (истина для "до ЛР" и "после ЛР").
    /// </summary>
    /// <param name="ctPlaceCode">Код места установки ТТ (PlaceCode).</param>
    private static bool IsLineCt(string ctPlaceCode)
    {
        if (string.IsNullOrWhiteSpace(ctPlaceCode))
            return false;

        return string.Equals(ctPlaceCode, PlaceCodes.Ct.LineBeforeLr, StringComparison.Ordinal)
               || string.Equals(ctPlaceCode, PlaceCodes.Ct.LineAfterLr, StringComparison.Ordinal);
    }
}
