using System;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SystemMonitor
{
    [Serializable]
    public class ConfigData
    {
        public bool ShowCpu { get; set; } = true;
        public bool ShowGpu { get; set; } = true;
        public bool ShowRam { get; set; } = true;
        public bool AlwaysOnTop { get; set; } = false;

        // Temperature toggles
        public bool ShowCpuTemp { get; set; } = false;
        public bool ShowGpuTemp { get; set; } = false;

        // New display mode toggles
        // If both false => graphs + text
        public bool DisplayGraphsOnly { get; set; } = false;
        public bool DisplayTextOnly { get; set; } = false;

        // Removed ShowCpuGraph, ShowGpuGraph, ShowRamGraph as obsolete

        public Color BackgroundColor { get; set; } = Color.Black;
        public Color CpuGraphColor { get; set; } = Color.Green;
        public Color GpuGraphColor { get; set; } = Color.Red;
        public Color RamGraphColor { get; set; } = Color.Blue;
        public Color TextColor { get; set; } = Color.White;
        public Color ValueColor { get; set; } = Color.Yellow;

        // New properties to remember window size and location
        public Size? WindowSize { get; set; } = null;
        public Point? WindowLocation { get; set; } = null;

        private static string ConfigPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.dat");

        public void Save()
        {
            using (FileStream fs = new FileStream(ConfigPath, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, this);
            }
        }

        public static ConfigData Load()
        {
            if (!File.Exists(ConfigPath))
                return new ConfigData();

            using (FileStream fs = new FileStream(ConfigPath, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                try
                {
                    return (ConfigData)formatter.Deserialize(fs);
                }
                catch
                {
                    // If corrupted or incompatible config file, return default config
                    return new ConfigData();
                }
            }
        }
    }
}
