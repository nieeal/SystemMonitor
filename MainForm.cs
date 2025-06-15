using System;
using System.Drawing;
using System.Windows.Forms;
using OpenHardwareMonitor.Hardware;

namespace SystemMonitor
{
    public partial class MainForm : Form
    {
        private Timer updateTimer;
        private Computer computer;

        private ISensor cpuLoadSensor, cpuTempSensor;
        private ISensor ramUsedSensor;
        private ISensor gpuLoadSensor, gpuTempSensor, gpuRamUsedSensor;

        private Label cpuLabel, gpuLabel, ramLabel;
        private PictureBox cpuIcon, gpuIcon;
        private GraphControl cpuGraph, gpuGraph, ramGraph;
        private Button settingsButton, closeButton;

        private Point lastMousePos;
        private bool isDragging;

        public MainForm()
        {
            ConfigData.Load();
            InitializeForm();
            InitializeSensors();
            ApplySettings();
            StartUpdateTimer();
        }

        private void InitializeForm()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.BackColor = Color.Black;
            this.DoubleBuffered = true;

            this.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    isDragging = true;
                    lastMousePos = e.Location;
                }
            };
            this.MouseMove += (s, e) =>
            {
                if (isDragging)
                {
                    this.Left += e.X - lastMousePos.X;
                    this.Top += e.Y - lastMousePos.Y;
                }
            };
            this.MouseUp += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                    isDragging = false;
            };

            settingsButton = new Button
            {
                Text = "⚙",
                ForeColor = Color.White,
                BackColor = Color.DimGray,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(30, 30),
                Location = new Point(10, 5)
            };
            settingsButton.FlatAppearance.BorderSize = 0;
            settingsButton.Click += OpenSettings_Click;
            this.Controls.Add(settingsButton);

            closeButton = new Button
            {
                Text = "X",
                ForeColor = Color.White,
                BackColor = Color.Red,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(30, 30),
                Location = new Point(45, 5),
                Visible = false
            };
            closeButton.FlatAppearance.BorderSize = 0;
            closeButton.Click += (s, e) => this.Close();
            this.Controls.Add(closeButton);

            this.MouseEnter += (s, e) => closeButton.Visible = true;
            this.MouseLeave += (s, e) =>
            {
                var pos = this.PointToClient(Cursor.Position);
                if (!closeButton.Bounds.Contains(pos))
                    closeButton.Visible = false;
            };

            cpuIcon = new PictureBox
            {
                Image = Properties.Resources.intel_cpu,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Size = new Size(24, 24)
            };
            this.Controls.Add(cpuIcon);

            cpuLabel = new Label
            {
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.White,
                AutoSize = true
            };
            this.Controls.Add(cpuLabel);

            cpuGraph = new GraphControl
            {
                Size = new Size(240, 20)
            };
            this.Controls.Add(cpuGraph);

            gpuIcon = new PictureBox
            {
                Image = Properties.Resources.intel_gpu,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Size = new Size(24, 24)
            };
            this.Controls.Add(gpuIcon);

            gpuLabel = new Label
            {
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.White,
                AutoSize = true
            };
            this.Controls.Add(gpuLabel);

            gpuGraph = new GraphControl
            {
                Size = new Size(240, 20)
            };
            this.Controls.Add(gpuGraph);

            ramLabel = new Label
            {
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.White,
                AutoSize = true
            };
            this.Controls.Add(ramLabel);

            ramGraph = new GraphControl
            {
                Size = new Size(240, 20)
            };
            this.Controls.Add(ramGraph);
        }

        private void InitializeSensors()
        {
            computer = new Computer()
            {
                CPUEnabled = true,
                GPUEnabled = true,
                RAMEnabled = true
            };
            computer.Open();

            foreach (var hw in computer.Hardware)
            {
                hw.Update();
                foreach (var s in hw.Sensors)
                {
                    if (s.SensorType == SensorType.Load && s.Name == "CPU Total")
                        cpuLoadSensor = s;
                    else if (s.SensorType == SensorType.Temperature && s.Name.Contains("CPU"))
                        cpuTempSensor = s;
                    else if (s.SensorType == SensorType.Load && s.Name == "Memory")
                        ramUsedSensor = s;
                    else if (s.SensorType == SensorType.Load && s.Name.Contains("GPU Core"))
                        gpuLoadSensor = s;
                    else if (s.SensorType == SensorType.Temperature && s.Name.Contains("GPU"))
                        gpuTempSensor = s;
                    else if (s.SensorType == SensorType.Load && s.Name.Contains("GPU Memory"))
                        gpuRamUsedSensor = s;
                }
            }
        }

        private void StartUpdateTimer()
        {
            updateTimer = new Timer();
            updateTimer.Interval = ConfigData.Current.UpdateIntervalMs;
            updateTimer.Tick += (s, e) => UpdateSensorData();

            if (ConfigData.Current.AutoUpdate)
                updateTimer.Start();
        }

        private void UpdateSensorData()
        {
            foreach (var hw in computer.Hardware)
                hw.Update();

            SetCpuInfo(cpuLoadSensor?.Value ?? 0, cpuTempSensor?.Value ?? 0);
            SetGpuInfo(gpuLoadSensor?.Value ?? 0, gpuTempSensor?.Value ?? 0, gpuRamUsedSensor?.Value ?? 0);
            SetRamInfo(ramUsedSensor?.Value ?? 0);
        }

        private void ApplySettings()
        {
            this.Opacity = ConfigData.Current.Opacity;
            this.TopMost = ConfigData.Current.AlwaysOnTop;

            cpuGraph.GraphColor = ConfigData.Current.CpuColor;
            gpuGraph.GraphColor = ConfigData.Current.GpuColor;
            ramGraph.GraphColor = ConfigData.Current.RamColor;

            cpuLabel.ForeColor = ConfigData.Current.CpuTextColor;
            gpuLabel.ForeColor = ConfigData.Current.GpuTextColor;
            ramLabel.ForeColor = ConfigData.Current.RamTextColor;

            UpdateVisibilityAndLayout();

            if (updateTimer != null)
            {
                updateTimer.Interval = ConfigData.Current.UpdateIntervalMs;
                if (ConfigData.Current.AutoUpdate)
                    updateTimer.Start();
                else
                    updateTimer.Stop();
            }
        }

        private void UpdateVisibilityAndLayout()
        {
            int marginTop = 40;
            int sectionSpacing = 50;

            int y = marginTop;

            cpuIcon.Visible = cpuLabel.Visible = cpuGraph.Visible = ConfigData.Current.ShowCpu;
            if (ConfigData.Current.ShowCpu)
            {
                cpuIcon.Location = new Point(10, y);
                cpuLabel.Location = new Point(40, y);
                cpuGraph.Location = new Point(10, y + 22);
                y += sectionSpacing;
            }

            gpuIcon.Visible = gpuLabel.Visible = gpuGraph.Visible = ConfigData.Current.ShowGpu;
            if (ConfigData.Current.ShowGpu)
            {
                gpuIcon.Location = new Point(10, y);
                gpuLabel.Location = new Point(40, y);
                gpuGraph.Location = new Point(10, y + 22);
                y += sectionSpacing;
            }

            ramLabel.Visible = ramGraph.Visible = ConfigData.Current.ShowRam;
            if (ConfigData.Current.ShowRam)
            {
                ramLabel.Location = new Point(10, y);
                ramGraph.Location = new Point(10, y + 22);
                y += sectionSpacing;
            }

            this.Size = new Size(270, y + 10);
        }

        private void SetCpuInfo(float usage, float temp)
        {
            cpuLabel.Text = $"CPU: {usage:F1}% @ {temp:F1}°C";
            cpuGraph.AddValue(usage);
        }

        private void SetGpuInfo(float usage, float temp, float ram)
        {
            gpuLabel.Text = $"GPU: {usage:F1}% @ {temp:F1}°C";
            gpuGraph.AddValue(usage);
        }

        private void SetRamInfo(float usage)
        {
            ramLabel.Text = $"RAM: {usage:F1}%";
            ramGraph.AddValue(usage);
        }

        private void OpenSettings_Click(object sender, EventArgs e)
        {
            using (var settings = new SettingsForm())
            {
                if (settings.ShowDialog() == DialogResult.OK)
                {
                    ConfigData.Save();
                    ApplySettings();
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            updateTimer?.Dispose();
            computer?.Close();
        }
    }
}
