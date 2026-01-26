namespace WinForms.UI
{
    partial class InstructionGenerationForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblLine;
        private System.Windows.Forms.ComboBox comboLine;

        private System.Windows.Forms.TableLayoutPanel tableMain;
        private System.Windows.Forms.GroupBox grpSideAInput;
        private System.Windows.Forms.GroupBox grpSideAInstructions;
        private System.Windows.Forms.GroupBox grpSideBInstructions;
        private System.Windows.Forms.GroupBox grpSideBInput;

        private WinForms.UI.SideInputControl sideAControl;
        private WinForms.UI.SideInputControl sideBControl;

        private System.Windows.Forms.RichTextBox txtSideAInstructions;
        private System.Windows.Forms.RichTextBox txtSideBInstructions;

        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.RichTextBox txtOutput;

        /// <summary>
        /// Освобождает ресурсы.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblLine = new System.Windows.Forms.Label();
            this.comboLine = new System.Windows.Forms.ComboBox();

            this.tableMain = new System.Windows.Forms.TableLayoutPanel();
            this.grpSideAInput = new System.Windows.Forms.GroupBox();
            this.grpSideAInstructions = new System.Windows.Forms.GroupBox();
            this.grpSideBInstructions = new System.Windows.Forms.GroupBox();
            this.grpSideBInput = new System.Windows.Forms.GroupBox();

            this.sideAControl = new WinForms.UI.SideInputControl();
            this.sideBControl = new WinForms.UI.SideInputControl();

            this.txtSideAInstructions = new System.Windows.Forms.RichTextBox();
            this.txtSideBInstructions = new System.Windows.Forms.RichTextBox();

            this.panelBottom = new System.Windows.Forms.Panel();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.txtOutput = new System.Windows.Forms.RichTextBox();

            this.panelTop.SuspendLayout();
            this.tableMain.SuspendLayout();
            this.grpSideAInput.SuspendLayout();
            this.grpSideAInstructions.SuspendLayout();
            this.grpSideBInstructions.SuspendLayout();
            this.grpSideBInput.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();

            // panelTop
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Height = 54;
            this.panelTop.Padding = new System.Windows.Forms.Padding(12, 10, 12, 10);
            this.panelTop.Controls.Add(this.lblLine);
            this.panelTop.Controls.Add(this.comboLine);

            // lblLine
            this.lblLine.AutoSize = true;
            this.lblLine.Location = new System.Drawing.Point(12, 16);
            this.lblLine.Text = "Линия:";

            // comboLine
            this.comboLine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboLine.Location = new System.Drawing.Point(70, 12);
            this.comboLine.Width = 820;
            this.comboLine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));

            // tableMain
            this.tableMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableMain.ColumnCount = 4;
            this.tableMain.RowCount = 1;
            this.tableMain.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.tableMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this.tableMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16F));
            this.tableMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16F));
            this.tableMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));

            // grpSideAInput
            this.grpSideAInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSideAInput.Text = "Сторона A (ввод)";
            this.grpSideAInput.Padding = new System.Windows.Forms.Padding(10);
            this.grpSideAInput.Controls.Add(this.sideAControl);

            // sideAControl
            this.sideAControl.Dock = System.Windows.Forms.DockStyle.Fill;

            // grpSideAInstructions
            this.grpSideAInstructions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSideAInstructions.Text = "Указания стороны A";
            this.grpSideAInstructions.Padding = new System.Windows.Forms.Padding(10);
            this.grpSideAInstructions.Controls.Add(this.txtSideAInstructions);

            // txtSideAInstructions
            this.txtSideAInstructions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSideAInstructions.ReadOnly = true;
            this.txtSideAInstructions.WordWrap = true;
            this.txtSideAInstructions.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;

            // grpSideBInstructions
            this.grpSideBInstructions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSideBInstructions.Text = "Указания стороны B";
            this.grpSideBInstructions.Padding = new System.Windows.Forms.Padding(10);
            this.grpSideBInstructions.Controls.Add(this.txtSideBInstructions);

            // txtSideBInstructions
            this.txtSideBInstructions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSideBInstructions.ReadOnly = true;
            this.txtSideBInstructions.WordWrap = true;
            this.txtSideBInstructions.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;

            // grpSideBInput
            this.grpSideBInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSideBInput.Text = "Сторона B (ввод)";
            this.grpSideBInput.Padding = new System.Windows.Forms.Padding(10);
            this.grpSideBInput.Controls.Add(this.sideBControl);

            // sideBControl
            this.sideBControl.Dock = System.Windows.Forms.DockStyle.Fill;

            // add to tableMain
            this.tableMain.Controls.Add(this.grpSideAInput, 0, 0);
            this.tableMain.Controls.Add(this.grpSideAInstructions, 1, 0);
            this.tableMain.Controls.Add(this.grpSideBInstructions, 2, 0);
            this.tableMain.Controls.Add(this.grpSideBInput, 3, 0);

            // panelBottom
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Height = 260;
            this.panelBottom.Padding = new System.Windows.Forms.Padding(12, 8, 12, 12);

            // btnReset
            this.btnReset.Text = "Сбросить";
            this.btnReset.Width = 140;
            this.btnReset.Height = 32;
            this.btnReset.Location = new System.Drawing.Point(12, 8);

            // btnGenerate
            this.btnGenerate.Text = "Сформировать указания";
            this.btnGenerate.Width = 220;
            this.btnGenerate.Height = 32;
            this.btnGenerate.Location = new System.Drawing.Point(162, 8);

            // txtOutput
            this.txtOutput.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtOutput.Height = 200;
            this.txtOutput.ReadOnly = true;
            this.txtOutput.WordWrap = true;
            this.txtOutput.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;

            this.panelBottom.Controls.Add(this.btnReset);
            this.panelBottom.Controls.Add(this.btnGenerate);
            this.panelBottom.Controls.Add(this.txtOutput);

            // InstructionGenerationForm
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Text = "Формирование указаний";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.MinimumSize = new System.Drawing.Size(1200, 750);
            this.ClientSize = new System.Drawing.Size(1500, 900);

            this.Controls.Add(this.tableMain);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelTop);

            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.tableMain.ResumeLayout(false);
            this.grpSideAInput.ResumeLayout(false);
            this.grpSideAInstructions.ResumeLayout(false);
            this.grpSideBInstructions.ResumeLayout(false);
            this.grpSideBInput.ResumeLayout(false);
            this.panelBottom.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
