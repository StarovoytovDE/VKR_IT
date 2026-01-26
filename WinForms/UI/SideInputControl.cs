using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WinForms.Services;

namespace WinForms.UI
{
    /// <summary>
    /// UserControl ввода для стороны линии:
    /// - заголовок (ПС),
    /// - выбор действия,
    /// - список устройств с чекбоксами ввода диспетчера.
    /// </summary>
    public partial class SideInputControl : UserControl
    {
        private readonly List<DeviceInputRow> _rows = new();

        /// <summary>
        /// Создаёт контрол стороны.
        /// </summary>
        public SideInputControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Устанавливает заголовок (ПС) для стороны.
        /// </summary>
        public void SetHeader(string substationName)
        {
            lblSubstation.Text = string.IsNullOrWhiteSpace(substationName)
                ? "Подстанция (не задано)"
                : substationName;
        }

        /// <summary>
        /// Устанавливает список доступных действий (фиксированный список).
        /// </summary>
        public void SetActions(IReadOnlyList<UiActionItem> actions)
        {
            comboAction.DataSource = actions.ToList();
            comboAction.DisplayMember = nameof(UiActionItem.DisplayName);
            comboAction.ValueMember = nameof(UiActionItem.Code);

            if (actions.Count > 0)
                comboAction.SelectedIndex = 0;
        }

        /// <summary>
        /// Устанавливает устройства стороны и перерисовывает строки ввода.
        /// </summary>
        public void SetDevices(IReadOnlyList<LineSideDeviceItem> devices)
        {
            flowDevices.SuspendLayout();
            try
            {
                flowDevices.Controls.Clear();
                _rows.Clear();

                foreach (var d in devices)
                {
                    var row = new DeviceInputRow(d);
                    _rows.Add(row);
                    flowDevices.Controls.Add(row);
                }
            }
            finally
            {
                flowDevices.ResumeLayout();
            }
        }

        /// <summary>
        /// Сбрасывает ввод (чекбоксы и выбранное действие).
        /// </summary>
        public void ResetInputs()
        {
            if (comboAction.Items.Count > 0)
                comboAction.SelectedIndex = 0;

            foreach (var r in _rows)
                r.Reset();
        }

        /// <summary>
        /// Собирает ввод диспетчера по текущей стороне.
        /// </summary>
        public SideDispatcherInput BuildDispatcherInput(UiSide side)
        {
            var action = comboAction.SelectedItem as UiActionItem
                         ?? new UiActionItem(UiActionCode.LineWithdrawalWithFieldClosing,
                             UiActionNames.GetDisplayName(UiActionCode.LineWithdrawalWithFieldClosing));

            var perDevice = _rows.Select(r => r.BuildDeviceInput()).ToList();
            return new SideDispatcherInput(side, action.Code, perDevice);
        }

        /// <summary>
        /// Панель ввода на одно устройство: имя + чекбоксы.
        /// </summary>
        private sealed class DeviceInputRow : Panel
        {
            private readonly Label _labelDevice;
            private readonly CheckBox _cbDfz;
            private readonly CheckBox _cbDzl;
            private readonly CheckBox _cbDz;

            private readonly LineSideDeviceItem _device;

            public DeviceInputRow(LineSideDeviceItem device)
            {
                _device = device ?? throw new ArgumentNullException(nameof(device));

                Height = 68;
                Dock = DockStyle.Top;
                Padding = new Padding(6);
                BorderStyle = BorderStyle.FixedSingle;

                _labelDevice = new Label
                {
                    AutoSize = true,
                    Text = device.DeviceName,
                    Left = 6,
                    Top = 8,
                    Font = new Font(SystemFonts.DefaultFont, FontStyle.Bold)
                };

                _cbDfz = new CheckBox
                {
                    Text = "ДФЗ введена",
                    Left = 10,
                    Top = 36,
                    AutoSize = true
                };

                _cbDzl = new CheckBox
                {
                    Text = "ДЗЛ введена",
                    Left = 150,
                    Top = 36,
                    AutoSize = true
                };

                _cbDz = new CheckBox
                {
                    Text = "ДЗ введена",
                    Left = 290,
                    Top = 36,
                    AutoSize = true
                };

                Controls.Add(_labelDevice);
                Controls.Add(_cbDfz);
                Controls.Add(_cbDzl);
                Controls.Add(_cbDz);
            }

            public void Reset()
            {
                _cbDfz.Checked = false;
                _cbDzl.Checked = false;
                _cbDz.Checked = false;
            }

            public DeviceDispatcherInput BuildDeviceInput()
            {
                return new DeviceDispatcherInput(
                    _device.DeviceId,
                    _device.DeviceName,
                    dfzEnabled: _cbDfz.Checked,
                    dzlEnabled: _cbDzl.Checked,
                    dzEnabled: _cbDz.Checked);
            }
        }
    }
}
