using System;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SystemMonitor
{
    public class ConfigData
    {
        private static readonly string ConfigFileName = "config.json";

        public float Opacity { get; set; } = 1.0f; // 0 to 1
        public int UpdateIntervalMs { get; set; } = 1000; // update every 1s by default

        public bool AlwaysOnTop { get; set; } = false;
        public bool AutoUpdate { get; set; } = true;

        public bool ShowCpu { get; set; } = true;
        public bool ShowGpu { get; set; } = true;
        public bool ShowRam { get; set; } = true;

        public Color CpuColor { get; set; } = Color.Lime;
        public Color GpuColor { get; set; } = Color.Red;
        public Color RamColor { get; set; } = Color.Blue;

        public Color CpuTextColor { get; set; } = Color.White;
        public Color GpuTextColor { get; set; } = Color.White;
        public Color RamTextColor { get; set; } = Color.White;

        public static ConfigData Current { get; private set; } = Load();

        public static ConfigData Load()
        {
            try
            {
                if (!File.Exists(ConfigFileName))
                    return new ConfigData();

                string json = File.ReadAllText(ConfigFileName);
                var options = new JsonSerializerOptions
                {
                    Converters = { new ColorJsonConverter() },
                    AllowTrailingCommas = true,
                };
                var config = JsonSerializer.Deserialize<ConfigData>(json, options);

                // Validate values
                if (config.Opacity < 0f || config.Opacity > 1f) config.Opacity = 1.0f;
                if (config.UpdateIntervalMs < 100) config.UpdateIntervalMs = 1000;

                return config;
            }
            catch (Exception ex)
            {
                // Log error and return default config
                Console.Error.WriteLine($"Failed to load config: {ex.Message}");
                return new ConfigData();
            }
        }

        public void Save()
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Converters = { new ColorJsonConverter() },
                };
                string json = JsonSerializer.Serialize(this, options);
                File.WriteAllText(ConfigFileName, json);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save config: {ex.Message}");
            }
        }
    }

    public class ColorJsonConverter : JsonConverter<Color>
    {
        public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var s = reader.GetString();
            return ColorTranslator.FromHtml(s);
        }

        public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
        {
            string colorStr = ColorTranslator.ToHtml(value);
            writer.WriteStringValue(colorStr);
        }
    }
}
