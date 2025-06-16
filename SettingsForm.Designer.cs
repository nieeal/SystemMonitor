namespace SystemMonitor
{
    partial class SettingsForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.CheckBox checkBoxShowCpu;
        private System.Windows.Forms.CheckBox checkBoxShowGpu;
        private System.Windows.Forms.CheckBox checkBoxShowRam;
        private System.Windows.Forms.CheckBox checkBoxAlwaysOnTop;
        private System.Windows.Forms.CheckBox checkBoxShowCpuTemp;
        private System.Windows.Forms.CheckBox checkBoxShowGpuTemp;

        // Removed old checkboxes for CPU/GPU/RAM graphs to use display mode radio buttons instead

        // New group box and radio buttons for display mode
        private System.Windows.Forms.GroupBox groupBoxDisplayMode;
        private System.Windows.Forms.RadioButton radioButtonDisplayGraphsAndText;
        private System.Windows.Forms.RadioButton radioButtonDisplayGraphsOnly;
        private System.Windows.Forms.RadioButton radioButtonDisplayTextOnly;

        private System.Windows.Forms.Panel panelCpuColor;
        private System.Windows.Forms.Panel panelGpuColor;
        private System.Windows.Forms.Panel panelRamColor;
        private System.Windows.Forms.Panel panelTextColor;
        private System.Windows.Forms.Panel panelValueColor;
        private System.Windows.Forms.Panel panelBackgroundColor;

        private System.Windows.Forms.Label labelBackgroundColor;
        private System.Windows.Forms.Label labelCpuColor;
        private System.Windows.Forms.Label labelGpuColor;
        private System.Windows.Forms.Label labelRamColor;
        private System.Windows.Forms.Label labelTextColor;
        private System.Windows.Forms.Label labelValueColor;

        private System.Windows.Forms.Button btnSave;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            // Initialize checkboxes
            this.checkBoxShowCpu = new System.Windows.Forms.CheckBox();
            this.checkBoxShowGpu = new System.Windows.Forms.CheckBox();
            this.checkBoxShowRam = new System.Windows.Forms.CheckBox();
            this.checkBoxAlwaysOnTop = new System.Windows.Forms.CheckBox();
            this.checkBoxShowCpuTemp = new System.Windows.Forms.CheckBox();
            this.checkBoxShowGpuTemp = new System.Windows.Forms.CheckBox();

            // Initialize group box and radio buttons for display mode
            this.groupBoxDisplayMode = new System.Windows.Forms.GroupBox();
            this.radioButtonDisplayGraphsAndText = new System.Windows.Forms.RadioButton();
            this.radioButtonDisplayGraphsOnly = new System.Windows.Forms.RadioButton();
            this.radioButtonDisplayTextOnly = new System.Windows.Forms.RadioButton();

            // Initialize color panels and labels
            this.panelCpuColor = new System.Windows.Forms.Panel();
            this.panelGpuColor = new System.Windows.Forms.Panel();
            this.panelRamColor = new System.Windows.Forms.Panel();
            this.panelTextColor = new System.Windows.Forms.Panel();
            this.panelValueColor = new System.Windows.Forms.Panel();
            this.panelBackgroundColor = new System.Windows.Forms.Panel();

            this.labelCpuColor = new System.Windows.Forms.Label();
            this.labelGpuColor = new System.Windows.Forms.Label();
            this.labelRamColor = new System.Windows.Forms.Label();
            this.labelTextColor = new System.Windows.Forms.Label();
            this.labelValueColor = new System.Windows.Forms.Label();
            this.labelBackgroundColor = new System.Windows.Forms.Label();

            this.btnSave = new System.Windows.Forms.Button();

            this.SuspendLayout();

            int y = 15;
            int spacing = 28;
            int labelOffsetX = 15;
            int controlOffsetX = 140;

            // Setup checkboxes with text and locations
            string[] checkboxTexts = {
                "Show CPU", "Show GPU", "Show RAM",
                "Always on Top", "Show CPU Temperature", "Show GPU Temperature"
            };
            System.Windows.Forms.CheckBox[] checkboxes = {
                checkBoxShowCpu, checkBoxShowGpu, checkBoxShowRam,
                checkBoxAlwaysOnTop, checkBoxShowCpuTemp, checkBoxShowGpuTemp
            };
            for (int i = 0; i < checkboxes.Length; i++)
            {
                var cb = checkboxes[i];
                cb.Text = checkboxTexts[i];
                cb.Location = new System.Drawing.Point(labelOffsetX, y);
                cb.AutoSize = true;
                this.Controls.Add(cb);
                y += spacing;
            }

            // Setup group box for display mode
            this.groupBoxDisplayMode.Text = "Display Mode";
            this.groupBoxDisplayMode.Location = new System.Drawing.Point(labelOffsetX, y);
            this.groupBoxDisplayMode.Size = new System.Drawing.Size(200, 90);
            this.groupBoxDisplayMode.TabStop = false;

            // Setup radio buttons inside group box
            this.radioButtonDisplayGraphsAndText.Text = "Graphs + Text";
            this.radioButtonDisplayGraphsAndText.Location = new System.Drawing.Point(10, 20);
            this.radioButtonDisplayGraphsAndText.AutoSize = true;
            this.radioButtonDisplayGraphsAndText.Name = "radioButtonDisplayGraphsAndText";
            this.radioButtonDisplayGraphsAndText.Checked = true; // default

            this.radioButtonDisplayGraphsOnly.Text = "Graphs Only";
            this.radioButtonDisplayGraphsOnly.Location = new System.Drawing.Point(10, 45);
            this.radioButtonDisplayGraphsOnly.AutoSize = true;
            this.radioButtonDisplayGraphsOnly.Name = "radioButtonDisplayGraphsOnly";

            this.radioButtonDisplayTextOnly.Text = "Text Only";
            this.radioButtonDisplayTextOnly.Location = new System.Drawing.Point(10, 70);
            this.radioButtonDisplayTextOnly.AutoSize = true;
            this.radioButtonDisplayTextOnly.Name = "radioButtonDisplayTextOnly";

            this.groupBoxDisplayMode.Controls.Add(this.radioButtonDisplayGraphsAndText);
            this.groupBoxDisplayMode.Controls.Add(this.radioButtonDisplayGraphsOnly);
            this.groupBoxDisplayMode.Controls.Add(this.radioButtonDisplayTextOnly);

            this.Controls.Add(this.groupBoxDisplayMode);

            y += this.groupBoxDisplayMode.Height + 10;

            // Helper to add color label + panel
            void AddColorSetting(string labelText, System.Windows.Forms.Label label, System.Windows.Forms.Panel panel)
            {
                label.Text = labelText;
                label.Location = new System.Drawing.Point(labelOffsetX, y);
                label.AutoSize = true;
                panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                panel.Size = new System.Drawing.Size(35, 22);
                panel.Location = new System.Drawing.Point(controlOffsetX, y - 2);
                panel.Cursor = System.Windows.Forms.Cursors.Hand;
                panel.Click += new System.EventHandler(this.ColorPanel_Click);

                this.Controls.Add(label);
                this.Controls.Add(panel);

                y += spacing;
            }

            AddColorSetting("CPU Graph Color:", labelCpuColor, panelCpuColor);
            AddColorSetting("GPU Graph Color:", labelGpuColor, panelGpuColor);
            AddColorSetting("RAM Graph Color:", labelRamColor, panelRamColor);
            AddColorSetting("Text Color:", labelTextColor, panelTextColor);
            AddColorSetting("Value Color:", labelValueColor, panelValueColor);
            AddColorSetting("Background Color:", labelBackgroundColor, panelBackgroundColor);

            // Setup Save button
            this.btnSave.Text = "Save";
            this.btnSave.Location = new System.Drawing.Point(labelOffsetX, y);
            this.btnSave.Size = new System.Drawing.Size(160, 30);
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            this.Controls.Add(this.btnSave);

            y += 50;

            // Final form setup
            this.ClientSize = new System.Drawing.Size(250, y);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";

            this.ResumeLayout(false);
        }
    }
}
