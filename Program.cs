using System;
using System.Windows.Forms;

namespace SystemMonitor
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            // Load configuration before app runs
            ConfigData.Load();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
