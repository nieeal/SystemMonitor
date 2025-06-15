**SystemMonitor** **-** **README** **📦** **Requirements**

> • .NET Framework 4.8
>
> • Visual Studio 2019 or later • Reference DLLs:
>
> •
> [<u>OpenHardwareMonitorLib.dll</u>](https://github.com/openhardwaremonitor/openhardwaremonitor)
> • [<u>Newtonsoft.Json.dll</u>](https://www.newtonsoft.com/json)

Place the DLLs in a lib folder inside the project directory.

> **Project** **Structure**
>
> • Program.cs — App entry point
>
> • MainForm.cs — Rounded widget UI and sensor display
>
> • SettingsForm.cs — Toggle sensors and customize colors • AppConfig.cs
> — Load/save JSON settings
>
> • Resources/ — Icons: • IntelCPU.png
>
> • IntelGPU.png • GSkillRAM.png

**⚙** **Configuration**

A config file is auto-generated at runtime in the app directory:

> config.json

Contains: - Enabled/disabled elements - Color selection (graph and text)

> **How** **to** **Build** **&** **Run**
>
> 1\. Open the solution in Visual Studio
>
> 2\. Add OpenHardwareMonitorLib.dll and Newtonsoft.Json.dll to the
> project references (right-click → Add Reference)
>
> 3\. Add icons into a Resources folder and embed them in the project 4.
> Build the solution
>
> 5\. Run SystemMonitor.exe from bin/Release
>
> **Notes**
>
> • If no config exists at startup, it will be created with all widgets
> enabled and default colors:
> • Graphs: Green
> • Text: White
> • Numbers: Green
> • The window auto-sizes itself based on active items
>
>
<br><br><br>
Built to mimic OpenHardwareMonitor's widget appearance and behavior.
Fully dynamic and minimal.

Enjoy!
