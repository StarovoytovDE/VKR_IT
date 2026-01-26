namespace WinForms.UI
{
    partial class SideInputControl
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TableLayoutPanel tableRoot;
        private System.Windows.Forms.Label lblSubstation;
        private System.Windows.Forms.Panel panelAction;
        private System.Windows.Forms.Label lblAction;
        private System.Windows.Forms.ComboBox comboAction;
        private System.Windows.Forms.FlowLayoutPanel flowDevices;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.tableRoot = new System.Windows.Forms.TableLayoutPanel();
            this.lblSubstation = new System.Windows.Forms.Label();
            this.panelAction = new System.Windows.Forms.Panel();
            this.lblAction = new System.Windows.Forms.Label();
            this.comboAction = new System.Windows.Forms.ComboBox();
            this.flowDevices = new System.Windows.Forms.FlowLayoutPanel();

            this.tableRoot.SuspendLayout();
            this.panelAction.SuspendLayout();
            this.SuspendLayout();

            // tableRoot
            this.tableRoot.ColumnCount = 1;
            this.tableRoot.RowCount = 3;
            this.tableRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tableRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            this.tableRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));

            // lblSubstation
            this.lblSubstation.AutoSize = true;
            this.lblSubstation.Text = "Подстанция";
            this.lblSubstation.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSubstation.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblSubstation.Padding = new System.Windows.Forms.Padding(0, 0, 0, 6);

            // panelAction
            this.panelAction.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelAction.Height = 34;

            // lblAction
            this.lblAction.AutoSize = true;
            this.lblAction.Text = "Действие:";
            this.lblAction.Left = 0;
            this.lblAction.Top = 8;

            // comboAction
            this.comboAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboAction.Left = 80;
            this.comboAction.Top = 4;
            this.comboAction.Width = 320;
            this.comboAction.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));

            this.panelAction.Controls.Add(this.lblAction);
            this.panelAction.Controls.Add(this.comboAction);

            // flowDevices
            this.flowDevices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowDevices.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowDevices.WrapContents = false;
            this.flowDevices.AutoScroll = true;
            this.flowDevices.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);

            // add to tableRoot
            this.tableRoot.Controls.Add(this.lblSubstation, 0, 0);
            this.tableRoot.Controls.Add(this.panelAction, 0, 1);
            this.tableRoot.Controls.Add(this.flowDevices, 0, 2);

            // SideInputControl
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableRoot);
            this.Name = "SideInputControl";

            this.tableRoot.ResumeLayout(false);
            this.tableRoot.PerformLayout();
            this.panelAction.ResumeLayout(false);
            this.panelAction.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}
