using System.Collections.Generic;
using System.Threading.Tasks;

namespace WinForms.Services
{
    /// <summary>
    /// Сторона линии для UI.
    /// </summary>
    public enum UiSide
    {
        /// <summary>Сторона A.</summary>
        A,

        /// <summary>Сторона B.</summary>
        B
    }

    /// <summary>
    /// Код действия, выбираемого диспетчером (фиксированный список).
    /// </summary>
    public enum UiActionCode
    {
        /// <summary>Вывод ВЛ с замыканием поля.</summary>
        LineWithdrawalWithFieldClosing,

        /// <summary>Вывод ВЛ без замыкания поля.</summary>
        LineWithdrawalWithoutFieldClosing,

        /// <summary>Перевод цепей напряжения на резервный шинный ТН.</summary>
        LineWithdrawalWithBusSideDisconnector,

        /// <summary>Односторонний вывод (пример).</summary>
        LineSingleSideWithdrawal
    }

    /// <summary>
    /// Отображаемые названия действий.
    /// </summary>
    public static class UiActionNames
    {
        /// <summary>
        /// Возвращает человекочитаемое название действия.
        /// </summary>
        public static string GetDisplayName(UiActionCode code)
        {
            return code switch
            {
                UiActionCode.LineWithdrawalWithFieldClosing => "Вывод ВЛ с замыканием поля",
                UiActionCode.LineWithdrawalWithoutFieldClosing => "Вывод ВЛ без замыкания поля",
                UiActionCode.LineWithdrawalWithBusSideDisconnector => "Перевод цепей напряжения на резервный шинный ТН",
                UiActionCode.LineSingleSideWithdrawal => "Односторонний вывод",
                _ => code.ToString()
            };
        }
    }

    /// <summary>
    /// Элемент действия для комбобокса.
    /// </summary>
    public sealed record UiActionItem(UiActionCode Code, string DisplayName);

    /// <summary>
    /// Элемент списка линий для комбобокса.
    /// </summary>
    public sealed record LineListItem(int LineId, string DisplayName);

    /// <summary>
    /// Устройство на стороне линии (минимум для UI).
    /// </summary>
    public sealed record LineSideDeviceItem(int DeviceId, string DeviceName);

    /// <summary>
    /// Контекст линии для UI: сторона A и сторона B.
    /// </summary>
    public sealed record LineUiContext(LineSideContext SideA, LineSideContext SideB);

    /// <summary>
    /// Контекст одной стороны линии.
    /// </summary>
    public sealed record LineSideContext(string SubstationName, IReadOnlyList<LineSideDeviceItem> Devices);

    /// <summary>
    /// Ввод диспетчера для одного устройства (переменные параметры).
    /// </summary>
    public sealed record DeviceDispatcherInput(
        int DeviceId,
        string DeviceName,
        bool dfzEnabled,
        bool dzlEnabled,
        bool dzEnabled);

    /// <summary>
    /// Ввод диспетчера по стороне: выбранное действие + набор устройств.
    /// </summary>
    public sealed record SideDispatcherInput(
        UiSide Side,
        UiActionCode Action,
        IReadOnlyList<DeviceDispatcherInput> Devices);

    /// <summary>
    /// Команда на формирование указаний по выбранной линии.
    /// </summary>
    public sealed record GenerateInstructionsCommand(
        int LineId,
        string LineDisplayName,
        SideDispatcherInput SideA,
        SideDispatcherInput SideB);

    /// <summary>
    /// Результат формирования указаний для одного устройства.
    /// </summary>
    public sealed record DeviceInstructionResult(
        UiSide Side,
        int DeviceId,
        string DeviceName,
        IReadOnlyList<string> Instructions);

    /// <summary>
    /// Сервис UI-данных: линии/стороны/устройства (постоянные данные из БД).
    /// </summary>
    public interface ILineUiDataService
    {
        /// <summary>
        /// Возвращает список линий для выбора.
        /// </summary>
        Task<IReadOnlyList<LineListItem>> GetLinesAsync();

        /// <summary>
        /// Возвращает контекст выбранной линии: названия ПС и устройства по сторонам.
        /// </summary>
        Task<LineUiContext> GetLineContextAsync(int lineId);
    }

    /// <summary>
    /// Сервис формирования указаний (вызов вашего ApplicationLayer генератора).
    /// </summary>
    public interface IInstructionUiService
    {
        /// <summary>
        /// Формирует указания по команде UI.
        /// </summary>
        Task<IReadOnlyList<DeviceInstructionResult>> GenerateAsync(GenerateInstructionsCommand command);
    }
}
