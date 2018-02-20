namespace O2MusicBox
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.MainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.FileMenu = new System.Windows.Forms.MenuItem();
            this.OpenMenu = new System.Windows.Forms.MenuItem();
            this.MenuSeparator1 = new System.Windows.Forms.MenuItem();
            this.AddFileMenu = new System.Windows.Forms.MenuItem();
            this.AddFolderMenu = new System.Windows.Forms.MenuItem();
            this.MenuSeparator2 = new System.Windows.Forms.MenuItem();
            this.SavePlaylistMenu = new System.Windows.Forms.MenuItem();
            this.LoadPlaylistMenu = new System.Windows.Forms.MenuItem();
            this.GenerateOJNListMenu = new System.Windows.Forms.MenuItem();
            this.MenuSeparator3 = new System.Windows.Forms.MenuItem();
            this.MenuPreferences = new System.Windows.Forms.MenuItem();
            this.MenuSeparator4 = new System.Windows.Forms.MenuItem();
            this.ExitMenu = new System.Windows.Forms.MenuItem();
            this.EditMenu = new System.Windows.Forms.MenuItem();
            this.MenuFind = new System.Windows.Forms.MenuItem();
            this.MenuSeparator5 = new System.Windows.Forms.MenuItem();
            this.MenuCut = new System.Windows.Forms.MenuItem();
            this.MenuCopy = new System.Windows.Forms.MenuItem();
            this.MenuPaste = new System.Windows.Forms.MenuItem();
            this.MenuDelete = new System.Windows.Forms.MenuItem();
            this.MenuSeparator6 = new System.Windows.Forms.MenuItem();
            this.MenuSelectAll = new System.Windows.Forms.MenuItem();
            this.MenuSeparator7 = new System.Windows.Forms.MenuItem();
            this.MenuClear = new System.Windows.Forms.MenuItem();
            this.ViewMenu = new System.Windows.Forms.MenuItem();
            this.AlwaysOnTopMenu = new System.Windows.Forms.MenuItem();
            this.MinimizeMenu = new System.Windows.Forms.MenuItem();
            this.PlaybackMenu = new System.Windows.Forms.MenuItem();
            this.PlayMenu = new System.Windows.Forms.MenuItem();
            this.PauseMenu = new System.Windows.Forms.MenuItem();
            this.StopMenu = new System.Windows.Forms.MenuItem();
            this.MenuSeparator8 = new System.Windows.Forms.MenuItem();
            this.NextTrackMenu = new System.Windows.Forms.MenuItem();
            this.PreviousTrackMenu = new System.Windows.Forms.MenuItem();
            this.MenuSeparator9 = new System.Windows.Forms.MenuItem();
            this.ShuffleMenu = new System.Windows.Forms.MenuItem();
            this.RenderMenu = new System.Windows.Forms.MenuItem();
            this.RenderToFileMenu = new System.Windows.Forms.MenuItem();
            this.RenderSelectedMenu = new System.Windows.Forms.MenuItem();
            this.MenuSeparator10 = new System.Windows.Forms.MenuItem();
            this.RenderAllMenu = new System.Windows.Forms.MenuItem();
            this.HelpMenu = new System.Windows.Forms.MenuItem();
            this.GithubMenu = new System.Windows.Forms.MenuItem();
            this.AboutMenu = new System.Windows.Forms.MenuItem();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblArtist = new System.Windows.Forms.Label();
            this.lblPattern = new System.Windows.Forms.Label();
            this.GroupList = new System.Windows.Forms.GroupBox();
            this.BtnClear = new System.Windows.Forms.Button();
            this.MusicList = new System.Windows.Forms.ListView();
            this.ColumnTitle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnArtist = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnPattern = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.BtnRemove = new System.Windows.Forms.Button();
            this.BtnAdd = new System.Windows.Forms.Button();
            this.GroupUtilities = new System.Windows.Forms.GroupBox();
            this.BtnRenderToFile = new System.Windows.Forms.Button();
            this.BtnSaveCover = new System.Windows.Forms.Button();
            this.BtnSaveThumbnail = new System.Windows.Forms.Button();
            this.GroupPlayback = new System.Windows.Forms.GroupBox();
            this.BtnNext = new System.Windows.Forms.Button();
            this.BtnPrevious = new System.Windows.Forms.Button();
            this.RepeatBox = new System.Windows.Forms.ComboBox();
            this.ShuffleBox = new System.Windows.Forms.CheckBox();
            this.DifficultyBox = new System.Windows.Forms.ComboBox();
            this.BtnPlay = new System.Windows.Forms.Button();
            this.BtnPause = new System.Windows.Forms.Button();
            this.BtnStop = new System.Windows.Forms.Button();
            this.BtnExit = new System.Windows.Forms.Button();
            this.BtnAbout = new System.Windows.Forms.Button();
            this.NotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.BtnEditMeta = new System.Windows.Forms.Button();
            this.lblPlayingOffset = new System.Windows.Forms.Label();
            this.StatusStrip = new System.Windows.Forms.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ThumbnailBox = new System.Windows.Forms.PictureBox();
            this.GroupList.SuspendLayout();
            this.GroupUtilities.SuspendLayout();
            this.GroupPlayback.SuspendLayout();
            this.StatusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ThumbnailBox)).BeginInit();
            this.SuspendLayout();
            // 
            // MainMenu
            // 
            this.MainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.FileMenu,
            this.EditMenu,
            this.ViewMenu,
            this.PlaybackMenu,
            this.RenderMenu,
            this.HelpMenu});
            // 
            // FileMenu
            // 
            this.FileMenu.Index = 0;
            this.FileMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.OpenMenu,
            this.MenuSeparator1,
            this.AddFileMenu,
            this.AddFolderMenu,
            this.MenuSeparator2,
            this.SavePlaylistMenu,
            this.LoadPlaylistMenu,
            this.MenuSeparator3,
            this.GenerateOJNListMenu,
            this.MenuPreferences,
            this.MenuSeparator4,
            this.ExitMenu});
            this.FileMenu.Text = "File";
            // 
            // OpenMenu
            // 
            this.OpenMenu.Index = 0;
            this.OpenMenu.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
            this.OpenMenu.Text = "Open...";
            this.OpenMenu.Click += new System.EventHandler(this.OnOpenMenuClick);
            // 
            // MenuSeparator1
            // 
            this.MenuSeparator1.Index = 1;
            this.MenuSeparator1.Text = "-";
            // 
            // AddFileMenu
            // 
            this.AddFileMenu.Index = 2;
            this.AddFileMenu.Shortcut = System.Windows.Forms.Shortcut.CtrlN;
            this.AddFileMenu.Text = "Add Files...";
            this.AddFileMenu.Click += new System.EventHandler(this.OnBtnAddClick);
            // 
            // AddFolderMenu
            // 
            this.AddFolderMenu.Index = 3;
            this.AddFolderMenu.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftN;
            this.AddFolderMenu.Text = "Add Folder...";
            this.AddFolderMenu.Click += new System.EventHandler(this.OnAddFolderMenuClick);
            // 
            // MenuSeparator2
            // 
            this.MenuSeparator2.Index = 4;
            this.MenuSeparator2.Text = "-";
            // 
            // SavePlaylistMenu
            // 
            this.SavePlaylistMenu.Enabled = false;
            this.SavePlaylistMenu.Index = 5;
            this.SavePlaylistMenu.Text = "Save Playlist";
            this.SavePlaylistMenu.Click += new System.EventHandler(this.OnSavePlaylistMenuClick);
            // 
            // LoadPlaylistMenu
            // 
            this.LoadPlaylistMenu.Index = 6;
            this.LoadPlaylistMenu.Text = "Load Playlist";
            this.LoadPlaylistMenu.Click += new System.EventHandler(this.OnLoadPlaylistMenuClick);
            // 
            // GenerateOJNListMenu
            // 
            this.GenerateOJNListMenu.Enabled = false;
            this.GenerateOJNListMenu.Index = 8;
            this.GenerateOJNListMenu.Text = "Generate OJNList";
            this.GenerateOJNListMenu.Click += new System.EventHandler(this.OnGenerateOJNListMenuClick);
            // 
            // MenuSeparator3
            // 
            this.MenuSeparator3.Index = 7;
            this.MenuSeparator3.Text = "-";
            // 
            // MenuPreferences
            // 
            this.MenuPreferences.Index = 9;
            this.MenuPreferences.Text = "Preferences..";
            this.MenuPreferences.Visible = false;
            // 
            // MenuSeparator4
            // 
            this.MenuSeparator4.Index = 10;
            this.MenuSeparator4.Text = "-";
            // 
            // ExitMenu
            // 
            this.ExitMenu.Index = 11;
            this.ExitMenu.Shortcut = System.Windows.Forms.Shortcut.AltF4;
            this.ExitMenu.Text = "Exit";
            this.ExitMenu.Click += new System.EventHandler(this.OnBtnExitClick);
            // 
            // EditMenu
            // 
            this.EditMenu.Index = 1;
            this.EditMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.MenuFind,
            this.MenuSeparator5,
            this.MenuCut,
            this.MenuCopy,
            this.MenuPaste,
            this.MenuDelete,
            this.MenuSeparator6,
            this.MenuSelectAll,
            this.MenuSeparator7,
            this.MenuClear});
            this.EditMenu.Text = "Edit";
            this.EditMenu.Visible = false;
            // 
            // MenuFind
            // 
            this.MenuFind.Index = 0;
            this.MenuFind.Text = "Find..";
            // 
            // MenuSeparator5
            // 
            this.MenuSeparator5.Index = 1;
            this.MenuSeparator5.Text = "-";
            // 
            // MenuCut
            // 
            this.MenuCut.Enabled = false;
            this.MenuCut.Index = 2;
            this.MenuCut.Text = "Cut";
            // 
            // MenuCopy
            // 
            this.MenuCopy.Enabled = false;
            this.MenuCopy.Index = 3;
            this.MenuCopy.Text = "Copy";
            // 
            // MenuPaste
            // 
            this.MenuPaste.Enabled = false;
            this.MenuPaste.Index = 4;
            this.MenuPaste.Text = "Paste";
            // 
            // MenuDelete
            // 
            this.MenuDelete.Enabled = false;
            this.MenuDelete.Index = 5;
            this.MenuDelete.Text = "Delete";
            this.MenuDelete.Click += new System.EventHandler(this.OnBtnRemoveClick);
            // 
            // MenuSeparator6
            // 
            this.MenuSeparator6.Index = 6;
            this.MenuSeparator6.Text = "-";
            // 
            // MenuSelectAll
            // 
            this.MenuSelectAll.Index = 7;
            this.MenuSelectAll.Text = "Select All";
            this.MenuSelectAll.Click += new System.EventHandler(this.OnSelectAllMenuClick);
            // 
            // MenuSeparator7
            // 
            this.MenuSeparator7.Index = 8;
            this.MenuSeparator7.Text = "-";
            // 
            // MenuClear
            // 
            this.MenuClear.Index = 9;
            this.MenuClear.Text = "Clear Playlists";
            this.MenuClear.Click += new System.EventHandler(this.OnBtnClearClick);
            // 
            // ViewMenu
            // 
            this.ViewMenu.Index = 2;
            this.ViewMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.AlwaysOnTopMenu,
            this.MinimizeMenu});
            this.ViewMenu.Text = "View";
            // 
            // AlwaysOnTopMenu
            // 
            this.AlwaysOnTopMenu.Index = 0;
            this.AlwaysOnTopMenu.Text = "Always on the Top";
            this.AlwaysOnTopMenu.Click += new System.EventHandler(this.OnAlwaysOnTopMenuClick);
            // 
            // MinimizeMenu
            // 
            this.MinimizeMenu.Index = 1;
            this.MinimizeMenu.Text = "Minimize to System Tray";
            this.MinimizeMenu.Click += new System.EventHandler(this.OnMinimizeMenuClick);
            // 
            // PlaybackMenu
            // 
            this.PlaybackMenu.Enabled = false;
            this.PlaybackMenu.Index = 3;
            this.PlaybackMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.PlayMenu,
            this.PauseMenu,
            this.StopMenu,
            this.MenuSeparator8,
            this.NextTrackMenu,
            this.PreviousTrackMenu,
            this.MenuSeparator9,
            this.ShuffleMenu});
            this.PlaybackMenu.Text = "Playback";
            // 
            // PlayMenu
            // 
            this.PlayMenu.Index = 0;
            this.PlayMenu.Text = "Play";
            this.PlayMenu.Click += new System.EventHandler(this.OnBtnPlayClick);
            // 
            // PauseMenu
            // 
            this.PauseMenu.Index = 1;
            this.PauseMenu.Text = "Pause";
            this.PauseMenu.Click += new System.EventHandler(this.OnBtnPauseClick);
            // 
            // StopMenu
            // 
            this.StopMenu.Index = 2;
            this.StopMenu.Text = "Stop";
            this.StopMenu.Click += new System.EventHandler(this.OnBtnStopClick);
            // 
            // MenuSeparator8
            // 
            this.MenuSeparator8.Index = 3;
            this.MenuSeparator8.Text = "-";
            // 
            // NextTrackMenu
            // 
            this.NextTrackMenu.Index = 4;
            this.NextTrackMenu.Text = "Next Track";
            this.NextTrackMenu.Click += new System.EventHandler(this.OnBtnNextClick);
            // 
            // PreviousTrackMenu
            // 
            this.PreviousTrackMenu.Index = 5;
            this.PreviousTrackMenu.Text = "Previous Track";
            this.PreviousTrackMenu.Click += new System.EventHandler(this.OnBtnPreviousClick);
            // 
            // MenuSeparator9
            // 
            this.MenuSeparator9.Index = 6;
            this.MenuSeparator9.Text = "-";
            // 
            // ShuffleMenu
            // 
            this.ShuffleMenu.Index = 7;
            this.ShuffleMenu.Text = "Shuffle";
            this.ShuffleMenu.Click += new System.EventHandler(this.OnShuffleMenuClick);
            // 
            // RenderMenu
            // 
            this.RenderMenu.Enabled = false;
            this.RenderMenu.Index = 4;
            this.RenderMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.RenderToFileMenu,
            this.RenderSelectedMenu,
            this.MenuSeparator10,
            this.RenderAllMenu});
            this.RenderMenu.Text = "Render";
            // 
            // RenderToFileMenu
            // 
            this.RenderToFileMenu.Enabled = false;
            this.RenderToFileMenu.Index = 0;
            this.RenderToFileMenu.Text = "Render to File..";
            this.RenderToFileMenu.Click += new System.EventHandler(this.OnBtnRenderToFileClick);
            // 
            // RenderSelectedMenu
            // 
            this.RenderSelectedMenu.Enabled = false;
            this.RenderSelectedMenu.Index = 1;
            this.RenderSelectedMenu.Text = "Render selected charts..";
            this.RenderSelectedMenu.Click += new System.EventHandler(this.OnRenderSelectedMenuClick);
            // 
            // MenuSeparator10
            // 
            this.MenuSeparator10.Index = 2;
            this.MenuSeparator10.Text = "-";
            // 
            // RenderAllMenu
            // 
            this.RenderAllMenu.Index = 3;
            this.RenderAllMenu.Text = "Render all..";
            this.RenderAllMenu.Click += new System.EventHandler(this.OnRenderAllMenuClick);
            // 
            // HelpMenu
            // 
            this.HelpMenu.Index = 5;
            this.HelpMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.GithubMenu,
            this.AboutMenu});
            this.HelpMenu.Text = "Help";
            // 
            // GithubMenu
            // 
            this.GithubMenu.Index = 0;
            this.GithubMenu.Text = "Github Repository";
            this.GithubMenu.Click += new System.EventHandler(this.OnGithubMenuClick);
            // 
            // AboutMenu
            // 
            this.AboutMenu.Index = 1;
            this.AboutMenu.Text = "About";
            this.AboutMenu.Click += new System.EventHandler(this.OnAboutMenuClick);
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(682, 32);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(222, 17);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Ready to play!";
            // 
            // lblArtist
            // 
            this.lblArtist.Location = new System.Drawing.Point(682, 48);
            this.lblArtist.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblArtist.Name = "lblArtist";
            this.lblArtist.Size = new System.Drawing.Size(222, 17);
            this.lblArtist.TabIndex = 2;
            this.lblArtist.Text = "Load a file or folder to begin!";
            // 
            // lblPattern
            // 
            this.lblPattern.Location = new System.Drawing.Point(682, 65);
            this.lblPattern.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPattern.Name = "lblPattern";
            this.lblPattern.Size = new System.Drawing.Size(222, 17);
            this.lblPattern.TabIndex = 3;
            // 
            // GroupList
            // 
            this.GroupList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.GroupList.Controls.Add(this.BtnClear);
            this.GroupList.Controls.Add(this.MusicList);
            this.GroupList.Controls.Add(this.BtnRemove);
            this.GroupList.Controls.Add(this.BtnAdd);
            this.GroupList.Location = new System.Drawing.Point(13, 13);
            this.GroupList.Margin = new System.Windows.Forms.Padding(4);
            this.GroupList.Name = "GroupList";
            this.GroupList.Padding = new System.Windows.Forms.Padding(4);
            this.GroupList.Size = new System.Drawing.Size(560, 436);
            this.GroupList.TabIndex = 5;
            this.GroupList.TabStop = false;
            this.GroupList.Text = "List";
            // 
            // BtnClear
            // 
            this.BtnClear.Location = new System.Drawing.Point(402, 388);
            this.BtnClear.Margin = new System.Windows.Forms.Padding(4);
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.Size = new System.Drawing.Size(150, 40);
            this.BtnClear.TabIndex = 15;
            this.BtnClear.Text = "Clear";
            this.BtnClear.UseVisualStyleBackColor = true;
            this.BtnClear.Click += new System.EventHandler(this.OnBtnClearClick);
            // 
            // MusicList
            // 
            this.MusicList.BackColor = System.Drawing.SystemColors.Window;
            this.MusicList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnTitle,
            this.ColumnArtist,
            this.ColumnPattern});
            this.MusicList.FullRowSelect = true;
            this.MusicList.Location = new System.Drawing.Point(8, 23);
            this.MusicList.Margin = new System.Windows.Forms.Padding(4);
            this.MusicList.Name = "MusicList";
            this.MusicList.Size = new System.Drawing.Size(544, 357);
            this.MusicList.TabIndex = 0;
            this.MusicList.UseCompatibleStateImageBehavior = false;
            this.MusicList.View = System.Windows.Forms.View.Details;
            this.MusicList.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.OnMusicListColumnClick);
            this.MusicList.SelectedIndexChanged += new System.EventHandler(this.OnMusicListSelectedIndexChange);
            this.MusicList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnMusicListKeyDown);
            this.MusicList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.OnMusicListMouseDoubleClick);
            // 
            // ColumnTitle
            // 
            this.ColumnTitle.Text = "Title";
            this.ColumnTitle.Width = 210;
            // 
            // ColumnArtist
            // 
            this.ColumnArtist.Text = "Artist";
            this.ColumnArtist.Width = 107;
            // 
            // ColumnPattern
            // 
            this.ColumnPattern.Text = "Pattern";
            this.ColumnPattern.Width = 108;
            // 
            // BtnRemove
            // 
            this.BtnRemove.Location = new System.Drawing.Point(110, 388);
            this.BtnRemove.Margin = new System.Windows.Forms.Padding(4);
            this.BtnRemove.Name = "BtnRemove";
            this.BtnRemove.Size = new System.Drawing.Size(94, 40);
            this.BtnRemove.TabIndex = 14;
            this.BtnRemove.Text = "Remove";
            this.BtnRemove.UseVisualStyleBackColor = true;
            this.BtnRemove.Click += new System.EventHandler(this.OnBtnRemoveClick);
            // 
            // BtnAdd
            // 
            this.BtnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnAdd.Location = new System.Drawing.Point(8, 388);
            this.BtnAdd.Margin = new System.Windows.Forms.Padding(4);
            this.BtnAdd.Name = "BtnAdd";
            this.BtnAdd.Size = new System.Drawing.Size(94, 40);
            this.BtnAdd.TabIndex = 13;
            this.BtnAdd.Text = "Add";
            this.BtnAdd.UseVisualStyleBackColor = true;
            this.BtnAdd.Click += new System.EventHandler(this.OnBtnAddClick);
            // 
            // GroupUtilities
            // 
            this.GroupUtilities.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupUtilities.Controls.Add(this.BtnRenderToFile);
            this.GroupUtilities.Controls.Add(this.BtnSaveCover);
            this.GroupUtilities.Controls.Add(this.BtnSaveThumbnail);
            this.GroupUtilities.Enabled = false;
            this.GroupUtilities.Location = new System.Drawing.Point(595, 280);
            this.GroupUtilities.Margin = new System.Windows.Forms.Padding(4);
            this.GroupUtilities.Name = "GroupUtilities";
            this.GroupUtilities.Padding = new System.Windows.Forms.Padding(4);
            this.GroupUtilities.Size = new System.Drawing.Size(314, 108);
            this.GroupUtilities.TabIndex = 11;
            this.GroupUtilities.TabStop = false;
            this.GroupUtilities.Text = "Utilities";
            // 
            // BtnRenderToFile
            // 
            this.BtnRenderToFile.Location = new System.Drawing.Point(8, 63);
            this.BtnRenderToFile.Margin = new System.Windows.Forms.Padding(4);
            this.BtnRenderToFile.Name = "BtnRenderToFile";
            this.BtnRenderToFile.Size = new System.Drawing.Size(290, 34);
            this.BtnRenderToFile.TabIndex = 14;
            this.BtnRenderToFile.Text = "Render to File";
            this.BtnRenderToFile.UseVisualStyleBackColor = true;
            this.BtnRenderToFile.Click += new System.EventHandler(this.OnBtnRenderToFileClick);
            // 
            // BtnSaveCover
            // 
            this.BtnSaveCover.Location = new System.Drawing.Point(157, 21);
            this.BtnSaveCover.Margin = new System.Windows.Forms.Padding(4);
            this.BtnSaveCover.Name = "BtnSaveCover";
            this.BtnSaveCover.Size = new System.Drawing.Size(141, 34);
            this.BtnSaveCover.TabIndex = 13;
            this.BtnSaveCover.Text = "Save Cover Art";
            this.BtnSaveCover.UseVisualStyleBackColor = true;
            this.BtnSaveCover.Click += new System.EventHandler(this.OnBtnSaveCoverClick);
            // 
            // BtnSaveThumbnail
            // 
            this.BtnSaveThumbnail.Location = new System.Drawing.Point(8, 21);
            this.BtnSaveThumbnail.Margin = new System.Windows.Forms.Padding(4);
            this.BtnSaveThumbnail.Name = "BtnSaveThumbnail";
            this.BtnSaveThumbnail.Size = new System.Drawing.Size(141, 34);
            this.BtnSaveThumbnail.TabIndex = 1;
            this.BtnSaveThumbnail.Text = "Save Thumbnail";
            this.BtnSaveThumbnail.UseVisualStyleBackColor = true;
            this.BtnSaveThumbnail.Click += new System.EventHandler(this.OnBtnSaveThumbnailClick);
            // 
            // GroupPlayback
            // 
            this.GroupPlayback.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupPlayback.Controls.Add(this.BtnNext);
            this.GroupPlayback.Controls.Add(this.BtnPrevious);
            this.GroupPlayback.Controls.Add(this.RepeatBox);
            this.GroupPlayback.Controls.Add(this.ShuffleBox);
            this.GroupPlayback.Controls.Add(this.DifficultyBox);
            this.GroupPlayback.Controls.Add(this.BtnPlay);
            this.GroupPlayback.Controls.Add(this.BtnPause);
            this.GroupPlayback.Controls.Add(this.BtnStop);
            this.GroupPlayback.Enabled = false;
            this.GroupPlayback.Location = new System.Drawing.Point(595, 127);
            this.GroupPlayback.Margin = new System.Windows.Forms.Padding(4);
            this.GroupPlayback.Name = "GroupPlayback";
            this.GroupPlayback.Padding = new System.Windows.Forms.Padding(4);
            this.GroupPlayback.Size = new System.Drawing.Size(314, 145);
            this.GroupPlayback.TabIndex = 12;
            this.GroupPlayback.TabStop = false;
            this.GroupPlayback.Text = "Playback";
            // 
            // BtnNext
            // 
            this.BtnNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnNext.Location = new System.Drawing.Point(247, 26);
            this.BtnNext.Margin = new System.Windows.Forms.Padding(4);
            this.BtnNext.Name = "BtnNext";
            this.BtnNext.Size = new System.Drawing.Size(51, 42);
            this.BtnNext.TabIndex = 3;
            this.BtnNext.Text = "⏭";
            this.BtnNext.UseVisualStyleBackColor = true;
            this.BtnNext.Click += new System.EventHandler(this.OnBtnNextClick);
            // 
            // BtnPrevious
            // 
            this.BtnPrevious.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnPrevious.Location = new System.Drawing.Point(12, 26);
            this.BtnPrevious.Margin = new System.Windows.Forms.Padding(4);
            this.BtnPrevious.Name = "BtnPrevious";
            this.BtnPrevious.Size = new System.Drawing.Size(51, 42);
            this.BtnPrevious.TabIndex = 4;
            this.BtnPrevious.Text = "⏮";
            this.BtnPrevious.UseVisualStyleBackColor = true;
            this.BtnPrevious.Click += new System.EventHandler(this.OnBtnPreviousClick);
            // 
            // RepeatBox
            // 
            this.RepeatBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RepeatBox.FormattingEnabled = true;
            this.RepeatBox.Items.AddRange(new object[] {
            "Repeat: None",
            "Repeat: Song",
            "Repeat: Playlist"});
            this.RepeatBox.Location = new System.Drawing.Point(99, 108);
            this.RepeatBox.Margin = new System.Windows.Forms.Padding(4);
            this.RepeatBox.Name = "RepeatBox";
            this.RepeatBox.Size = new System.Drawing.Size(199, 24);
            this.RepeatBox.TabIndex = 6;
            // 
            // ShuffleBox
            // 
            this.ShuffleBox.AutoSize = true;
            this.ShuffleBox.Location = new System.Drawing.Point(12, 113);
            this.ShuffleBox.Margin = new System.Windows.Forms.Padding(4);
            this.ShuffleBox.Name = "ShuffleBox";
            this.ShuffleBox.Size = new System.Drawing.Size(74, 21);
            this.ShuffleBox.TabIndex = 7;
            this.ShuffleBox.Text = "Shuffle";
            this.ShuffleBox.UseVisualStyleBackColor = true;
            this.ShuffleBox.CheckedChanged += new System.EventHandler(this.OnShuffleBoxCheckedChange);
            // 
            // DifficultyBox
            // 
            this.DifficultyBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DifficultyBox.FormattingEnabled = true;
            this.DifficultyBox.Items.AddRange(new object[] {
            "[EX] Easy",
            "[NX] Normal",
            "[HX] Hard"});
            this.DifficultyBox.Location = new System.Drawing.Point(12, 75);
            this.DifficultyBox.Margin = new System.Windows.Forms.Padding(4);
            this.DifficultyBox.Name = "DifficultyBox";
            this.DifficultyBox.Size = new System.Drawing.Size(286, 24);
            this.DifficultyBox.TabIndex = 5;
            // 
            // BtnPlay
            // 
            this.BtnPlay.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnPlay.Location = new System.Drawing.Point(129, 26);
            this.BtnPlay.Margin = new System.Windows.Forms.Padding(4);
            this.BtnPlay.Name = "BtnPlay";
            this.BtnPlay.Size = new System.Drawing.Size(51, 42);
            this.BtnPlay.TabIndex = 0;
            this.BtnPlay.Text = "▶";
            this.BtnPlay.UseVisualStyleBackColor = true;
            this.BtnPlay.Click += new System.EventHandler(this.OnBtnPlayClick);
            // 
            // BtnPause
            // 
            this.BtnPause.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnPause.Location = new System.Drawing.Point(188, 26);
            this.BtnPause.Margin = new System.Windows.Forms.Padding(4);
            this.BtnPause.Name = "BtnPause";
            this.BtnPause.Size = new System.Drawing.Size(51, 42);
            this.BtnPause.TabIndex = 1;
            this.BtnPause.Text = "||";
            this.BtnPause.UseVisualStyleBackColor = true;
            this.BtnPause.Click += new System.EventHandler(this.OnBtnPauseClick);
            // 
            // BtnStop
            // 
            this.BtnStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnStop.Location = new System.Drawing.Point(71, 26);
            this.BtnStop.Margin = new System.Windows.Forms.Padding(4);
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.Size = new System.Drawing.Size(51, 42);
            this.BtnStop.TabIndex = 2;
            this.BtnStop.Text = "■";
            this.BtnStop.UseVisualStyleBackColor = true;
            this.BtnStop.Click += new System.EventHandler(this.OnBtnStopClick);
            // 
            // BtnExit
            // 
            this.BtnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnExit.Location = new System.Drawing.Point(756, 399);
            this.BtnExit.Margin = new System.Windows.Forms.Padding(4);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(153, 50);
            this.BtnExit.TabIndex = 17;
            this.BtnExit.Text = "Exit";
            this.BtnExit.UseVisualStyleBackColor = true;
            this.BtnExit.Click += new System.EventHandler(this.OnBtnExitClick);
            // 
            // BtnAbout
            // 
            this.BtnAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnAbout.Location = new System.Drawing.Point(595, 399);
            this.BtnAbout.Margin = new System.Windows.Forms.Padding(4);
            this.BtnAbout.Name = "BtnAbout";
            this.BtnAbout.Size = new System.Drawing.Size(153, 50);
            this.BtnAbout.TabIndex = 16;
            this.BtnAbout.Text = "About";
            this.BtnAbout.UseVisualStyleBackColor = true;
            this.BtnAbout.Click += new System.EventHandler(this.OnAboutMenuClick);
            // 
            // NotifyIcon
            // 
            this.NotifyIcon.BalloonTipText = "The player is minimized to system tray";
            this.NotifyIcon.BalloonTipTitle = "O2MusicBox";
            this.NotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("NotifyIcon.Icon")));
            this.NotifyIcon.Text = "O2MusicBox";
            this.NotifyIcon.DoubleClick += new System.EventHandler(this.OnNotifyIconDoubleClick);
            // 
            // BtnEditMeta
            // 
            this.BtnEditMeta.Enabled = false;
            this.BtnEditMeta.Location = new System.Drawing.Point(596, 121);
            this.BtnEditMeta.Margin = new System.Windows.Forms.Padding(4);
            this.BtnEditMeta.Name = "BtnEditMeta";
            this.BtnEditMeta.Size = new System.Drawing.Size(313, 30);
            this.BtnEditMeta.TabIndex = 18;
            this.BtnEditMeta.Text = "Chart Metadata";
            this.BtnEditMeta.UseVisualStyleBackColor = true;
            this.BtnEditMeta.Visible = false;
            // 
            // lblPlayingOffset
            // 
            this.lblPlayingOffset.Location = new System.Drawing.Point(682, 95);
            this.lblPlayingOffset.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPlayingOffset.Name = "lblPlayingOffset";
            this.lblPlayingOffset.Size = new System.Drawing.Size(222, 17);
            this.lblPlayingOffset.TabIndex = 19;
            this.lblPlayingOffset.Text = "--:-- / --:--";
            // 
            // StatusStrip
            // 
            this.StatusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel});
            this.StatusStrip.Location = new System.Drawing.Point(0, 461);
            this.StatusStrip.Name = "StatusStrip";
            this.StatusStrip.Size = new System.Drawing.Size(935, 25);
            this.StatusStrip.TabIndex = 20;
            this.StatusStrip.Text = "Testing";
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(50, 20);
            this.StatusLabel.Text = "Ready";
            // 
            // ThumbnailBox
            // 
            this.ThumbnailBox.Image = global::O2MusicBox.Properties.Resources.no_image;
            this.ThumbnailBox.Location = new System.Drawing.Point(598, 33);
            this.ThumbnailBox.Margin = new System.Windows.Forms.Padding(4);
            this.ThumbnailBox.Name = "ThumbnailBox";
            this.ThumbnailBox.Size = new System.Drawing.Size(80, 80);
            this.ThumbnailBox.TabIndex = 0;
            this.ThumbnailBox.TabStop = false;
            this.ThumbnailBox.Click += new System.EventHandler(this.OnThumbnailBoxClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(935, 486);
            this.Controls.Add(this.StatusStrip);
            this.Controls.Add(this.lblPlayingOffset);
            this.Controls.Add(this.BtnEditMeta);
            this.Controls.Add(this.BtnExit);
            this.Controls.Add(this.BtnAbout);
            this.Controls.Add(this.GroupPlayback);
            this.Controls.Add(this.GroupUtilities);
            this.Controls.Add(this.GroupList);
            this.Controls.Add(this.lblPattern);
            this.Controls.Add(this.lblArtist);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.ThumbnailBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Menu = this.MainMenu;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "O2Jam Music Box";
            this.Load += new System.EventHandler(this.OnMainFormLoad);
            this.SizeChanged += new System.EventHandler(this.OnMainFormSizeChanged);
            this.GroupList.ResumeLayout(false);
            this.GroupUtilities.ResumeLayout(false);
            this.GroupPlayback.ResumeLayout(false);
            this.GroupPlayback.PerformLayout();
            this.StatusStrip.ResumeLayout(false);
            this.StatusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ThumbnailBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MainMenu MainMenu;
        private System.Windows.Forms.MenuItem FileMenu;
        private System.Windows.Forms.MenuItem OpenMenu;
        private System.Windows.Forms.MenuItem MenuSeparator1;
        private System.Windows.Forms.MenuItem AddFileMenu;
        private System.Windows.Forms.MenuItem AddFolderMenu;
        private System.Windows.Forms.MenuItem MenuSeparator2;
        private System.Windows.Forms.MenuItem ExitMenu;
        private System.Windows.Forms.MenuItem EditMenu;
        private System.Windows.Forms.MenuItem MenuClear;
        private System.Windows.Forms.MenuItem PlaybackMenu;
        private System.Windows.Forms.MenuItem PlayMenu;
        private System.Windows.Forms.MenuItem PauseMenu;
        private System.Windows.Forms.MenuItem StopMenu;
        private System.Windows.Forms.MenuItem ShuffleMenu;
        private System.Windows.Forms.MenuItem HelpMenu;
        private System.Windows.Forms.MenuItem AboutMenu;
        private System.Windows.Forms.PictureBox ThumbnailBox;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblArtist;
        private System.Windows.Forms.Label lblPattern;
        private System.Windows.Forms.GroupBox GroupList;
        private System.Windows.Forms.ListView MusicList;
        private System.Windows.Forms.ColumnHeader ColumnArtist;
        private System.Windows.Forms.ColumnHeader ColumnTitle;
        private System.Windows.Forms.GroupBox GroupUtilities;
        private System.Windows.Forms.Button BtnRenderToFile;
        private System.Windows.Forms.Button BtnSaveCover;
        private System.Windows.Forms.Button BtnSaveThumbnail;
        private System.Windows.Forms.GroupBox GroupPlayback;
        private System.Windows.Forms.ComboBox RepeatBox;
        private System.Windows.Forms.CheckBox ShuffleBox;
        private System.Windows.Forms.ComboBox DifficultyBox;
        private System.Windows.Forms.Button BtnPlay;
        private System.Windows.Forms.Button BtnPause;
        private System.Windows.Forms.Button BtnStop;
        private System.Windows.Forms.Button BtnAdd;
        private System.Windows.Forms.Button BtnRemove;
        private System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.Button BtnAbout;
        private System.Windows.Forms.MenuItem LoadPlaylistMenu;
        private System.Windows.Forms.MenuItem MenuSeparator3;
        private System.Windows.Forms.MenuItem ViewMenu;
        private System.Windows.Forms.MenuItem AlwaysOnTopMenu;
        private System.Windows.Forms.MenuItem MinimizeMenu;
        private System.Windows.Forms.NotifyIcon NotifyIcon;
        private System.Windows.Forms.ColumnHeader ColumnPattern;
        private System.Windows.Forms.MenuItem GenerateOJNListMenu;
        private System.Windows.Forms.MenuItem MenuPreferences;
        private System.Windows.Forms.MenuItem MenuSeparator4;
        private System.Windows.Forms.MenuItem MenuFind;
        private System.Windows.Forms.MenuItem MenuSeparator5;
        private System.Windows.Forms.MenuItem MenuCut;
        private System.Windows.Forms.MenuItem MenuCopy;
        private System.Windows.Forms.MenuItem MenuPaste;
        private System.Windows.Forms.MenuItem MenuDelete;
        private System.Windows.Forms.MenuItem MenuSeparator6;
        private System.Windows.Forms.MenuItem MenuSelectAll;
        private System.Windows.Forms.MenuItem MenuSeparator7;
        private System.Windows.Forms.MenuItem RenderMenu;
        private System.Windows.Forms.MenuItem RenderToFileMenu;
        private System.Windows.Forms.MenuItem RenderSelectedMenu;
        private System.Windows.Forms.Button BtnClear;
        private System.Windows.Forms.Button BtnEditMeta;
        private System.Windows.Forms.MenuItem MenuSeparator8;
        private System.Windows.Forms.Button BtnNext;
        private System.Windows.Forms.Button BtnPrevious;
        private System.Windows.Forms.Label lblPlayingOffset;
        private System.Windows.Forms.MenuItem NextTrackMenu;
        private System.Windows.Forms.MenuItem PreviousTrackMenu;
        private System.Windows.Forms.MenuItem MenuSeparator9;
        private System.Windows.Forms.StatusStrip StatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.MenuItem MenuSeparator10;
        private System.Windows.Forms.MenuItem RenderAllMenu;
        private System.Windows.Forms.MenuItem GithubMenu;
        private System.Windows.Forms.MenuItem SavePlaylistMenu;
    }
}

