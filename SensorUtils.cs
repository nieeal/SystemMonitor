using OpenHardwareMonitor.Hardware;

namespace SystemMonitor
{
    public static class SensorUtils
    {
        public static ISensor FindSensor(IHardware hardware, SensorType type, string nameContains)
        {
            foreach (var sensor in hardware.Sensors)
            {
                if (sensor.SensorType == type && sensor.Name.Contains(nameContains))
                {
                    return sensor;
                }
            }
            return null;
        }

        public static void UpdateAllHardware(Computer computer)
        {
            foreach (var hw in computer.Hardware)
            {
                hw.Update();
                foreach (var sub in hw.SubHardware)
                {
                    sub.Update();
                }
            }
        }
    }
}
