using ApplicationLayer.InstructionGeneration.DeviceParams;
using ApplicationLayer.InstructionGeneration.Models;
using ApplicationLayer.InstructionGeneration.Requests;
using Domain.ReferenceData;

namespace ApplicationLayer.InstructionGeneration.Criteria;

/// <summary>
/// Сборщик критериев операций по линии.
/// </summary>
public sealed class LineOperationCriteriaBuilder
{
    public LineOperationCriteria Build(
        LineOperationRequest request,
        int deviceObjectId,
        DeviceParamsSnapshot snapshot)
    {
        var criteria = new LineOperationCriteria(
            request.LineCode,
            request.Side,
            deviceObjectId,
            request.ActionCode)
        {
            DeviceName = snapshot.DeviceName,

            CtPlace = snapshot.CtPlace.Place ?? string.Empty,
            CtPlaceCode = snapshot.CtPlace.PlaceCode ?? string.Empty,

            DeviceConnectedToLineCT =
                snapshot.CtPlace.PlaceCode == PlaceCodes.Ct.LineBeforeLr ||
                snapshot.CtPlace.PlaceCode == PlaceCodes.Ct.LineAfterLr,

            VtSwitchTrue = snapshot.VtSwitchTrue,

            MainVtPlace = snapshot.Vts.Main.Place ?? string.Empty,
            MainVtPlaceCode = snapshot.Vts.Main.PlaceCode ?? string.Empty,

            ReserveVtPlace = snapshot.Vts.Reserve.Place ?? string.Empty,
            ReserveVtPlaceCode = snapshot.Vts.Reserve.PlaceCode ?? string.Empty,

            HasDFZ = snapshot.Dfz.Has,
            HasDZL = snapshot.Dzl.Has,
            HasDZ = snapshot.Dz.Has,
            HasOAPV = snapshot.Oapv.State.Has,
            HasTAPV = snapshot.Tapv.Has,

            IsFieldClosingAllowed = snapshot.IsFieldClosingAllowed,
            NeedDisableUpaskReceivers = snapshot.NeedDisableUpaskReceivers,
            NeedDisconnectLineCTFromDZO = snapshot.NeedDisconnectLineCTFromDzo,
            NeedMtzoShinovkaAtoB = snapshot.NeedMtzoShinovkaAtoB
        };

        return criteria;
    }
}
