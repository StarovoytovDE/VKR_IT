namespace Domain.ReferenceData;

/// <summary>
/// Справочник кодов мест подключения ТН/ТТ.
/// Используется в алгоритмах (PlaceCode), чтобы не сравнивать русские строки.
/// </summary>
public static class PlaceCodes
{
    /// <summary>
    /// Коды мест подключения ТН (VT).
    /// </summary>
    public static class Vt
    {
        /// <summary>Линейный ТН.</summary>
        public const string Line = "VT_LINE";

        /// <summary>Шинный ТН.</summary>
        public const string Bus = "VT_BUS";

        /// <summary>ТН ошиновки.</summary>
        public const string Busbar = "VT_BUSBAR";
    }

    /// <summary>
    /// Коды мест подключения ТТ (CT).
    /// </summary>
    public static class Ct
    {
        /// <summary>Линейный ТТ до ЛР.</summary>
        public const string LineBeforeLr = "CT_LINE_BEFORE_LR";

        /// <summary>Линейный ТТ после ЛР.</summary>
        public const string LineAfterLr = "CT_LINE_AFTER_LR";

        /// <summary>Сумма токов выключателей линии.</summary>
        public const string SumBreakers = "CT_SUM_BREAKERS";
    }
}
