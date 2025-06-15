using System;
using OpenHardwareMonitor.Hardware;

namespace SystemMonitor
{
    public class HardwareMonitor : IDisposable
    {
        private Computer computer;

        public HardwareMonitor()
        {
            computer = new Computer()
            {
                CPUEnabled = true,
                GPUEnabled = true,
                RAMEnabled = true
            };
            computer.Open();
        }

        public void Dispose()
        {
            computer.Close();
            computer = null;
        }

        private string GetManufacturerFromName(string name)
        {
            string lowerName = name.ToLower();
            if (lowerName.Contains("intel")) return "Intel";
            if (lowerName.Contains("amd")) return "AMD";
            if (lowerName.Contains("nvidia")) return "NVIDIA";
            return "";
        }

        public CpuInfo GetCpuInfo()
        {
            foreach (var hardware in computer.Hardware)
            {
                if (hardware.HardwareType == HardwareType.CPU)
                {
                    hardware.Update();

                    string manufacturer = GetManufacturerFromName(hardware.Name);
                    string name = hardware.Name;
                    float cpuTemp = 0f;
                    float cpuLoad = 0f;

                    foreach (var sensor in hardware.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Temperature && sensor.Name.ToLower().Contains("package"))
                        {
                            cpuTemp = sensor.Value ?? 0f;
                        }
                        else if (sensor.SensorType == SensorType.Load && sensor.Name == "CPU Total")
                        {
                            cpuLoad = sensor.Value ?? 0f;
                        }
                    }

                    return new CpuInfo
                    {
                        Manufacturer = manufacturer,
                        Name = name,
                        Temperature = cpuTemp,
                        Load = cpuLoad
                    };
                }
            }
            return null;
        }

        public RamInfo GetRamInfo()
        {
            foreach (var hardware in computer.Hardware)
            {
                if (hardware.HardwareType == HardwareType.RAM)
                {
                    hardware.Update();

                    float used = 0f;
                    float total = 0f;

                    foreach (var sensor in hardware.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Data)
                        {
                            if (sensor.Name.ToLower().Contains("used"))
                                used = sensor.Value ?? 0f;
                            else if (sensor.Name.ToLower().Contains("total"))
                                total = sensor.Value ?? 0f;
                        }
                    }

                    return new RamInfo
                    {
                        UsedMB = used,
                        TotalMB = total,
                        TypeSpeed = hardware.Name
                    };
                }
            }
            return null;
        }

        public GpuInfo GetGpuInfo()
        {
            foreach (var hardware in computer.Hardware)
            {
                if (hardware.HardwareType == HardwareType.GpuNvidia || hardware.HardwareType == HardwareType.GpuAti)
                {
                    hardware.Update();

                    string manufacturer = GetManufacturerFromName(hardware.Name);
                    string name = hardware.Name;

                    float gpuLoad = 0f;
                    float gpuTemp = 0f;
                    float gpuRamUsed = 0f;
                    float gpuRamTotal = 0f;

                    foreach (var sensor in hardware.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Load && sensor.Name == "GPU Core")
                        {
                            gpuLoad = sensor.Value ?? 0f;
                        }
                        else if (sensor.SensorType == SensorType.Temperature && sensor.Name == "GPU Core")
                        {
                            gpuTemp = sensor.Value ?? 0f;
                        }
                        else if (sensor.SensorType == SensorType.SmallData && sensor.Name.ToLower().Contains("memory used"))
                        {
                            gpuRamUsed = sensor.Value ?? 0f;
                        }
                        else if (sensor.SensorType == SensorType.SmallData && sensor.Name.ToLower().Contains("memory total"))
                        {
                            gpuRamTotal = sensor.Value ?? 0f;
                        }
                    }

                    return new GpuInfo
                    {
                        Manufacturer = manufacturer,
                        Name = name,
                        Load = gpuLoad,
                        Temperature = gpuTemp,
                        RamUsedMB = gpuRamUsed,
                        RamTotalMB = gpuRamTotal
                    };
                }
            }
            return null;
        }
    }

    public class CpuInfo
    {
        public string Manufacturer { get; set; }
        public string Name { get; set; }
        public float Temperature { get; set; }
        public float TemperatureCelsius => Temperature; // alias
        public float Load { get; set; }
        public float LoadPercent => Load; // alias
    }

    public class GpuInfo
    {
        public string Manufacturer { get; set; }
        public string Name { get; set; }
        public float Load { get; set; }
        public float LoadPercent => Load; // alias
        public float Temperature { get; set; }
        public float TemperatureCelsius => Temperature; // alias
        public float RamUsedMB { get; set; }
        public float RamTotalMB { get; set; }
    }

    public class RamInfo
    {
        public float UsedMB { get; set; }
        public float TotalMB { get; set; }
        public string TypeSpeed { get; set; }
    }
}
