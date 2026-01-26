using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinForms.Services;

namespace WinForms.UI
{
    /// <summary>
    /// Форма "Формирование указаний".
    /// </summary>
    public partial class InstructionGenerationForm : Form
    {
        private readonly ILineUiDataService _lineUiDataService;
        private readonly IInstructionUiService _instructionUiService;

        /// <summary>
        /// Создаёт форму.
        /// </summary>
        public InstructionGenerationForm(
            ILineUiDataService lineUiDataService,
            IInstructionUiService instructionUiService)
        {
            _lineUiDataService = lineUiDataService ?? throw new ArgumentNullException(nameof(lineUiDataService));
            _instructionUiService = instructionUiService ?? throw new ArgumentNullException(nameof(instructionUiService));

            InitializeComponent();
            InitializeActions();
            HookEvents();
        }

        private void InitializeActions()
        {
            var actions = Enum.GetValues(typeof(UiActionCode))
                .Cast<UiActionCode>()
                .Select(x => new UiActionItem(x, UiActionNames.GetDisplayName(x)))
                .ToList();

            sideAControl.SetActions(actions);
            sideBControl.SetActions(actions);
        }

        private void HookEvents()
        {
            Shown += async (_, __) => await LoadLinesAsync();
            comboLine.SelectedIndexChanged += async (_, __) => await OnLineChangedAsync();

            btnReset.Click += (_, __) => ResetAllInputs();
            btnGenerate.Click += async (_, __) => await GenerateAsync();
        }

        private async Task LoadLinesAsync()
        {
            comboLine.Enabled = false;
            try
            {
                var lines = await _lineUiDataService.GetLinesAsync();

                comboLine.DataSource = lines.ToList();
                comboLine.DisplayMember = nameof(LineListItem.DisplayName);
                comboLine.ValueMember = nameof(LineListItem.LineId);

                if (lines.Count > 0)
                {
                    comboLine.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Ошибка загрузки линий", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                comboLine.Enabled = true;
            }
        }

        private async Task OnLineChangedAsync()
        {
            if (comboLine.SelectedItem is not LineListItem selectedLine)
                return;

            try
            {
                var ctx = await _lineUiDataService.GetLineContextAsync(selectedLine.LineId);

                sideAControl.SetHeader(ctx.SideA.SubstationName);
                sideBControl.SetHeader(ctx.SideB.SubstationName);

                sideAControl.SetDevices(ctx.SideA.Devices);
                sideBControl.SetDevices(ctx.SideB.Devices);

                // При смене линии очищаем указания по сторонам и общий вывод.
                txtSideAInstructions.Clear();
                txtSideBInstructions.Clear();
                txtOutput.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Ошибка загрузки устройств", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetAllInputs()
        {
            sideAControl.ResetInputs();
            sideBControl.ResetInputs();

            txtSideAInstructions.Clear();
            txtSideBInstructions.Clear();
            txtOutput.Clear();
        }

        private async Task GenerateAsync()
        {
            if (comboLine.SelectedItem is not LineListItem selectedLine)
            {
                MessageBox.Show(this, "Выберите линию.", "Проверка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                btnGenerate.Enabled = false;

                txtSideAInstructions.Clear();
                txtSideBInstructions.Clear();
                txtOutput.Clear();

                // Ввод диспетчера по сторонам
                var sideAInput = sideAControl.BuildDispatcherInput(UiSide.A);
                var sideBInput = sideBControl.BuildDispatcherInput(UiSide.B);

                var cmd = new GenerateInstructionsCommand(
                    selectedLine.LineId,
                    selectedLine.DisplayName,
                    sideAInput,
                    sideBInput);

                // Генерация
                var results = await _instructionUiService.GenerateAsync(cmd);

                // Центральные боксы: указания по сторонам отдельно
                txtSideAInstructions.Text = FormatResultsBySide(results, UiSide.A);
                txtSideBInstructions.Text = FormatResultsBySide(results, UiSide.B);

                // Нижний общий вывод: всё вместе (удобно для копирования/отладки)
                txtOutput.Text = FormatResultsAll(results);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Ошибка формирования", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnGenerate.Enabled = true;
            }
        }

        private static string FormatResultsBySide(System.Collections.Generic.IReadOnlyList<DeviceInstructionResult> results, UiSide side)
        {
            var sb = new StringBuilder();

            var filtered = results
                .Where(x => x.Side == side)
                .OrderBy(x => x.DeviceName)
                .ToList();

            if (filtered.Count == 0)
            {
                sb.AppendLine("(нет данных по стороне)");
                return sb.ToString();
            }

            foreach (var r in filtered)
            {
                sb.AppendLine($"Указания для устройства — {r.DeviceName}");
                if (r.Instructions.Count == 0)
                {
                    sb.AppendLine("  (нет указаний)");
                }
                else
                {
                    foreach (var item in r.Instructions)
                        sb.AppendLine($" - {item}");
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        private static string FormatResultsAll(System.Collections.Generic.IReadOnlyList<DeviceInstructionResult> results)
        {
            var sb = new StringBuilder();

            foreach (var r in results.OrderBy(x => x.Side).ThenBy(x => x.DeviceName))
            {
                sb.AppendLine($"Указания для устройства ({r.Side}) — {r.DeviceName}");
                if (r.Instructions.Count == 0)
                {
                    sb.AppendLine("  (нет указаний)");
                }
                else
                {
                    foreach (var item in r.Instructions)
                        sb.AppendLine($" - {item}");
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
