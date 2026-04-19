using System.Text.Json;
using System.Text.Json.Serialization;

namespace UnitySaveManager;

/// <summary>
/// Manifest stored in each backup folder (_manifest.json).
/// Enables restore on a different PC by storing company/product names.
/// </summary>
public class BackupManifest
{
    [JsonPropertyName("companyName")]
    public string CompanyName { get; set; } = string.Empty;

    [JsonPropertyName("productName")]
    public string ProductName { get; set; } = string.Empty;

    [JsonPropertyName("gameName")]
    public string GameName { get; set; } = string.Empty;

    [JsonPropertyName("backedUpAt")]
    public DateTime BackedUpAt { get; set; }

    [JsonPropertyName("originalSavePath")]
    public string OriginalSavePath { get; set; } = string.Empty;

    [JsonPropertyName("fileCount")]
    public int FileCount { get; set; }

    [JsonPropertyName("totalSizeBytes")]
    public long TotalSizeBytes { get; set; }

    public const string FileName = "_manifest.json";

    /// <summary>Write manifest to the backup directory.</summary>
    public void Save(string backupDir)
    {
        var path = Path.Combine(backupDir, FileName);
        var json = JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        File.WriteAllText(path, json);
    }

    /// <summary>Read manifest from a backup directory. Returns null if not found.</summary>
    public static BackupManifest? Load(string backupDir)
    {
        var path = Path.Combine(backupDir, FileName);
        if (!File.Exists(path)) return null;
        try
        {
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<BackupManifest>(json);
        }
        catch
        {
            return null;
        }
    }
}
