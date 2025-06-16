using System;
using System.Drawing;
using System.Windows.Forms;

namespace SystemMonitor
{
    public partial class SettingsForm : Form
    {
        public ConfigData Config { get; private set; }

        public SettingsForm(ConfigData config)
        {
            InitializeComponent();
            Config = config;

            // Load checkboxes states from config
            checkBoxShowCpu.Checked = config.ShowCpu;
            checkBoxShowGpu.Checked = config.ShowGpu;
            checkBoxShowRam.Checked = config.ShowRam;
            checkBoxAlwaysOnTop.Checked = config.AlwaysOnTop;
            checkBoxShowCpuTemp.Checked = config.ShowCpuTemp;
            checkBoxShowGpuTemp.Checked = config.ShowGpuTemp;

            // Load display mode radio buttons states from config
            if (config.DisplayGraphsOnly)
                radioButtonDisplayGraphsOnly.Checked = true;
            else if (config.DisplayTextOnly)
                radioButtonDisplayTextOnly.Checked = true;
            else
                radioButtonDisplayGraphsAndText.Checked = true;

            // Load colors into panels
            panelCpuColor.BackColor = config.CpuGraphColor;
            panelGpuColor.BackColor = config.GpuGraphColor;
            panelRamColor.BackColor = config.RamGraphColor;
            panelTextColor.BackColor = config.TextColor;
            panelValueColor.BackColor = config.ValueColor;
            panelBackgroundColor.BackColor = config.BackgroundColor;

            // Wire up color panel click events (if not wired in Designer)
            panelCpuColor.Click += ColorPanel_Click;
            panelGpuColor.Click += ColorPanel_Click;
            panelRamColor.Click += ColorPanel_Click;
            panelTextColor.Click += ColorPanel_Click;
            panelValueColor.Click += ColorPanel_Click;
            panelBackgroundColor.Click += ColorPanel_Click;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Save checkboxes states back to config
            Config.ShowCpu = checkBoxShowCpu.Checked;
            Config.ShowGpu = checkBoxShowGpu.Checked;
            Config.ShowRam = checkBoxShowRam.Checked;
            Config.AlwaysOnTop = checkBoxAlwaysOnTop.Checked;
            Config.ShowCpuTemp = checkBoxShowCpuTemp.Checked;
            Config.ShowGpuTemp = checkBoxShowGpuTemp.Checked;

            // Save display mode radio buttons state to config
            if (radioButtonDisplayGraphsOnly.Checked)
            {
                Config.DisplayGraphsOnly = true;
                Config.DisplayTextOnly = false;
            }
            else if (radioButtonDisplayTextOnly.Checked)
            {
                Config.DisplayGraphsOnly = false;
                Config.DisplayTextOnly = true;
            }
            else
            {
                // Graphs + Text
                Config.DisplayGraphsOnly = false;
                Config.DisplayTextOnly = false;
            }

            // Save colors from panels back to config
            Config.CpuGraphColor = panelCpuColor.BackColor;
            Config.GpuGraphColor = panelGpuColor.BackColor;
            Config.RamGraphColor = panelRamColor.BackColor;
            Config.TextColor = panelTextColor.BackColor;
            Config.ValueColor = panelValueColor.BackColor;
            Config.BackgroundColor = panelBackgroundColor.BackColor;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void ColorPanel_Click(object sender, EventArgs e)
        {
            if (sender is Panel panel)
            {
                using (ColorDialog colorDialog = new ColorDialog())
                {
                    colorDialog.Color = panel.BackColor;
                    if (colorDialog.ShowDialog() == DialogResult.OK)
                    {
                        panel.BackColor = colorDialog.Color;
                    }
                }
            }
        }
    }
}
