/////////////////////////////////////////////////
// Sample Usage of CXO2 Engine                 //
// Written by CXO2 a.k.a ChronoCross           //
//                                             //
// DATE: 03/10/2013 15:39:02                   //
/////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using CXO2;
using CXO2.Core;
using CXO2.Core.Renderer;
using CXO2.Core.SoundSystem;
using CXO2.Data;
using CXO2.Parser;
using CXO2.IO;

namespace JamPlayer
{
    public partial class formMain : Form
    {

        #region --- Variables ---
        bool playing = false;
        int playingID = -1;
        List<string> fileList = new List<string>();
        List<int> playedSongs = new List<int>();
        System.Windows.Forms.Timer renderTimer = new System.Windows.Forms.Timer();
        #endregion

        #region --- Constructor ---
        // Just Ordinary Constructor
        public formMain()
        {
            InitializeComponent();

            // Load Icon from Resource
            Bitmap bmp = JamPlayer.Properties.Resources.O2jam;
            System.IntPtr icH = bmp.GetHicon();
            this.Icon = Icon.FromHandle(icH);
            notifyIcon.Icon = this.Icon;

            // Used for rendering
            renderTimer.Tick += OnRender;
            renderTimer.Interval = 1;
        }
        
        #endregion

        #region --- OnLoad ---
        // I dont know why but I more like to use Override instead the Event LOL
        protected override void OnLoad(EventArgs e)
        {
            // Keep the base load properly
            base.OnLoad(e);

            // Initializing Sound System
            if (!SoundEngine.init())
                // Something wrong with SoundSystem
                // Check FMOD DLL and / or Version
                Application.Exit();


        }
        #endregion

        #region --- OnResize ---
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (mbMinimizeToTray.Checked)
            {
                if (this.WindowState == FormWindowState.Minimized)
                {
                    this.ShowInTaskbar = false;
                    notifyIcon.Visible = true;
                }
            }
        }
        #endregion

        #region --- OnRender ---
        void OnRender(object sender, EventArgs e)
        {
            // Update Renderer
            ChartRenderer.update();

            // Update sound Engine
            SoundEngine.update();

            if (!ChartRenderer.getRunningState())
            {
                // Repeat Song
                if (rdRepeatSong.Checked)
                {
                    // Items removed
                    if (playingID == -1)
                    {
                        // Reset
                        btnPlay.Text = "Play";

                        // reset played song
                        playedSongs.Clear();
                        playingID = -1;

                        // Stop
                        playing = false;
                        renderTimer.Stop();
                        return;
                    }

                    // Stop
                    playing = false;
                    btnPlay.PerformClick();
                }
                else if (rdRepeatNone.Checked)
                {
                    // Reset
                    btnPlay.Text = "Play";

                    // reset played song
                    playedSongs.Clear();
                    playingID = -1;

                    // Stop
                    playing = false;
                }

                // reset played song
                if (playListView.Items.Count == playedSongs.Count)
                    playedSongs.Clear();

                // Shuffle Mode
                if (shuffleBox.Checked)
                {
                    // Add current song to queue played song
                    if (playListView.Items.Count > playedSongs.Count)
                    {
                        if (playingID != -1)
                            playedSongs.Add(playingID);
                    }
                    // Queue playedsongs is full, clear it
                    else
                    {
                        if (rdRepeatPlaylist.Checked)
                        {
                            playedSongs.Clear();
                        }
                        else
                        {
                            // Reset
                            btnPlay.Text = "Play";

                            // reset played song
                            playedSongs.Clear();
                            playingID = -1;

                            // Stop
                            playing = false;
                        }
                    }

                    // Randomize the play
                reshuffle:
                    playingID = new Random().Next((playListView.Items.Count - 1));

                    // Its already played get new random again
                    if (playedSongs.Contains(playingID))
                        goto reshuffle;

                    // Play it
                    playing = false;
                    btnPlay.PerformClick();
                }
                else if (rdRepeatPlaylist.Checked)
                {
                    playingID++;

                    if (playListView.Items.Count == playingID)
                        playingID = 0;
                    
                    // Stop
                    playing = false;
                    btnPlay.PerformClick();
                }
            }
        }
        #endregion

        #region --- NotifyIcon ---
        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;

            notifyIcon.Visible = false;
        }
        #endregion

        #region --- Add ---
        // Add Chart to the list
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // When Button play clicked
            try
            {
                // Create open dialog
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Multiselect = true;
                ofd.Filter = "O2Jam Chart Music|*.ojn";

                // User press cancel
                if (ofd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return;

                List<string> errorfn = new List<string>();
                foreach (string fn in ofd.FileNames)
                {
                    // Create new chart variable
                    Chart chart = new Chart();

                    // Parse Header Chart
                    if (!ChartParser.parseHeader(ref chart, fn))
                    {
                        // Failed to parse
                        errorfn.Add(new FileInfo(fn).Name);
                        continue;
                    }

                    // Add file to the list
                    fileList.Add(fn);
                    playListView.Items.Add(new ListViewItem(new string[] { chart.title, chart.artist, chart.pattern }));
                }

                // Error files
                if (errorfn.Count > 0)
                {
                    string err = "";
                    foreach (string n in errorfn)
                        err += n + "\n";

                    MessageBox.Show("Failed to parse " + errorfn.Count.ToString() + " charts: " + err + "Make sure selected file is valid OJN file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {
                // Failed to parse
                MessageBox.Show("Failed to parse chart", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region --- Remove ---
        // Remoe file from list
        private void btnRemove_Click(object sender, EventArgs e)
        {
            // Check selected Items
            if (playListView.FocusedItem == null)
                return;

            // Set current playing ID
            if (playingID == playListView.FocusedItem.Index)
                playingID = -1;

            // Remove selected file
            fileList.RemoveAt(playListView.FocusedItem.Index);
            playListView.Items.Remove(playListView.FocusedItem);
        }
        #endregion

        #region --- Play and Stop ---
        private void btnPlay_Click(object sender, EventArgs e)
        {
            // When Button play clicked
            try
            {
                // Play Button
                if (!playing)
                {
                    // Nothing to play
                    if (playListView.Items.Count == 0)
                    {
                        MessageBox.Show("Please add song first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                   
                    // Make sure there are selected song
                    if (playListView.FocusedItem == null | playListView.FocusedItem.Index < 0)
                    {
                        MessageBox.Show("Please select song that you want to play", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    

                    // Set playing ID
                    if (playingID == -1)
                        playingID = playListView.FocusedItem.Index;

                    // Create new chart variable
                    Chart chart = new Chart();
                    Chart.Modes mode = Chart.Modes.HX;

                    // Modes
                    if (rdEX.Checked)
                        mode = Chart.Modes.EX;
                    else if (rdNX.Checked)
                        mode = Chart.Modes.NX;
                    else if (rdHX.Checked)
                        mode = Chart.Modes.HX;

                    // Parse Chart
                    if (!ChartParser.parse(ref chart, fileList[playingID], mode))
                    {
                        // remove corrupted / invalid chart
                        fileList.RemoveAt(playListView.FocusedItem.Index);
                        playListView.Items.RemoveAt(playListView.FocusedItem.Index);

                        // Failed to parse
                        MessageBox.Show("Failed to parse chart", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Make sure all stopped
                    renderTimer.Stop();
                    
                    // Stop All playing Sound
                    SoundEngine.stopAllSound();

                    // Init chart renderer
                    ChartRenderer.init(chart, 4.0f, mode);

                    // Chart Renderer is playing
                    btnPlay.Text = "Stop";
                    playing = true;

                    // Update Start Tick and Start the Chart
                    ChartRenderer.updateStartTick();
                    renderTimer.Start();
                }
                else
                {
                    // Stop
                    renderTimer.Stop();
                    playing = false;

                    // Stop sound
                    SoundEngine.stopAllSound();

                    // reset played song
                    playedSongs.Clear();
                    playingID = -1;

                    // Reset
                    btnPlay.Text = "Play";

                }
            }
            catch (Exception)
            {
                // Failed to parse
                MessageBox.Show("Failed to parse chart", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region --- Menu Bar ---

        #region --- File ---

        // Open new Chart
        private void mbOpen_Click(object sender, EventArgs e)
        {
            mbAddFiles.PerformClick();
        }

        // Add new Chart
        private void mbAddFiles_Click(object sender, EventArgs e)
        {
            btnAdd.PerformClick();
        }

        // Add Chart from folder
        private void mbAddFolder_Click(object sender, EventArgs e)
        {
            // Folder browser
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowNewFolderButton = false;
            fbd.Description = "Please select O2Jam Music Folder";

            if (fbd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            List<string> errorfn = new List<string>();
            foreach (string fn in Directory.GetFiles(fbd.SelectedPath, "*.ojn", SearchOption.AllDirectories))
            {
                // Create new chart variable
                Chart chart = new Chart();

                // Parse Header Chart
                if (!ChartParser.parseHeader(ref chart, fn))
                {
                    // Failed to parse
                    errorfn.Add(new FileInfo(fn).Name);
                    continue;
                }

                // Add file to the list
                fileList.Add(fn);
                playListView.Items.Add(new ListViewItem(new string[] { chart.title, chart.artist, chart.pattern }));
            }

            // Error files
            if (errorfn.Count > 0)
            {
                string err = "";
                foreach (string n in errorfn)
                    err += n + "\n";

                MessageBox.Show("Failed to parse " + errorfn.Count.ToString() + " charts: " + err + "Make sure selected file is valid OJN file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void mbLoadPlaylist_Click(object sender, EventArgs e)
        {
            try
            {
                // Open playlist File
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "JamPlayer Playlist File|*.dat";

                if (ofd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return;

                // Check the Signature
                StreamMemoryEngine buffer = new StreamMemoryEngine(ofd.FileName);
                string sign = buffer.getString(3);

                if (sign != "JML")
                {
                    buffer.Free();
                    MessageBox.Show("Failed to parse playlist file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Loop and read each the line
                StringReader sr = new StringReader(Encoding.UTF8.GetString(buffer.getAllBuffer()));
                string line = sr.ReadLine();

                // Clear
                playListView.Items.Clear();
                fileList.Clear();
                playedSongs.Clear();
                playingID = -1;

                while (line != null)
                {
                    // Ignore sign
                    if (line == "JML")
                    {
                        // Next Line
                        line = sr.ReadLine();
                        continue;
                    }

                    // Check if file exist
                    if (!File.Exists(line))
                    {
                        // Next Line
                        line = sr.ReadLine();
                        continue;
                    }

                    // Create new chart variable
                    Chart chart = new Chart();

                    // Parse Header Chart
                    if (!ChartParser.parseHeader(ref chart, line))
                    {
                        // Next Line
                        line = sr.ReadLine();
                        continue;
                    }

                    // Add file to the list
                    fileList.Add(line);
                    playListView.Items.Add(new ListViewItem(new string[] { chart.title, chart.artist, chart.pattern }));

                    // Next Line
                    line = sr.ReadLine();
                }

                buffer.Free();
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to parse playlist file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void mbSavePlaylist_Click(object sender, EventArgs e)
        {
            try
            {
                // Save dialog
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "JamPlayer Playlist File|*.dat";
                sfd.FileName = "Playlist.dat";

                // Save Dialog
                if (sfd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return;

                // Write Signature
                StreamEngine buffer = new StreamEngine(sfd.FileName, FileMode.Create);
                buffer.write("JML" + Environment.NewLine);

                // Write Filename
                foreach (string fn in fileList)
                    buffer.write(fn + Environment.NewLine);

                // Dispose and finish
                buffer.Free();
                MessageBox.Show("Playlist file was successfully saved.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to save playlist file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void mbExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion

        #region --- Edit ---

        private void mbClear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure want to clear the list?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.No)
                return;

            // Clear
            playListView.Items.Clear();
            fileList.Clear();
            playedSongs.Clear();

            playingID = -1;
        }

        #endregion

        #region --- View ---

        private void mbAlwaysOnTop_Click(object sender, EventArgs e)
        {
            this.TopMost = !this.TopMost;
        }

        private void mbMinimizeToTray_Click(object sender, EventArgs e)
        {
            mbMinimizeToTray.Checked = !mbMinimizeToTray.Checked;

            if (!mbMinimizeToTray.Checked)
                notifyIcon.Visible = false;
        }

        #endregion

        #region --- Help ---

        private void mbAbout_Click(object sender, EventArgs e)
        {
            btnAbout.PerformClick();
        }

        #endregion

        #endregion

        #region --- About ---
        private void btnAbout_Click(object sender, EventArgs e)
        {
            new formAbout().ShowDialog();
        }
        #endregion

        #region --- Exit ---
        private void btnExit_Click(object sender, EventArgs e)
        {
            // It recommended to dispose chart data samples here but...
            // too lazy lol

            Application.Exit();
        }
        #endregion

        
    }
}
