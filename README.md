# Unity Save Manager

Unity Save Manager is a lightweight Windows application (built with C# & .NET 8 WinForms) designed to automatically detect your Unity games and manage their save files. Built specifically for gamers and developers dealing with games that store their saves in the obscure `LocalAppDataLow` directory.

Instead of manually digging around in `%USERPROFILE%\AppData\LocalLow\...` whenever you want to backup or restore a save file, Unity Save Manager detects installed games, surfaces their hidden save data, and lets you back them up directly alongside the game's installation folder.

## Features

- **Automatic Game Detection:** Point it to your games directory and it will automatically find all Unity games by scanning for `app.info` files within `_Data` folders.
- **Smart Metadata Extraction:** It extracts the correct Company and Product names required to locate the specific save file directories inside the `AppData\LocalLow` folder.
- **One-Click Backups:** You can easily back up the save data for one or multiple games. Backups are neatly organized in a `[GameName]_savefiles` folder right next to your game installation.
- **Safe Restorations:** Restore your game saves with a single click. A backup manifest tracking file counts and sizes ensures your saves are safely restored.
- **Save Insights:** Instantly view whether a game has saves, if it's currently backed up, and the storage footprint for both.

## Prerequisites

- **OS:** Windows Only (The current version only supports Windows)
- **Framework:** [.NET 8.0 SDK / Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- **Platform:** Targets `net8.0-windows`

## Compile & Build

You can compile the application from source:

```bat
cd UnitySaveManager
dotnet build -c Release
```
*(Or simply run the included `build.bat` if it provides the publishing steps.)*

## How to Use

1. **Launch the Application**.
2. **Scan Your Games:** Click the `Browse...` button and select the root directory where you install your games (e.g., `D:\Games\`). Then click the **Scan** button.
3. **Backup Your Saves:** 
   - Select games you want to back up from the list.
   - Click `Backup Selected` to place a copy of their save data safely next to your game folder.
   - Alternatively, use `Backup All` for a peace-of-mind full backup!
4. **Restore Your Saves:** 
   - Did you mess up your run? Or reinstalling the game? Select the game and click `Restore Selected`. The tool will replace your current save with the backed up data.

## For Contributors

If you'd like to dive into the codebase, the structure is clean and minimal:

- `MainForm.cs` - Handles the application UI, pagination, asynchronous operations, and path persistence.
- `SaveManager.cs` - The core engine for scanning directories for `app.info` files, safely copying directory trees, and dealing with manifestations.
- `GameInfo.cs` - Model holding metadata paths, automatic resolving of %USERPROFILE% constraints, and size formatting.
- `Manifest.cs` - JSON manifestation formatting to keep track of backup sizes, counts, and timestamps.
