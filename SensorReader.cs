using OpenHardwareMonitor.Hardware;
using System;

namespace SystemMonitor
{
    public static class SensorReader
    {
        public static ISensor FindSensor(Computer computer, HardwareType hardwareType, SensorType sensorType, Func<ISensor, bool> match)
        {
            foreach (var hardware in computer.Hardware)
            {
                if (hardware.HardwareType != hardwareType) continue;

                hardware.Update();
                foreach (var sensor in hardware.Sensors)
                {
                    if (sensor.SensorType == sensorType && match(sensor))
                    {
                        return sensor;
                    }
                }
            }
            return null;
        }

        public static ISensor FindCpuLoadSensor(Computer computer)
        {
            return FindSensor(computer, HardwareType.CPU, SensorType.Load, s => s.Name == "CPU Total");
        }

        public static ISensor FindCpuTempSensor(Computer computer)
        {
            return FindSensor(computer, HardwareType.CPU, SensorType.Temperature, s => s.Name.Contains("CPU"));
        }

        public static ISensor FindRamLoadSensor(Computer computer)
        {
            return FindSensor(computer, HardwareType.RAM, SensorType.Load, s => s.Name == "Memory");
        }

        public static ISensor FindGpuLoadSensor(Computer computer)
        {
            return FindSensor(computer, HardwareType.GpuNvidia, SensorType.Load, s => s.Name.Contains("GPU Core"))
                ?? FindSensor(computer, HardwareType.GpuAti, SensorType.Load, s => s.Name.Contains("GPU Core"));
        }

        public static ISensor FindGpuTempSensor(Computer computer)
        {
            return FindSensor(computer, HardwareType.GpuNvidia, SensorType.Temperature, s => s.Name.Contains("GPU"))
                ?? FindSensor(computer, HardwareType.GpuAti, SensorType.Temperature, s => s.Name.Contains("GPU"));
        }

        public static ISensor FindGpuRamLoadSensor(Computer computer)
        {
            return FindSensor(computer, HardwareType.GpuNvidia, SensorType.Load, s => s.Name.Contains("GPU Memory"))
                ?? FindSensor(computer, HardwareType.GpuAti, SensorType.Load, s => s.Name.Contains("GPU Memory"));
        }
    }
}
