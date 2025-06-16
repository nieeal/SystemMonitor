using OpenHardwareMonitor.Hardware;
using System;
using System.Linq;

namespace SystemMonitor
{
    public class SensorManager
    {
        private Computer computer;

        public float CpuUsage { get; private set; }
        public float GpuUsage { get; private set; }
        public float RamUsage { get; private set; }

        public float CpuTemperature { get; private set; }
        public float GpuTemperature { get; private set; }

        public float RamInstalledGB { get; private set; }
        public float RamUsedGB { get; private set; }
        public float RamFreeGB { get; private set; }

        public float? CpuTemp => CpuTemperature;
        public float? GpuTemp => GpuTemperature;

        public SensorManager()
        {
            computer = new Computer
            {
                CPUEnabled = true,
                GPUEnabled = true,
                RAMEnabled = true,
                MainboardEnabled = true
            };
            computer.Open();
        }

        public void Update()
        {
            CpuUsage = 0f;
            GpuUsage = 0f;
            RamUsage = 0f;
            CpuTemperature = 0f;
            GpuTemperature = 0f;

            RamInstalledGB = 0f;
            RamUsedGB = 0f;
            RamFreeGB = 0f;

            foreach (var hardware in computer.Hardware)
            {
                hardware.Update();

                // CPU Temperature
                if (hardware.HardwareType == HardwareType.CPU)
                {
                    var tempSensors = hardware.Sensors
                        .Where(s => s.SensorType == SensorType.Temperature && s.Value != null)
                        .ToList();

                    var primaryCpuTemp = tempSensors.FirstOrDefault(s =>
                        s.Name.ToLower().Contains("package") || s.Name.ToLower().Contains("core"));

                    if (primaryCpuTemp != null)
                        CpuTemperature = primaryCpuTemp.Value ?? 0f;
                    else if (tempSensors.Any())
                        CpuTemperature = tempSensors.Max(s => s.Value ?? 0f);
                }

                // GPU Temperature
                if (hardware.HardwareType == HardwareType.GpuNvidia ||
                    hardware.HardwareType == HardwareType.GpuAti ||
                    hardware.Name.ToLower().Contains("intel"))
                {
                    var tempSensors = hardware.Sensors
                        .Where(s => s.SensorType == SensorType.Temperature && s.Value != null)
                        .ToList();

                    var primaryGpuTemp = tempSensors.FirstOrDefault(s =>
                        s.Name.ToLower().Contains("core") || s.Name.ToLower().Contains("gpu"));

                    if (primaryGpuTemp != null)
                        GpuTemperature = primaryGpuTemp.Value ?? 0f;
                    else if (tempSensors.Any())
                        GpuTemperature = tempSensors.Max(s => s.Value ?? 0f);
                }

                foreach (var sensor in hardware.Sensors)
                {
                    if (sensor?.Value == null)
                        continue;

                    switch (sensor.SensorType)
                    {
                        case SensorType.Load:
                            if (sensor.Name.Contains("CPU Total"))
                                CpuUsage = sensor.Value ?? 0f;

                            if (sensor.Name.ToLower().Contains("gpu core") || sensor.Name.ToLower().Contains("gpu total"))
                                GpuUsage = sensor.Value ?? 0f;

                            if (sensor.Name.ToLower().Contains("memory") || sensor.Name.ToLower().Contains("ram"))
                                RamUsage = sensor.Value ?? 0f;
                            break;

                        case SensorType.Data:
                            if (hardware.HardwareType == HardwareType.RAM)
                            {
                                string lowerName = sensor.Name.ToLower();
                                if (lowerName.Contains("used memory"))
                                    RamUsedGB = (sensor.Value ?? 0f) / 1024f;
                                else if (lowerName.Contains("available memory"))
                                    RamFreeGB = (sensor.Value ?? 0f) / 1024f;
                                else if (lowerName.Contains("total memory") || lowerName.Contains("memory"))
                                    RamInstalledGB = (sensor.Value ?? 0f) / 1024f;
                            }
                            break;
                    }
                }
            }

            if (RamInstalledGB == 0)
                RamInstalledGB = RamUsedGB + RamFreeGB;
        }
    }
}
