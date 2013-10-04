namespace JamPlayer
{
    partial class formMain
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
            this.btnAdd = new System.Windows.Forms.Button();
            this.groupDifficulty = new System.Windows.Forms.GroupBox();
            this.rdHX = new System.Windows.Forms.RadioButton();
            this.rdNX = new System.Windows.Forms.RadioButton();
            this.rdEX = new System.Windows.Forms.RadioButton();
            this.btnAbout = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.playListView = new System.Windows.Forms.ListView();
            this.columnTitle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnArtist = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnPattern = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnPlay = new System.Windows.Forms.Button();
            this.shuffleBox = new System.Windows.Forms.CheckBox();
            this.repeatGroupBox = new System.Windows.Forms.GroupBox();
            this.rdRepeatNone = new System.Windows.Forms.RadioButton();
            this.rdRepeatPlaylist = new System.Windows.Forms.RadioButton();
            this.rdRepeatSong = new System.Windows.Forms.RadioButton();
            this.mainMenu = new System.Windows.Forms.MainMenu(this.components);
            this.mbFile = new System.Windows.Forms.MenuItem();
            this.mbOpen = new System.Windows.Forms.MenuItem();
            this.mbSeparator1 = new System.Windows.Forms.MenuItem();
            this.mbAddFiles = new System.Windows.Forms.MenuItem();
            this.mbAddFolder = new System.Windows.Forms.MenuItem();
            this.mbSeparator2 = new System.Windows.Forms.MenuItem();
            this.mbLoadPlaylist = new System.Windows.Forms.MenuItem();
            this.mbSavePlaylist = new System.Windows.Forms.MenuItem();
            this.mbSeparator3 = new System.Windows.Forms.MenuItem();
            this.mbExit = new System.Windows.Forms.MenuItem();
            this.mbEdit = new System.Windows.Forms.MenuItem();
            this.mbClear = new System.Windows.Forms.MenuItem();
            this.mbView = new System.Windows.Forms.MenuItem();
            this.mbAlwaysOnTop = new System.Windows.Forms.MenuItem();
            this.mbHelp = new System.Windows.Forms.MenuItem();
            this.mbAbout = new System.Windows.Forms.MenuItem();
            this.groupDifficulty.SuspendLayout();
            this.repeatGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(12, 225);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(269, 35);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // groupDifficulty
            // 
            this.groupDifficulty.Controls.Add(this.rdHX);
            this.groupDifficulty.Controls.Add(this.rdNX);
            this.groupDifficulty.Controls.Add(this.rdEX);
            this.groupDifficulty.Location = new System.Drawing.Point(12, 266);
            this.groupDifficulty.Name = "groupDifficulty";
            this.groupDifficulty.Size = new System.Drawing.Size(192, 55);
            this.groupDifficulty.TabIndex = 1;
            this.groupDifficulty.TabStop = false;
            this.groupDifficulty.Text = "Difficulty";
            // 
            // rdHX
            // 
            this.rdHX.AutoSize = true;
            this.rdHX.Location = new System.Drawing.Point(133, 23);
            this.rdHX.Name = "rdHX";
            this.rdHX.Size = new System.Drawing.Size(48, 17);
            this.rdHX.TabIndex = 2;
            this.rdHX.Text = "Hard";
            this.rdHX.UseVisualStyleBackColor = true;
            // 
            // rdNX
            // 
            this.rdNX.AutoSize = true;
            this.rdNX.Location = new System.Drawing.Point(69, 23);
            this.rdNX.Name = "rdNX";
            this.rdNX.Size = new System.Drawing.Size(58, 17);
            this.rdNX.TabIndex = 1;
            this.rdNX.Text = "Normal";
            this.rdNX.UseVisualStyleBackColor = true;
            // 
            // rdEX
            // 
            this.rdEX.AutoSize = true;
            this.rdEX.Checked = true;
            this.rdEX.Location = new System.Drawing.Point(15, 23);
            this.rdEX.Name = "rdEX";
            this.rdEX.Size = new System.Drawing.Size(48, 17);
            this.rdEX.TabIndex = 0;
            this.rdEX.TabStop = true;
            this.rdEX.Text = "Easy";
            this.rdEX.UseVisualStyleBackColor = true;
            // 
            // btnAbout
            // 
            this.btnAbout.Location = new System.Drawing.Point(210, 266);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(143, 55);
            this.btnAbout.TabIndex = 2;
            this.btnAbout.Text = "About";
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(359, 266);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 55);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(287, 225);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(144, 35);
            this.btnRemove.TabIndex = 4;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // playListView
            // 
            this.playListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnTitle,
            this.columnArtist,
            this.columnPattern});
            this.playListView.FullRowSelect = true;
            this.playListView.Location = new System.Drawing.Point(12, 8);
            this.playListView.Name = "playListView";
            this.playListView.Size = new System.Drawing.Size(419, 164);
            this.playListView.TabIndex = 5;
            this.playListView.UseCompatibleStateImageBehavior = false;
            this.playListView.View = System.Windows.Forms.View.Details;
            // 
            // columnTitle
            // 
            this.columnTitle.Text = "Title";
            this.columnTitle.Width = 193;
            // 
            // columnArtist
            // 
            this.columnArtist.Text = "Artist";
            this.columnArtist.Width = 105;
            // 
            // columnPattern
            // 
            this.columnPattern.Text = "Pattern";
            this.columnPattern.Width = 97;
            // 
            // btnPlay
            // 
            this.btnPlay.Location = new System.Drawing.Point(12, 178);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(127, 35);
            this.btnPlay.TabIndex = 6;
            this.btnPlay.Text = "Play";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // shuffleBox
            // 
            this.shuffleBox.AutoSize = true;
            this.shuffleBox.Location = new System.Drawing.Point(150, 196);
            this.shuffleBox.Name = "shuffleBox";
            this.shuffleBox.Size = new System.Drawing.Size(59, 17);
            this.shuffleBox.TabIndex = 8;
            this.shuffleBox.Text = "Shuffle";
            this.shuffleBox.UseVisualStyleBackColor = true;
            // 
            // repeatGroupBox
            // 
            this.repeatGroupBox.Controls.Add(this.rdRepeatNone);
            this.repeatGroupBox.Controls.Add(this.rdRepeatPlaylist);
            this.repeatGroupBox.Controls.Add(this.rdRepeatSong);
            this.repeatGroupBox.Location = new System.Drawing.Point(215, 178);
            this.repeatGroupBox.Name = "repeatGroupBox";
            this.repeatGroupBox.Size = new System.Drawing.Size(216, 41);
            this.repeatGroupBox.TabIndex = 9;
            this.repeatGroupBox.TabStop = false;
            this.repeatGroupBox.Text = "Repeat";
            // 
            // rdRepeatNone
            // 
            this.rdRepeatNone.AutoSize = true;
            this.rdRepeatNone.Checked = true;
            this.rdRepeatNone.Location = new System.Drawing.Point(125, 17);
            this.rdRepeatNone.Name = "rdRepeatNone";
            this.rdRepeatNone.Size = new System.Drawing.Size(51, 17);
            this.rdRepeatNone.TabIndex = 2;
            this.rdRepeatNone.TabStop = true;
            this.rdRepeatNone.Text = "None";
            this.rdRepeatNone.UseVisualStyleBackColor = true;
            // 
            // rdRepeatPlaylist
            // 
            this.rdRepeatPlaylist.AutoSize = true;
            this.rdRepeatPlaylist.Location = new System.Drawing.Point(62, 17);
            this.rdRepeatPlaylist.Name = "rdRepeatPlaylist";
            this.rdRepeatPlaylist.Size = new System.Drawing.Size(57, 17);
            this.rdRepeatPlaylist.TabIndex = 1;
            this.rdRepeatPlaylist.Text = "Playlist";
            this.rdRepeatPlaylist.UseVisualStyleBackColor = true;
            // 
            // rdRepeatSong
            // 
            this.rdRepeatSong.AutoSize = true;
            this.rdRepeatSong.Location = new System.Drawing.Point(6, 17);
            this.rdRepeatSong.Name = "rdRepeatSong";
            this.rdRepeatSong.Size = new System.Drawing.Size(50, 17);
            this.rdRepeatSong.TabIndex = 0;
            this.rdRepeatSong.Text = "Song";
            this.rdRepeatSong.UseVisualStyleBackColor = true;
            // 
            // mainMenu
            // 
            this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mbFile,
            this.mbEdit,
            this.mbView,
            this.mbHelp});
            // 
            // mbFile
            // 
            this.mbFile.Index = 0;
            this.mbFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mbOpen,
            this.mbSeparator1,
            this.mbAddFiles,
            this.mbAddFolder,
            this.mbSeparator2,
            this.mbLoadPlaylist,
            this.mbSavePlaylist,
            this.mbSeparator3,
            this.mbExit});
            this.mbFile.Text = "File";
            // 
            // mbOpen
            // 
            this.mbOpen.Index = 0;
            this.mbOpen.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
            this.mbOpen.Text = "Open";
            this.mbOpen.Click += new System.EventHandler(this.mbOpen_Click);
            // 
            // mbSeparator1
            // 
            this.mbSeparator1.Index = 1;
            this.mbSeparator1.Text = "-";
            // 
            // mbAddFiles
            // 
            this.mbAddFiles.Index = 2;
            this.mbAddFiles.Shortcut = System.Windows.Forms.Shortcut.CtrlA;
            this.mbAddFiles.Text = "Add Files...";
            this.mbAddFiles.Click += new System.EventHandler(this.mbAddFiles_Click);
            // 
            // mbAddFolder
            // 
            this.mbAddFolder.Index = 3;
            this.mbAddFolder.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftA;
            this.mbAddFolder.Text = "Add Folder...";
            this.mbAddFolder.Click += new System.EventHandler(this.mbAddFolder_Click);
            // 
            // mbSeparator2
            // 
            this.mbSeparator2.Index = 4;
            this.mbSeparator2.Text = "-";
            // 
            // mbLoadPlaylist
            // 
            this.mbLoadPlaylist.Index = 5;
            this.mbLoadPlaylist.Text = "Load Playlist...";
            this.mbLoadPlaylist.Click += new System.EventHandler(this.mbLoadPlaylist_Click);
            // 
            // mbSavePlaylist
            // 
            this.mbSavePlaylist.Index = 6;
            this.mbSavePlaylist.Text = "Save Playlist...";
            this.mbSavePlaylist.Click += new System.EventHandler(this.mbSavePlaylist_Click);
            // 
            // mbSeparator3
            // 
            this.mbSeparator3.Index = 7;
            this.mbSeparator3.Text = "-";
            // 
            // mbExit
            // 
            this.mbExit.Index = 8;
            this.mbExit.Text = "Exit";
            this.mbExit.Click += new System.EventHandler(this.mbExit_Click);
            // 
            // mbEdit
            // 
            this.mbEdit.Index = 1;
            this.mbEdit.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mbClear});
            this.mbEdit.Text = "Edit";
            // 
            // mbClear
            // 
            this.mbClear.Index = 0;
            this.mbClear.Text = "Clear";
            this.mbClear.Click += new System.EventHandler(this.mbClear_Click);
            // 
            // mbView
            // 
            this.mbView.Index = 2;
            this.mbView.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mbAlwaysOnTop});
            this.mbView.Text = "View";
            // 
            // mbAlwaysOnTop
            // 
            this.mbAlwaysOnTop.Index = 0;
            this.mbAlwaysOnTop.Shortcut = System.Windows.Forms.Shortcut.AltUpArrow;
            this.mbAlwaysOnTop.Text = "Always on Top";
            this.mbAlwaysOnTop.Click += new System.EventHandler(this.mbAlwaysOnTop_Click);
            // 
            // mbHelp
            // 
            this.mbHelp.Index = 3;
            this.mbHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mbAbout});
            this.mbHelp.Text = "Help";
            // 
            // mbAbout
            // 
            this.mbAbout.Index = 0;
            this.mbAbout.Text = "About";
            this.mbAbout.Click += new System.EventHandler(this.mbAbout_Click);
            // 
            // formMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 330);
            this.Controls.Add(this.repeatGroupBox);
            this.Controls.Add(this.shuffleBox);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.playListView);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnAbout);
            this.Controls.Add(this.groupDifficulty);
            this.Controls.Add(this.btnAdd);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Menu = this.mainMenu;
            this.Name = "formMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Jam Player";
            this.groupDifficulty.ResumeLayout(false);
            this.groupDifficulty.PerformLayout();
            this.repeatGroupBox.ResumeLayout(false);
            this.repeatGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.GroupBox groupDifficulty;
        private System.Windows.Forms.RadioButton rdHX;
        private System.Windows.Forms.RadioButton rdNX;
        private System.Windows.Forms.RadioButton rdEX;
        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.ListView playListView;
        private System.Windows.Forms.ColumnHeader columnTitle;
        private System.Windows.Forms.ColumnHeader columnArtist;
        private System.Windows.Forms.ColumnHeader columnPattern;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.CheckBox shuffleBox;
        private System.Windows.Forms.GroupBox repeatGroupBox;
        private System.Windows.Forms.RadioButton rdRepeatPlaylist;
        private System.Windows.Forms.RadioButton rdRepeatSong;
        private System.Windows.Forms.RadioButton rdRepeatNone;
        private System.Windows.Forms.MainMenu mainMenu;
        private System.Windows.Forms.MenuItem mbFile;
        private System.Windows.Forms.MenuItem mbOpen;
        private System.Windows.Forms.MenuItem mbSeparator1;
        private System.Windows.Forms.MenuItem mbAddFiles;
        private System.Windows.Forms.MenuItem mbAddFolder;
        private System.Windows.Forms.MenuItem mbSeparator2;
        private System.Windows.Forms.MenuItem mbLoadPlaylist;
        private System.Windows.Forms.MenuItem mbSavePlaylist;
        private System.Windows.Forms.MenuItem mbSeparator3;
        private System.Windows.Forms.MenuItem mbExit;
        private System.Windows.Forms.MenuItem mbEdit;
        private System.Windows.Forms.MenuItem mbClear;
        private System.Windows.Forms.MenuItem mbView;
        private System.Windows.Forms.MenuItem mbAlwaysOnTop;
        private System.Windows.Forms.MenuItem mbHelp;
        private System.Windows.Forms.MenuItem mbAbout;

    }
}

