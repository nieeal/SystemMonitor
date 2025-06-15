using System;
using System.Drawing;
using System.IO;

namespace SystemMonitor
{
    public static class ResourcesLoader
    {
        public static Image LoadImage(string name)
        {
            string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
            string path = Path.Combine(basePath, name);

            if (File.Exists(path))
            {
                return Image.FromFile(path);
            }

            return null;
        }
    }
}
