using OpenHardwareMonitor.Hardware;

namespace SystemMonitor
{
    public class SensorManager
    {
        private Computer computer;

        public ISensor CpuLoad { get; private set; }
        public ISensor CpuTemp { get; private set; }
        public ISensor RamUsed { get; private set; }
        public ISensor GpuLoad { get; private set; }
        public ISensor GpuTemp { get; private set; }
        public ISensor GpuRamUsed { get; private set; }

        public SensorManager()
        {
            computer = new Computer
            {
                CPUEnabled = true,
                RAMEnabled = true,
                GPUEnabled = true
            };
            computer.Open();
            FindSensors();
        }

        private void FindSensors()
        {
            foreach (var hw in computer.Hardware)
            {
                hw.Update();
                foreach (var sensor in hw.Sensors)
                {
                    if (sensor.SensorType == SensorType.Load && sensor.Name == "CPU Total")
                        CpuLoad = sensor;
                    else if (sensor.SensorType == SensorType.Temperature && sensor.Name.Contains("CPU"))
                        CpuTemp = sensor;
                    else if (sensor.SensorType == SensorType.Load && sensor.Name == "Memory")
                        RamUsed = sensor;
                    else if (sensor.SensorType == SensorType.Load && sensor.Name.Contains("GPU Core"))
                        GpuLoad = sensor;
                    else if (sensor.SensorType == SensorType.Temperature && sensor.Name.Contains("GPU"))
                        GpuTemp = sensor;
                    else if (sensor.SensorType == SensorType.Load && sensor.Name.Contains("GPU Memory"))
                        GpuRamUsed = sensor;
                }
            }
        }

        public void Update()
        {
            foreach (var hw in computer.Hardware)
                hw.Update();
        }
    }
}
