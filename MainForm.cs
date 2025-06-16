using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SystemMonitor
{
    public partial class MainForm : Form
    {
        private SensorManager sensorManager;
        private ConfigData config;
        private Button closeButton;
        private Panel topBar;

        public MainForm()
        {
            LoadConfiguration();

            this.ClientSize = new Size(820, 440); // Slightly bigger window

            if (config.WindowLocation.HasValue)
            {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = config.WindowLocation.Value;
            }

            InitializeComponent();
            InitializeTopBar();
            ConfigureWindow();
            InitializeSensorManager();
            ApplyAlwaysOnTop();
            UpdateTimer.Start();

            LayoutStaticControls();
        }

        private void InitializeTopBar()
        {
            topBar = new Panel();
            topBar.Height = 35;
            topBar.Dock = DockStyle.Top;
            topBar.BackColor = Color.LightGray;
            topBar.MouseDown += MainForm_MouseDown;
            topBar.MouseEnter += (s, e) => closeButton.Visible = true;
            topBar.MouseLeave += (s, e) => closeButton.Visible = false;
            this.Controls.Add(topBar);

            closeButton = new Button();
            closeButton.Text = "X";
            closeButton.Size = new Size(30, 25);
            closeButton.Location = new Point(this.ClientSize.Width - 35, 5);
            closeButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            closeButton.FlatStyle = FlatStyle.Flat;
            closeButton.BackColor = Color.LightGray;
            closeButton.ForeColor = Color.Black;
            closeButton.Visible = false;
            closeButton.Click += (s, e) => this.Close();
            topBar.Controls.Add(closeButton);

            settingsButton.Text = "Settings";
            settingsButton.Size = new Size(100, 25);
            settingsButton.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            settingsButton.ForeColor = Color.Black;
            settingsButton.BackColor = Color.White;
            settingsButton.FlatStyle = FlatStyle.Standard;
            settingsButton.Location = new Point(10, 5);
            topBar.Controls.Add(settingsButton);
        }

        private void LoadConfiguration()
        {
            config = ConfigData.Load();
        }

        private void ConfigureWindow()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = config.BackgroundColor;

            int radius = 20;
            var path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(new Rectangle(0, 0, radius, radius), 180, 90);
            path.AddLine(radius, 0, this.Width - radius, 0);
            path.AddArc(new Rectangle(this.Width - radius, 0, radius, radius), 270, 90);
            path.AddLine(this.Width, radius, this.Width, this.Height - radius);
            path.AddArc(new Rectangle(this.Width - radius, this.Height - radius, radius, radius), 0, 90);
            path.AddLine(this.Width - radius, this.Height, radius, this.Height);
            path.AddArc(new Rectangle(0, this.Height - radius, radius, radius), 90, 90);
            path.CloseFigure();

            this.Region = new Region(path);
            this.MouseDown += MainForm_MouseDown;
        }

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void InitializeSensorManager()
        {
            sensorManager = new SensorManager();

            graphControlCpu.LineColor = config.CpuGraphColor;
            graphControlGpu.LineColor = config.GpuGraphColor;
            graphControlRam.LineColor = config.RamGraphColor;

            labelCpu.ForeColor = config.TextColor;
            labelGpu.ForeColor = config.TextColor;
            labelRam.ForeColor = config.TextColor;

            labelCpuValue.ForeColor = config.ValueColor;
            labelGpuValue.ForeColor = config.ValueColor;
            labelRamValue.ForeColor = config.ValueColor;

            labelCpuTemp.ForeColor = config.ValueColor;
            labelGpuTemp.ForeColor = config.ValueColor;
        }

        private void LayoutStaticControls()
        {
            int margin = 20;
            int labelWidth = 150;
            int graphWidth = 300;
            int spacing = 10;
            int y = margin + 45; // Adjusted for top bar height

            labelCpu.SetBounds(margin, y, labelWidth, 20);
            labelCpuValue.SetBounds(margin, y + 20, labelWidth, 20);
            labelCpuTemp.SetBounds(margin, y + 40, labelWidth, 20);
            graphControlCpu.SetBounds(margin + labelWidth + spacing, y, graphWidth, 60);
            y += 80;

            labelGpu.SetBounds(margin, y, labelWidth, 20);
            labelGpuValue.SetBounds(margin, y + 20, labelWidth, 20);
            labelGpuTemp.SetBounds(margin, y + 40, labelWidth, 20);
            graphControlGpu.SetBounds(margin + labelWidth + spacing, y, graphWidth, 60);
            y += 80;

            labelRam.SetBounds(margin, y, labelWidth, 20);
            labelRamValue.SetBounds(margin, y + 20, labelWidth, 20);
            labelRamInstalled.SetBounds(margin, y + 40, labelWidth + 50, 20);
            labelRamUsed.SetBounds(margin, y + 60, labelWidth + 50, 20);
            labelRamFree.SetBounds(margin, y + 80, labelWidth + 50, 20);
            progressBarRamUsage.SetBounds(margin + labelWidth + spacing, y + 40, graphWidth, 20);
            graphControlRam.SetBounds(margin + labelWidth + spacing, y, graphWidth, 60);
        }

        private void ApplyAlwaysOnTop()
        {
            this.TopMost = config.AlwaysOnTop;
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            sensorManager.Update();

            float cpuUsage = sensorManager.CpuUsage;
            float gpuUsage = sensorManager.GpuUsage;
            float ramUsage = sensorManager.RamUsage;

            labelCpuValue.Text = $"{cpuUsage:F1}%";
            labelGpuValue.Text = $"{gpuUsage:F1}%";
            labelRamValue.Text = $"{ramUsage:F1}%";

            labelCpuTemp.Text = sensorManager.CpuTemp.HasValue ? $"{sensorManager.CpuTemp.Value:F1} °C" : "N/A";
            labelGpuTemp.Text = sensorManager.GpuTemp.HasValue ? $"{sensorManager.GpuTemp.Value:F1} °C" : "N/A";

            labelRamInstalled.Text = $"Installed RAM: {sensorManager.RamInstalledGB:F1} GB";
            labelRamUsed.Text = $"Used RAM: {sensorManager.RamUsedGB:F1} GB";
            labelRamFree.Text = $"Free RAM: {sensorManager.RamFreeGB:F1} GB";

            progressBarRamUsage.Value = (int)ramUsage;

            graphControlCpu.AddValue(cpuUsage);
            graphControlGpu.AddValue(gpuUsage);
            graphControlRam.AddValue(ramUsage);
        }

        private void settingsButton_Click(object sender, EventArgs e)
        {
            using (SettingsForm settings = new SettingsForm(config))
            {
                if (settings.ShowDialog() == DialogResult.OK)
                {
                    config = settings.Config;
                    config.Save();

                    ApplyAlwaysOnTop();
                    LayoutStaticControls();
                    UpdateTimer_Tick(null, null);
                }
            }
        }
    }
}
