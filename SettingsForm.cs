using System;
using System.Drawing;
using System.Windows.Forms;

namespace SystemMonitor
{
    public class SettingsForm : Form
    {
        private TrackBar opacitySlider;
        private CheckBox autoUpdateCheckBox;
        private CheckBox alwaysOnTopCheckBox;

        private CheckBox showCpuCheckBox;
        private CheckBox showGpuCheckBox;
        private CheckBox showRamCheckBox;

        private Button cpuGraphColorBtn;
        private Button gpuGraphColorBtn;
        private Button ramGraphColorBtn;

        private Button cpuTextColorBtn;
        private Button gpuTextColorBtn;
        private Button ramTextColorBtn;

        private Button okButton, cancelButton;

        private ColorDialog colorDialog = new ColorDialog();

        public SettingsForm()
        {
            InitializeComponent();
            colorDialog.FullOpen = true;

            this.AcceptButton = okButton;
            this.CancelButton = cancelButton;
        }

        private void InitializeComponent()
        {
            this.Text = "Settings";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            int labelX = 20;
            int controlX = 150;
            int y = 20;
            int yStep = 35;

            // Opacity
            Label opacityLabel = new Label { Text = "Opacity", Location = new Point(labelX, y + 5), AutoSize = true };
            this.Controls.Add(opacityLabel);

            opacitySlider = new TrackBar
            {
                Minimum = 20,
                Maximum = 100,
                TickFrequency = 10,
                Value = (int)(ConfigData.Current.Opacity * 100),
                Location = new Point(controlX, y),
                Width = 200,
                TabIndex = 0
            };
            this.Controls.Add(opacitySlider);

            y += yStep;

            // Auto Update
            autoUpdateCheckBox = new CheckBox
            {
                Text = "Auto Update",
                Checked = ConfigData.Current.AutoUpdate,
                Location = new Point(labelX, y),
                AutoSize = true,
                TabIndex = 1
            };
            this.Controls.Add(autoUpdateCheckBox);

            y += yStep;

            // Always On Top (default false if config is default)
            bool alwaysOnTopDefault = ConfigData.Current.AlwaysOnTop;
            if (!ConfigData.HasLoadedSettings) // Assuming you have a flag for first load
                alwaysOnTopDefault = false;

            alwaysOnTopCheckBox = new CheckBox
            {
                Text = "Always On Top",
                Checked = alwaysOnTopDefault,
                Location = new Point(labelX, y),
                AutoSize = true,
                TabIndex = 2
            };
            this.Controls.Add(alwaysOnTopCheckBox);

            y += yStep;

            // Show CPU
            showCpuCheckBox = new CheckBox
            {
                Text = "Show CPU",
                Checked = ConfigData.Current.ShowCpu,
                Location = new Point(labelX, y),
                AutoSize = true,
                TabIndex = 3
            };
            this.Controls.Add(showCpuCheckBox);

            y += yStep;

            // Show GPU
            showGpuCheckBox = new CheckBox
            {
                Text = "Show GPU",
                Checked = ConfigData.Current.ShowGpu,
                Location = new Point(labelX, y),
                AutoSize = true,
                TabIndex = 4
            };
            this.Controls.Add(showGpuCheckBox);

            y += yStep;

            // Show RAM
            showRamCheckBox = new CheckBox
            {
                Text = "Show RAM",
                Checked = ConfigData.Current.ShowRam,
                Location = new Point(labelX, y),
                AutoSize = true,
                TabIndex = 5
            };
            this.Controls.Add(showRamCheckBox);

            y += yStep + 10;

            // Graph Colors Label
            Label graphColorsLabel = new Label
            {
                Text = "Graph Colors",
                Location = new Point(labelX, y),
                Font = new Font(Font.FontFamily, 10, FontStyle.Bold),
                AutoSize = true
            };
            this.Controls.Add(graphColorsLabel);

            y += yStep;

            // CPU Graph Color
            CreateColorPickerButton("CPU Graph:", ConfigData.Current.CpuColor, labelX, y, out cpuGraphColorBtn, CpuGraphColorBtn_Click, 6);

            y += yStep;

            // GPU Graph Color
            CreateColorPickerButton("GPU Graph:", ConfigData.Current.GpuColor, labelX, y, out gpuGraphColorBtn, GpuGraphColorBtn_Click, 7);

            y += yStep;

            // RAM Graph Color
            CreateColorPickerButton("RAM Graph:", ConfigData.Current.RamColor, labelX, y, out ramGraphColorBtn, RamGraphColorBtn_Click, 8);

            y += yStep + 10;

            // Text Colors Label
            Label textColorsLabel = new Label
            {
                Text = "Text Colors",
                Location = new Point(labelX, y),
                Font = new Font(Font.FontFamily, 10, FontStyle.Bold),
                AutoSize = true
            };
            this.Controls.Add(textColorsLabel);

            y += yStep;

            // CPU Text Color
            CreateColorPickerButton("CPU Text:", ConfigData.Current.CpuTextColor, labelX, y, out cpuTextColorBtn, CpuTextColorBtn_Click, 9);

            y += yStep;

            // GPU Text Color
            CreateColorPickerButton("GPU Text:", ConfigData.Current.GpuTextColor, labelX, y, out gpuTextColorBtn, GpuTextColorBtn_Click, 10);

            y += yStep;

            // RAM Text Color
            CreateColorPickerButton("RAM Text:", ConfigData.Current.RamTextColor, labelX, y, out ramTextColorBtn, RamTextColorBtn_Click, 11);

            y += yStep + 20;

            // OK Button
            okButton = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Location = new Point(200, y),
                Size = new Size(75, 30),
                TabIndex = 12
            };
            okButton.Click += OkButton_Click;
            this.Controls.Add(okButton);

            // Cancel Button
            cancelButton = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Location = new Point(290, y),
                Size = new Size(75, 30),
                TabIndex = 13
            };
            this.Controls.Add(cancelButton);

            // Set FlatAppearance.BorderSize for color buttons for better visibility
            SetFlatAppearance(cpuGraphColorBtn);
            SetFlatAppearance(gpuGraphColorBtn);
            SetFlatAppearance(ramGraphColorBtn);
            SetFlatAppearance(cpuTextColorBtn);
            SetFlatAppearance(gpuTextColorBtn);
            SetFlatAppearance(ramTextColorBtn);

            // Set tab order for color buttons (optional)
            cpuGraphColorBtn.TabIndex = 6;
            gpuGraphColorBtn.TabIndex = 7;
            ramGraphColorBtn.TabIndex = 8;

            cpuTextColorBtn.TabIndex = 9;
            gpuTextColorBtn.TabIndex = 10;
            ramTextColorBtn.TabIndex = 11;
        }

        private void SetFlatAppearance(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderColor = Color.Black;
            btn.FlatAppearance.BorderSize = 1;
        }

        private void CreateColorPickerButton(string label, Color initialColor, int labelX, int y, out Button button, EventHandler clickHandler, int tabIndex)
        {
            Label colorLabel = new Label
            {
                Text = label,
                Location = new Point(labelX, y + 5),
                AutoSize = true
            };
            this.Controls.Add(colorLabel);

            button = new Button
            {
                BackColor = initialColor,
                Size = new Size(40, 23),
                Location = new Point(labelX + 100, y),
                TabIndex = tabIndex
            };
            SetFlatAppearance(button);
            button.Click += clickHandler;
            this.Controls.Add(button);
        }

        private void CpuGraphColorBtn_Click(object sender, EventArgs e) => PickColor(cpuGraphColorBtn);
        private void GpuGraphColorBtn_Click(object sender, EventArgs e) => PickColor(gpuGraphColorBtn);
        private void RamGraphColorBtn_Click(object sender, EventArgs e) => PickColor(ramGraphColorBtn);
        private void CpuTextColorBtn_Click(object sender, EventArgs e) => PickColor(cpuTextColorBtn);
        private void GpuTextColorBtn_Click(object sender, EventArgs e) => PickColor(gpuTextColorBtn);
        private void RamTextColorBtn_Click(object sender, EventArgs e) => PickColor(ramTextColorBtn);

        private void PickColor(Button button)
        {
            colorDialog.Color = button.BackColor;
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                button.BackColor = colorDialog.Color;
            }
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            ConfigData.Current.Opacity = opacitySlider.Value / 100f;
            ConfigData.Current.AutoUpdate = autoUpdateCheckBox.Checked;
            ConfigData.Current.AlwaysOnTop = alwaysOnTopCheckBox.Checked;

            ConfigData.Current.ShowCpu = showCpuCheckBox.Checked;
            ConfigData.Current.ShowGpu = showGpuCheckBox.Checked;
            ConfigData.Current.ShowRam = showRamCheckBox.Checked;

            ConfigData.Current.CpuColor = cpuGraphColorBtn.BackColor;
            ConfigData.Current.GpuColor = gpuGraphColorBtn.BackColor;
            ConfigData.Current.RamColor = ramGraphColorBtn.BackColor;

            ConfigData.Current.CpuTextColor = cpuTextColorBtn.BackColor;
            ConfigData.Current.GpuTextColor = gpuTextColorBtn.BackColor;
            ConfigData.Current.RamTextColor = ramTextColorBtn.BackColor;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
