namespace SystemMonitor
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Timer UpdateTimer;
        private GraphControl graphControlCpu;
        private GraphControl graphControlGpu;
        private GraphControl graphControlRam;
        private System.Windows.Forms.Label labelCpu;
        private System.Windows.Forms.Label labelGpu;
        private System.Windows.Forms.Label labelRam;
        private System.Windows.Forms.Label labelCpuValue;
        private System.Windows.Forms.Label labelGpuValue;
        private System.Windows.Forms.Label labelRamValue;
        private System.Windows.Forms.Label labelCpuTemp;
        private System.Windows.Forms.Label labelGpuTemp;
        private System.Windows.Forms.Label labelRamInstalled;
        private System.Windows.Forms.Label labelRamUsed;
        private System.Windows.Forms.Label labelRamFree;
        private System.Windows.Forms.ProgressBar progressBarRamUsage;
        private System.Windows.Forms.Button settingsButton;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.SuspendLayout();

            this.UpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.UpdateTimer.Interval = 1000;
            this.UpdateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);

            this.graphControlCpu = new SystemMonitor.GraphControl();
            this.graphControlGpu = new SystemMonitor.GraphControl();
            this.graphControlRam = new SystemMonitor.GraphControl();

            this.labelCpu = new System.Windows.Forms.Label();
            this.labelGpu = new System.Windows.Forms.Label();
            this.labelRam = new System.Windows.Forms.Label();

            this.labelCpuValue = new System.Windows.Forms.Label();
            this.labelGpuValue = new System.Windows.Forms.Label();
            this.labelRamValue = new System.Windows.Forms.Label();

            this.labelCpuTemp = new System.Windows.Forms.Label();
            this.labelGpuTemp = new System.Windows.Forms.Label();

            this.labelRamInstalled = new System.Windows.Forms.Label();
            this.labelRamUsed = new System.Windows.Forms.Label();
            this.labelRamFree = new System.Windows.Forms.Label();

            this.progressBarRamUsage = new System.Windows.Forms.ProgressBar();

            this.settingsButton = new System.Windows.Forms.Button();

            // 
            // graphControlCpu
            // 
            this.graphControlCpu.Location = new System.Drawing.Point(10, 10);
            this.graphControlCpu.Size = new System.Drawing.Size(200, 50);

            // 
            // labelCpu
            // 
            this.labelCpu.Location = new System.Drawing.Point(220, 10);
            this.labelCpu.Text = "CPU Usage:";
            this.labelCpu.AutoSize = true;

            // 
            // labelCpuValue
            // 
            this.labelCpuValue.Location = new System.Drawing.Point(300, 10);
            this.labelCpuValue.Text = "0%";
            this.labelCpuValue.AutoSize = true;

            // 
            // labelCpuTemp
            // 
            this.labelCpuTemp.Location = new System.Drawing.Point(220, 30);
            this.labelCpuTemp.Text = "CPU Temp: N/A";
            this.labelCpuTemp.AutoSize = true;

            // 
            // graphControlGpu
            // 
            this.graphControlGpu.Location = new System.Drawing.Point(10, 70);
            this.graphControlGpu.Size = new System.Drawing.Size(200, 50);

            // 
            // labelGpu
            // 
            this.labelGpu.Location = new System.Drawing.Point(220, 70);
            this.labelGpu.Text = "GPU Usage:";
            this.labelGpu.AutoSize = true;

            // 
            // labelGpuValue
            // 
            this.labelGpuValue.Location = new System.Drawing.Point(300, 70);
            this.labelGpuValue.Text = "0%";
            this.labelGpuValue.AutoSize = true;

            // 
            // labelGpuTemp
            // 
            this.labelGpuTemp.Location = new System.Drawing.Point(220, 90);
            this.labelGpuTemp.Text = "GPU Temp: N/A";
            this.labelGpuTemp.AutoSize = true;

            // 
            // graphControlRam
            // 
            this.graphControlRam.Location = new System.Drawing.Point(10, 130);
            this.graphControlRam.Size = new System.Drawing.Size(200, 50);

            // 
            // labelRam
            // 
            this.labelRam.Location = new System.Drawing.Point(220, 130);
            this.labelRam.Text = "RAM Usage:";
            this.labelRam.AutoSize = true;

            // 
            // labelRamValue
            // 
            this.labelRamValue.Location = new System.Drawing.Point(300, 130);
            this.labelRamValue.Text = "0%";
            this.labelRamValue.AutoSize = true;

            // 
            // progressBarRamUsage
            // 
            this.progressBarRamUsage.Location = new System.Drawing.Point(10, 185);
            this.progressBarRamUsage.Size = new System.Drawing.Size(200, 20);
            this.progressBarRamUsage.Minimum = 0;
            this.progressBarRamUsage.Maximum = 100;
            this.progressBarRamUsage.Value = 0;

            // 
            // labelRamInstalled
            // 
            this.labelRamInstalled.Location = new System.Drawing.Point(220, 150);
            this.labelRamInstalled.Text = "Installed RAM: N/A";
            this.labelRamInstalled.AutoSize = true;

            // 
            // labelRamUsed
            // 
            this.labelRamUsed.Location = new System.Drawing.Point(220, 170);
            this.labelRamUsed.Text = "Used RAM: N/A";
            this.labelRamUsed.AutoSize = true;

            // 
            // labelRamFree
            // 
            this.labelRamFree.Location = new System.Drawing.Point(220, 190);
            this.labelRamFree.Text = "Free RAM: N/A";
            this.labelRamFree.AutoSize = true;

            // 
            // settingsButton
            // 
            this.settingsButton.Location = new System.Drawing.Point(10, 215);
            this.settingsButton.Size = new System.Drawing.Size(100, 35);
            this.settingsButton.Text = "Settings";
            this.settingsButton.Click += new System.EventHandler(this.settingsButton_Click);

            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(400, 260);
            this.Controls.Add(this.graphControlCpu);
            this.Controls.Add(this.graphControlGpu);
            this.Controls.Add(this.graphControlRam);

            this.Controls.Add(this.labelCpu);
            this.Controls.Add(this.labelCpuValue);
            this.Controls.Add(this.labelCpuTemp);

            this.Controls.Add(this.labelGpu);
            this.Controls.Add(this.labelGpuValue);
            this.Controls.Add(this.labelGpuTemp);

            this.Controls.Add(this.labelRam);
            this.Controls.Add(this.labelRamValue);

            this.Controls.Add(this.progressBarRamUsage);

            this.Controls.Add(this.labelRamInstalled);
            this.Controls.Add(this.labelRamUsed);
            this.Controls.Add(this.labelRamFree);

            this.Controls.Add(this.settingsButton);

            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None; // <-- changed from FixedSingle
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "System Monitor";

            this.DoubleBuffered = true; // smoother redraws

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
