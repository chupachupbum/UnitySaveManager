namespace UnitySaveManager;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        // ---- Top Panel: Path Selection ----
        panelTop = new Panel();
        lblPath = new Label();
        txtPath = new TextBox();
        btnBrowse = new Button();
        btnScan = new Button();

        // ---- Game List ----
        gridGames = new DataGridView();
        colGameName = new DataGridViewTextBoxColumn();
        colCompany = new DataGridViewTextBoxColumn();
        colSaveStatus = new DataGridViewTextBoxColumn();
        colBackupStatus = new DataGridViewTextBoxColumn();
        colSaveSize = new DataGridViewTextBoxColumn();

        // ---- Action Bar ----
        panelActions = new Panel();
        btnBackupSelected = new Button();
        btnBackupAll = new Button();
        btnRestoreSelected = new Button();
        btnRestoreAll = new Button();

        // Paging Controls
        btnPrevPage = new Button();
        lblPage = new Label();
        btnNextPage = new Button();

        // ---- Log Panel ----
        panelLog = new Panel();
        lblLog = new Label();
        panelLogContent = new Panel();
        txtLog = new RichTextBox();

        // ---- Splitter ----
        splitContainer = new SplitContainer();

        panelTop.SuspendLayout();
        panelActions.SuspendLayout();
        panelLog.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
        splitContainer.Panel1.SuspendLayout();
        splitContainer.Panel2.SuspendLayout();
        splitContainer.SuspendLayout();
        SuspendLayout();

        // ========== panelTop ==========
        panelTop.Size = new Size(900, 60);
        panelTop.Controls.Add(lblPath);
        panelTop.Controls.Add(txtPath);
        panelTop.Controls.Add(btnBrowse);
        panelTop.Controls.Add(btnScan);
        panelTop.Dock = DockStyle.Top;
        panelTop.Height = 60;
        panelTop.Padding = new Padding(12, 12, 12, 8);
        panelTop.BackColor = Color.FromArgb(30, 30, 30);

        // lblPath
        lblPath.Text = "Game Root:";
        lblPath.AutoSize = true;
        lblPath.Location = new Point(14, 20);
        lblPath.ForeColor = Color.FromArgb(200, 200, 200);
        lblPath.Font = new Font("Segoe UI", 9.5f);

        // txtPath
        txtPath.Location = new Point(100, 16);
        txtPath.Size = new Size(655, 28);
        txtPath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtPath.Font = new Font("Segoe UI", 9.5f);
        txtPath.BackColor = Color.FromArgb(45, 45, 45);
        txtPath.ForeColor = Color.FromArgb(220, 220, 220);
        txtPath.BorderStyle = BorderStyle.FixedSingle;

        // btnBrowse
        btnBrowse.Text = "...";
        btnBrowse.Size = new Size(35, 30);
        btnBrowse.Location = new Point(759, 14);
        btnBrowse.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnBrowse.FlatStyle = FlatStyle.Flat;
        btnBrowse.BackColor = Color.FromArgb(60, 60, 60);
        btnBrowse.ForeColor = Color.White;
        btnBrowse.FlatAppearance.BorderColor = Color.FromArgb(80, 80, 80);
        btnBrowse.Font = new Font("Segoe UI", 9f);
        btnBrowse.Cursor = Cursors.Hand;

        // btnScan
        btnScan.Text = "🔍 Scan";
        btnScan.Size = new Size(90, 30);
        btnScan.Location = new Point(798, 14);
        btnScan.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnScan.FlatStyle = FlatStyle.Flat;
        btnScan.BackColor = Color.FromArgb(0, 122, 204);
        btnScan.ForeColor = Color.White;
        btnScan.FlatAppearance.BorderSize = 0;
        btnScan.Font = new Font("Segoe UI Semibold", 9f);
        btnScan.Cursor = Cursors.Hand;

        // ========== gridGames ==========
        ((System.ComponentModel.ISupportInitialize)gridGames).BeginInit();
        gridGames.Dock = DockStyle.Fill;
        gridGames.BorderStyle = BorderStyle.None;
        gridGames.BackgroundColor = Color.FromArgb(35, 35, 35);
        gridGames.ForeColor = Color.FromArgb(220, 220, 220);
        gridGames.Font = new Font("Segoe UI", 9.5f);
        gridGames.RowHeadersVisible = false;
        gridGames.AllowUserToAddRows = false;
        gridGames.AllowUserToDeleteRows = false;
        gridGames.AllowUserToResizeRows = false;
        gridGames.ReadOnly = true;
        gridGames.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        gridGames.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        gridGames.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        gridGames.GridColor = Color.FromArgb(50, 50, 50);
        
        gridGames.EnableHeadersVisualStyles = false;
        gridGames.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 45);
        gridGames.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        gridGames.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(45, 45, 45);
        gridGames.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        gridGames.ColumnHeadersHeight = 30;

        gridGames.DefaultCellStyle.BackColor = Color.FromArgb(35, 35, 35);
        gridGames.DefaultCellStyle.ForeColor = Color.FromArgb(220, 220, 220);
        gridGames.DefaultCellStyle.SelectionBackColor = Color.FromArgb(60, 60, 60);
        gridGames.DefaultCellStyle.SelectionForeColor = Color.White;
        gridGames.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(40, 40, 40);

        colGameName.HeaderText = "Game Name";
        colGameName.FillWeight = 220;
        
        colCompany.HeaderText = "Company";
        colCompany.FillWeight = 180;
        
        colSaveStatus.HeaderText = "Save Status";
        colSaveStatus.FillWeight = 130;
        
        colBackupStatus.HeaderText = "Backup Status";
        colBackupStatus.FillWeight = 140;
        
        colSaveSize.HeaderText = "Save Size";
        colSaveSize.FillWeight = 100;

        gridGames.Columns.AddRange(new DataGridViewColumn[]
        {
            colGameName, colCompany, colSaveStatus, colBackupStatus, colSaveSize
        });

        // ========== panelActions ==========
        panelActions.Controls.Add(btnNextPage);
        panelActions.Controls.Add(lblPage);
        panelActions.Controls.Add(btnPrevPage);
        panelActions.Controls.Add(btnBackupSelected);
        panelActions.Controls.Add(btnBackupAll);
        panelActions.Controls.Add(btnRestoreSelected);
        panelActions.Controls.Add(btnRestoreAll);
        panelActions.Dock = DockStyle.Bottom;
        panelActions.Height = 55;
        panelActions.Padding = new Padding(12, 10, 12, 10);
        panelActions.BackColor = Color.FromArgb(30, 30, 30);

        // Action buttons styling
        var actionButtons = new[] { btnBackupSelected, btnBackupAll, btnRestoreSelected, btnRestoreAll };
        var actionTexts = new[] { "💾 Backup Selected", "💾 Backup All", "📥 Restore Selected", "📥 Restore All" };
        var actionColors = new[]
        {
            Color.FromArgb(0, 122, 204),   // blue
            Color.FromArgb(0, 153, 76),    // green
            Color.FromArgb(180, 120, 0),   // amber
            Color.FromArgb(150, 80, 0)     // dark amber
        };

        for (int i = 0; i < actionButtons.Length; i++)
        {
            actionButtons[i].Text = actionTexts[i];
            actionButtons[i].Size = new Size(150, 35);
            actionButtons[i].Location = new Point(14 + i * 160, 10);
            actionButtons[i].FlatStyle = FlatStyle.Flat;
            actionButtons[i].BackColor = actionColors[i];
            actionButtons[i].ForeColor = Color.White;
            actionButtons[i].FlatAppearance.BorderSize = 0;
            actionButtons[i].Font = new Font("Segoe UI Semibold", 9f);
            actionButtons[i].Cursor = Cursors.Hand;
        }

        // btnNextPage
        btnNextPage.Text = "Next >";
        btnNextPage.Size = new Size(70, 30);
        btnNextPage.Location = new Point(810, 12);
        btnNextPage.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnNextPage.FlatStyle = FlatStyle.Flat;
        btnNextPage.BackColor = Color.FromArgb(60, 60, 60);
        btnNextPage.ForeColor = Color.White;
        btnNextPage.FlatAppearance.BorderColor = Color.FromArgb(80, 80, 80);
        btnNextPage.Font = new Font("Segoe UI", 9f);
        btnNextPage.Cursor = Cursors.Hand;

        // lblPage
        lblPage.Text = "Page 1 of 1";
        lblPage.AutoSize = false;
        lblPage.Size = new Size(80, 30);
        lblPage.Location = new Point(725, 12);
        lblPage.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblPage.ForeColor = Color.FromArgb(200, 200, 200);
        lblPage.Font = new Font("Segoe UI", 9f);
        lblPage.TextAlign = ContentAlignment.MiddleCenter;

        // btnPrevPage
        btnPrevPage.Text = "< Prev";
        btnPrevPage.Size = new Size(70, 30);
        btnPrevPage.Location = new Point(650, 12);
        btnPrevPage.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnPrevPage.FlatStyle = FlatStyle.Flat;
        btnPrevPage.BackColor = Color.FromArgb(60, 60, 60);
        btnPrevPage.ForeColor = Color.White;
        btnPrevPage.FlatAppearance.BorderColor = Color.FromArgb(80, 80, 80);
        btnPrevPage.Font = new Font("Segoe UI", 9f);
        btnPrevPage.Cursor = Cursors.Hand;

        // ========== panelLog ==========
        panelLog.Controls.Add(panelLogContent);
        panelLog.Controls.Add(lblLog);
        panelLog.Dock = DockStyle.Fill;
        panelLog.Padding = new Padding(8, 4, 8, 8);
        panelLog.BackColor = Color.FromArgb(25, 25, 25);

        // lblLog
        lblLog.Text = "Log";
        lblLog.Dock = DockStyle.Top;
        lblLog.Height = 22;
        lblLog.ForeColor = Color.FromArgb(140, 140, 140);
        lblLog.Font = new Font("Segoe UI", 8.5f);
        lblLog.Padding = new Padding(4, 4, 0, 4);

        // panelLogContent
        panelLogContent.Dock = DockStyle.Fill;
        panelLogContent.BackColor = Color.FromArgb(20, 20, 20);
        panelLogContent.Padding = new Padding(4, 4, 4, 4);
        panelLogContent.Controls.Add(txtLog);

        // txtLog
        txtLog.Dock = DockStyle.Fill;
        txtLog.ReadOnly = true;
        txtLog.BackColor = Color.FromArgb(20, 20, 20);
        txtLog.ForeColor = Color.FromArgb(180, 200, 180);
        txtLog.Font = new Font("Cascadia Mono", 9f, FontStyle.Regular);
        txtLog.BorderStyle = BorderStyle.None;
        txtLog.ScrollBars = RichTextBoxScrollBars.Vertical;

        // ========== splitContainer ==========
        splitContainer.Dock = DockStyle.Fill;
        splitContainer.Orientation = Orientation.Horizontal;
        splitContainer.SplitterDistance = 350;
        splitContainer.SplitterWidth = 6;
        splitContainer.BackColor = Color.FromArgb(50, 50, 50);
        splitContainer.Panel1.BackColor = Color.FromArgb(35, 35, 35);
        splitContainer.Panel2.BackColor = Color.FromArgb(25, 25, 25);

        splitContainer.Panel1.Controls.Add(gridGames);
        ((System.ComponentModel.ISupportInitialize)gridGames).EndInit();
        splitContainer.Panel1.Controls.Add(panelActions);
        splitContainer.Panel2.Controls.Add(panelLog);

        // ========== MainForm ==========
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(900, 620);
        Controls.Add(splitContainer);
        Controls.Add(panelTop);
        Text = "Unity Save Manager";
        BackColor = Color.FromArgb(30, 30, 30);
        StartPosition = FormStartPosition.CenterScreen;
        MinimumSize = new Size(750, 500);

        panelTop.ResumeLayout(false);
        panelTop.PerformLayout();
        panelActions.ResumeLayout(false);
        panelLog.ResumeLayout(false);
        splitContainer.Panel1.ResumeLayout(false);
        splitContainer.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
        splitContainer.ResumeLayout(false);
        ResumeLayout(false);
    }

    // Top panel
    private Panel panelTop;
    private Label lblPath;
    private TextBox txtPath;
    private Button btnBrowse;
    private Button btnScan;

    // Game list
    private DataGridView gridGames;
    private DataGridViewTextBoxColumn colGameName;
    private DataGridViewTextBoxColumn colCompany;
    private DataGridViewTextBoxColumn colSaveStatus;
    private DataGridViewTextBoxColumn colBackupStatus;
    private DataGridViewTextBoxColumn colSaveSize;

    // Paging
    private Button btnPrevPage;
    private Label lblPage;
    private Button btnNextPage;

    // Action buttons
    private Panel panelActions;
    private Button btnBackupSelected;
    private Button btnBackupAll;
    private Button btnRestoreSelected;
    private Button btnRestoreAll;

    // Log panel
    private Panel panelLog;
    private Panel panelLogContent;
    private Label lblLog;
    private RichTextBox txtLog;

    // Layout
    private SplitContainer splitContainer;
}
