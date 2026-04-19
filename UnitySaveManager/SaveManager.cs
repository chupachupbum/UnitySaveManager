namespace UnitySaveManager;

/// <summary>
/// Core logic for scanning, backing up, and restoring Unity game saves.
/// </summary>
public static class SaveManager
{
    /// <summary>
    /// Scan a root directory for Unity games by finding *_Data\app.info files.
    /// </summary>
    public static List<GameInfo> ScanForGames(string rootPath)
    {
        var games = new List<GameInfo>();
        if (!Directory.Exists(rootPath)) return games;

        // Find all app.info files within _Data folders
        try
        {
            var appInfoFiles = Directory.EnumerateFiles(rootPath, "app.info", SearchOption.AllDirectories);
            foreach (var appInfoPath in appInfoFiles)
            {
                var dataDir = Path.GetDirectoryName(appInfoPath);
                if (dataDir == null) continue;

                var dataDirName = Path.GetFileName(dataDir);
                if (!dataDirName.EndsWith("_Data", StringComparison.OrdinalIgnoreCase))
                    continue;

                try
                {
                    var lines = File.ReadAllLines(appInfoPath);
                    if (lines.Length < 2) continue;

                    var companyName = lines[0].Trim();
                    var productName = lines[1].Trim();
                    if (string.IsNullOrEmpty(companyName) || string.IsNullOrEmpty(productName))
                        continue;

                    // Game name = _Data folder name minus "_Data" suffix
                    var gameName = dataDirName[..^"_Data".Length];

                    // Game path = parent of the _Data folder
                    var gamePath = Directory.GetParent(dataDir)?.FullName;
                    if (gamePath == null) continue;

                    games.Add(new GameInfo
                    {
                        GameName = gameName,
                        CompanyName = companyName,
                        ProductName = productName,
                        GamePath = gamePath,
                        DataPath = dataDir
                    });
                }
                catch
                {
                    // Skip files we can't read
                }
            }
        }
        catch
        {
            // Directory enumeration failed
        }

        return games.OrderBy(g => g.GameName).ToList();
    }

    /// <summary>
    /// Backup a game's save data from LocalLow to the backup folder.
    /// </summary>
    public static BackupResult Backup(GameInfo game)
    {
        var result = new BackupResult { GameName = game.GameName };

        if (!game.HasSaves)
        {
            result.Success = false;
            result.Message = $"No save data found at: {game.SavePath}";
            return result;
        }

        try
        {
            // Clean the backup directory first (mirror behavior)
            if (Directory.Exists(game.BackupPath))
            {
                // Remove old files but keep the directory
                foreach (var file in Directory.EnumerateFiles(game.BackupPath, "*", SearchOption.AllDirectories))
                {
                    File.Delete(file);
                }
                // Remove empty subdirectories
                foreach (var dir in Directory.EnumerateDirectories(game.BackupPath, "*", SearchOption.AllDirectories)
                    .OrderByDescending(d => d.Length))
                {
                    if (!Directory.EnumerateFileSystemEntries(dir).Any())
                        Directory.Delete(dir);
                }
            }
            else
            {
                Directory.CreateDirectory(game.BackupPath);
            }

            // Copy all files from save path to backup path
            int fileCount = 0;
            long totalSize = 0;
            CopyDirectory(game.SavePath, game.BackupPath, ref fileCount, ref totalSize);

            // Write manifest
            var manifest = new BackupManifest
            {
                CompanyName = game.CompanyName,
                ProductName = game.ProductName,
                GameName = game.GameName,
                BackedUpAt = DateTime.Now,
                OriginalSavePath = game.SavePath,
                FileCount = fileCount,
                TotalSizeBytes = totalSize
            };
            manifest.Save(game.BackupPath);

            result.Success = true;
            result.FileCount = fileCount;
            result.TotalSize = totalSize;
            result.Message = $"Backed up {fileCount} files ({FormatSize(totalSize)}) to {game.BackupPath}";
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Message = $"Backup failed: {ex.Message}";
        }

        return result;
    }

    /// <summary>
    /// Restore a game's save data from the backup folder to LocalLow.
    /// </summary>
    public static BackupResult Restore(GameInfo game)
    {
        var result = new BackupResult { GameName = game.GameName };

        if (!game.HasBackup)
        {
            result.Success = false;
            result.Message = $"No backup found at: {game.BackupPath}";
            return result;
        }

        // Try reading manifest for metadata
        var manifest = BackupManifest.Load(game.BackupPath);
        var targetPath = game.SavePath; // Uses current PC's USERPROFILE

        try
        {
            // Create the LocalLow directory structure if it doesn't exist
            Directory.CreateDirectory(targetPath);

            // Copy all files from backup to save path (skip _manifest.json)
            int fileCount = 0;
            long totalSize = 0;
            CopyDirectory(game.BackupPath, targetPath, ref fileCount, ref totalSize,
                skipFile: BackupManifest.FileName);

            result.Success = true;
            result.FileCount = fileCount;
            result.TotalSize = totalSize;
            result.Message = $"Restored {fileCount} files ({FormatSize(totalSize)}) to {targetPath}";
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Message = $"Restore failed: {ex.Message}";
        }

        return result;
    }

    /// <summary>
    /// Recursively copy a directory tree.
    /// </summary>
    private static void CopyDirectory(string source, string dest,
        ref int fileCount, ref long totalSize, string? skipFile = null)
    {
        Directory.CreateDirectory(dest);

        foreach (var file in Directory.EnumerateFiles(source))
        {
            var fileName = Path.GetFileName(file);
            if (skipFile != null && fileName.Equals(skipFile, StringComparison.OrdinalIgnoreCase))
                continue;

            var destFile = Path.Combine(dest, fileName);
            File.Copy(file, destFile, overwrite: true);
            var info = new FileInfo(destFile);
            fileCount++;
            totalSize += info.Length;
        }

        foreach (var dir in Directory.EnumerateDirectories(source))
        {
            var dirName = Path.GetFileName(dir);
            var destDir = Path.Combine(dest, dirName);
            CopyDirectory(dir, destDir, ref fileCount, ref totalSize, skipFile);
        }
    }

    private static string FormatSize(long bytes)
    {
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

public class BackupResult
{
    public string GameName { get; set; } = string.Empty;
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public int FileCount { get; set; }
    public long TotalSize { get; set; }
}
