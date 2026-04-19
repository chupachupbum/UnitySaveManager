namespace UnitySaveManager;

public partial class MainForm : Form
{
    private List<GameInfo> _games = new();
    private readonly string _configPath;
    
    // Pagination state
    private int _currentPage = 0;
    private int _pageSize = 15;

    public MainForm()
    {
        InitializeComponent();

        // Config file next to the executable
        _configPath = Path.Combine(AppContext.BaseDirectory, "config.ini");

        // Wire up events
        btnBrowse.Click += BtnBrowse_Click;
        btnScan.Click += BtnScan_Click;
        btnBackupSelected.Click += BtnBackupSelected_Click;
        btnBackupAll.Click += BtnBackupAll_Click;
        btnRestoreSelected.Click += BtnRestoreSelected_Click;
        btnRestoreAll.Click += BtnRestoreAll_Click;
        
        btnPrevPage.Click += BtnPrevPage_Click;
        btnNextPage.Click += BtnNextPage_Click;

        // Load last used path
        LoadLastPath();

        Log("Unity Save Manager ready.", Color.FromArgb(100, 180, 255));
        Log("Select a game root directory and click Scan to find Unity games.", Color.FromArgb(140, 140, 140));
    }

    // ========== Event Handlers ==========

    private void BtnBrowse_Click(object? sender, EventArgs e)
    {
        using var dialog = new FolderBrowserDialog
        {
            Description = "Select the root folder containing Unity games",
            UseDescriptionForTitle = true,
            ShowNewFolderButton = false
        };

        // Start from current path if it exists
        if (Directory.Exists(txtPath.Text))
            dialog.InitialDirectory = txtPath.Text;

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            txtPath.Text = dialog.SelectedPath;
            SaveLastPath(dialog.SelectedPath);
        }
    }

    private void BtnScan_Click(object? sender, EventArgs e)
    {
        var rootPath = txtPath.Text.Trim();
        if (string.IsNullOrEmpty(rootPath) || !Directory.Exists(rootPath))
        {
            MessageBox.Show("Please select a valid directory.", "Invalid Path",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        SaveLastPath(rootPath);
        Log($"Scanning: {rootPath} ...", Color.FromArgb(100, 180, 255));

        btnScan.Enabled = false;
        btnScan.Text = "Scanning...";

        // Run scan on background thread to keep UI responsive
        Task.Run(() =>
        {
            var games = SaveManager.ScanForGames(rootPath);
            Invoke(() =>
            {
                _games = games;
                _currentPage = 0;
                RefreshGameList();
                btnScan.Enabled = true;
                btnScan.Text = "🔍 Scan";
                Log($"Found {games.Count} Unity game(s).", Color.FromArgb(100, 220, 100));
            });
        });
    }

    private void BtnBackupSelected_Click(object? sender, EventArgs e)
    {
        var selected = GetSelectedGames();
        if (selected.Count == 0)
        {
            MessageBox.Show("Please select one or more games to back up.", "No Selection",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        RunOperation("Backup", selected, game => SaveManager.Backup(game));
    }

    private void BtnBackupAll_Click(object? sender, EventArgs e)
    {
        var gamesWithSaves = _games.Where(g => g.HasSaves).ToList();
        if (gamesWithSaves.Count == 0)
        {
            MessageBox.Show("No games with save data found.", "Nothing to Backup",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirm = MessageBox.Show(
            $"Backup saves for {gamesWithSaves.Count} game(s)?",
            "Confirm Backup All",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        if (confirm == DialogResult.Yes)
            RunOperation("Backup", gamesWithSaves, game => SaveManager.Backup(game));
    }

    private void BtnRestoreSelected_Click(object? sender, EventArgs e)
    {
        var selected = GetSelectedGames();
        if (selected.Count == 0)
        {
            MessageBox.Show("Please select one or more games to restore.", "No Selection",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var withBackups = selected.Where(g => g.HasBackup).ToList();
        if (withBackups.Count == 0)
        {
            MessageBox.Show("None of the selected games have backups.", "No Backups",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var confirm = MessageBox.Show(
            $"Restore saves for {withBackups.Count} game(s)? This will overwrite current saves.",
            "Confirm Restore",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

        if (confirm == DialogResult.Yes)
            RunOperation("Restore", withBackups, game => SaveManager.Restore(game));
    }

    private void BtnRestoreAll_Click(object? sender, EventArgs e)
    {
        var gamesWithBackups = _games.Where(g => g.HasBackup).ToList();
        if (gamesWithBackups.Count == 0)
        {
            MessageBox.Show("No games with backups found.", "Nothing to Restore",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirm = MessageBox.Show(
            $"Restore saves for {gamesWithBackups.Count} game(s)? This will overwrite current saves.",
            "Confirm Restore All",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

        if (confirm == DialogResult.Yes)
            RunOperation("Restore", gamesWithBackups, game => SaveManager.Restore(game));
    }

    private void BtnPrevPage_Click(object? sender, EventArgs e)
    {
        if (_currentPage > 0)
        {
            _currentPage--;
            RefreshGameList();
        }
    }

    private void BtnNextPage_Click(object? sender, EventArgs e)
    {
        int totalPages = Math.Max(1, (int)Math.Ceiling(_games.Count / (double)_pageSize));
        if (_currentPage < totalPages - 1)
        {
            _currentPage++;
            RefreshGameList();
        }
    }

    // ========== Core Helpers ==========

    private void RunOperation(string operationName, List<GameInfo> games, Func<GameInfo, BackupResult> action)
    {
        SetButtonsEnabled(false);
        Log($"Starting {operationName} for {games.Count} game(s)...", Color.FromArgb(100, 180, 255));

        Task.Run(() =>
        {
            int successCount = 0;
            int failCount = 0;

            foreach (var game in games)
            {
                Invoke(() => Log($"  {operationName}: {game.GameName}...", Color.FromArgb(180, 180, 180)));

                var result = action(game);

                Invoke(() =>
                {
                    if (result.Success)
                    {
                        Log($"  ✓ {result.Message}", Color.FromArgb(100, 220, 100));
                        successCount++;
                    }
                    else
                    {
                        Log($"  ✗ {result.Message}", Color.FromArgb(255, 100, 100));
                        failCount++;
                    }
                });
            }

            Invoke(() =>
            {
                Log($"{operationName} complete: {successCount} succeeded, {failCount} failed.",
                    failCount > 0 ? Color.FromArgb(255, 180, 80) : Color.FromArgb(100, 220, 100));
                RefreshGameList();
                SetButtonsEnabled(true);
            });
        });
    }

    private List<GameInfo> GetSelectedGames()
    {
        var selected = new List<GameInfo>();
        foreach (DataGridViewRow row in gridGames.SelectedRows)
        {
            if (row.Tag is GameInfo game)
                selected.Add(game);
        }
        return selected;
    }

    private void RefreshGameList()
    {
        gridGames.Rows.Clear();
        
        int totalPages = Math.Max(1, (int)Math.Ceiling(_games.Count / (double)_pageSize));
        if (_currentPage >= totalPages) _currentPage = Math.Max(0, totalPages - 1);
        
        lblPage.Text = $"Page {_currentPage + 1} of {totalPages}";
        
        btnPrevPage.Enabled = _currentPage > 0;
        btnNextPage.Enabled = _currentPage < totalPages - 1;

        var pageGames = _games.Skip(_currentPage * _pageSize).Take(_pageSize).ToList();

        foreach (var game in pageGames)
        {
            int rowIndex = gridGames.Rows.Add(
                game.GameName,
                game.CompanyName,
                game.SaveStatus,
                game.BackupStatus,
                game.SaveSizeText
            );

            var row = gridGames.Rows[rowIndex];
            row.Tag = game;

            // Color the save status
            if (game.HasSaves)
                row.Cells[2].Style.ForeColor = Color.FromArgb(100, 220, 100);
            else
                row.Cells[2].Style.ForeColor = Color.FromArgb(255, 100, 100);

            // Color the backup status
            if (game.HasBackup)
                row.Cells[3].Style.ForeColor = Color.FromArgb(100, 180, 255);
        }
        
        gridGames.ClearSelection();
    }

    private void SetButtonsEnabled(bool enabled)
    {
        btnBackupSelected.Enabled = enabled;
        btnBackupAll.Enabled = enabled;
        btnRestoreSelected.Enabled = enabled;
        btnRestoreAll.Enabled = enabled;
        btnScan.Enabled = enabled;
        
        if (!enabled)
        {
            btnPrevPage.Enabled = false;
            btnNextPage.Enabled = false;
        }
        else
        {
            int totalPages = Math.Max(1, (int)Math.Ceiling(_games.Count / (double)_pageSize));
            btnPrevPage.Enabled = _currentPage > 0;
            btnNextPage.Enabled = _currentPage < totalPages - 1;
        }
    }

    // ========== Logging ==========

    private void Log(string message, Color? color = null)
    {
        var timestamp = DateTime.Now.ToString("HH:mm:ss");
        var line = $"[{timestamp}] {message}\n";

        txtLog.SelectionStart = txtLog.TextLength;
        txtLog.SelectionLength = 0;
        txtLog.SelectionColor = color ?? Color.FromArgb(180, 200, 180);
        txtLog.AppendText(line);
        txtLog.ScrollToCaret();
    }

    // ========== Config Persistence ==========

    private void LoadLastPath()
    {
        try
        {
            if (File.Exists(_configPath))
            {
                var lines = File.ReadAllLines(_configPath);
                foreach (var line in lines)
                {
                    if (line.StartsWith("LastPath=", StringComparison.OrdinalIgnoreCase))
                    {
                        var path = line["LastPath=".Length..];
                        if (Directory.Exists(path))
                            txtPath.Text = path;
                    }
                }
            }
        }
        catch { }
    }

    private void SaveLastPath(string path)
    {
        try
        {
            File.WriteAllText(_configPath, $"LastPath={path}\n");
        }
        catch { }
    }
}
