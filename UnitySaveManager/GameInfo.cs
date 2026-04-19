namespace UnitySaveManager;

/// <summary>
/// Represents a detected Unity game with its metadata and path information.
/// </summary>
public class GameInfo
{
    /// <summary>Game name derived from the _Data folder (e.g., "Lizardwomen 2")</summary>
    public string GameName { get; set; } = string.Empty;

    /// <summary>Company name from app.info line 1 (e.g., "Horny Capybara")</summary>
    public string CompanyName { get; set; } = string.Empty;

    /// <summary>Product name from app.info line 2 (e.g., "Lizardwomen 2")</summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>Full path to the game's root folder (e.g., D:\Games\Unity\Lizardwomen 2)</summary>
    public string GamePath { get; set; } = string.Empty;

    /// <summary>Full path to the _Data folder containing app.info</summary>
    public string DataPath { get; set; } = string.Empty;

    /// <summary>
    /// Full path to the Unity save location.
    /// %USERPROFILE%\AppData\LocalLow\{CompanyName}\{ProductName}
    /// </summary>
    public string SavePath => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
        "AppData", "LocalLow", CompanyName, ProductName);

    /// <summary>
    /// Full path to the backup folder (sibling to game folder).
    /// {GamePath's parent}\{GameName}_savefiles
    /// </summary>
    public string BackupPath
    {
        get
        {
            var parent = Directory.GetParent(GamePath)?.FullName ?? GamePath;
            return Path.Combine(parent, $"{GameName}_savefiles");
        }
    }

    /// <summary>Whether save data exists in LocalLow</summary>
    public bool HasSaves => Directory.Exists(SavePath) && DirectoryHasContent(SavePath);

    /// <summary>Whether a backup already exists</summary>
    public bool HasBackup => Directory.Exists(BackupPath) && DirectoryHasContent(BackupPath);

    /// <summary>Total size of save data in bytes</summary>
    public long SaveSizeBytes
    {
        get
        {
            if (!Directory.Exists(SavePath)) return 0;
            try
            {
                return new DirectoryInfo(SavePath)
                    .EnumerateFiles("*", SearchOption.AllDirectories)
                    .Sum(f => f.Length);
            }
            catch { return 0; }
        }
    }

    /// <summary>Total size of backup data in bytes</summary>
    public long BackupSizeBytes
    {
        get
        {
            if (!Directory.Exists(BackupPath)) return 0;
            try
            {
                return new DirectoryInfo(BackupPath)
                    .EnumerateFiles("*", SearchOption.AllDirectories)
                    .Sum(f => f.Length);
            }
            catch { return 0; }
        }
    }

    /// <summary>Human-readable save size</summary>
    public string SaveSizeText => FormatSize(SaveSizeBytes);

    /// <summary>Human-readable backup size</summary>
    public string BackupSizeText => FormatSize(BackupSizeBytes);

    public string SaveStatus => HasSaves ? "✓ Saves Found" : "✗ No Saves";
    public string BackupStatus => HasBackup ? "📦 Backup Exists" : "—";

    private static bool DirectoryHasContent(string path)
    {
        try
        {
            return Directory.EnumerateFileSystemEntries(path).Any();
        }
        catch { return false; }
    }

    private static string FormatSize(long bytes)
    {
        if (bytes <= 0) return "—";
        string[] units = ["B", "KB", "MB", "GB"];
        int i = 0;
        double size = bytes;
        while (size >= 1024 && i < units.Length - 1)
        {
            size /= 1024;
            i++;
        }
        return $"{size:F1} {units[i]}";
    }
}
