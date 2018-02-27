using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Runtime.InteropServices;
using System.Linq;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.IO;

using Cgen;
using Cgen.Audio;

using CXO2;
using CXO2.Charting;
using CXO2.Processors;

using CXO2.Charting.O2Jam;

namespace O2MusicBox
{
    public partial class MainForm : Form
    {
        #region --- Private Fields ---
        private PlaybackState playback;
        private List<Chart> charts;
        private Task renderTask = new Task(() => { });
        #endregion

        #region --- Constructors ---
        public MainForm()
        {
            InitializeComponent();

            // Costura load assembly on demand (lazy load)
            // So we initialize this earlier to prevent heavy load when we initialize first chart
            SoundSystem.Instance.Initialize();
            SoundProcessorFactory.InstallEncoder<MP3Encoder>((stream, sampleRate, channel, own) => new MP3Encoder(stream, sampleRate, channel, own));

            playback  = new PlaybackState();
            charts = new List<Chart>();
        }
        #endregion

        #region --- Form Callbacks ---
        private void OnMainFormLoad(object sender, EventArgs e)
        {
            DifficultyBox.SelectedIndex = 2;
            RepeatBox.SelectedIndex     = 0;
            NotifyIcon.Icon             = this.Icon;
        }

        private void OnMainFormSizeChanged(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized && MinimizeMenu.Checked)
            {
                NotifyIcon.Visible = true;
                ShowInTaskbar = false;

                NotifyIcon.Text = "O2Jam Music Box";
                NotifyIcon.BalloonTipText = StatusLabel.Text;
                NotifyIcon.ShowBalloonTip(1000);

            }
            else if (WindowState != FormWindowState.Minimized)
            {
                NotifyIcon.Visible = false;
                this.ShowInTaskbar = true;
            }
        }
        #endregion

        #region --- Form Components ---
        private void OnStatusLabelTextChanged(object sender, EventArgs e)
        {
        }

        private void OnNotifyIconDoubleClick(object sender, EventArgs e)
        {
            ShowInTaskbar = true;
            NotifyIcon.Visible = false;

            WindowState = FormWindowState.Normal;
            Focus();
            BringToFront();
        }

        private void OnBtnExitClick(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion

        #region --- Music List Controls ---
        private void OnMusicListSelectedIndexChange(object sender, EventArgs e)
        {
            if ((playback.Renderer != null && playback.Renderer.IsRendering) || StatusLabel.Text.StartsWith("Initializing"))
                return;

            bool validSelection = MusicList.FocusedItem != null && MusicList.FocusedItem.Index >= 0;
            BtnEditMeta.Enabled = GroupUtilities.Enabled = RenderToFileMenu.Enabled = RenderSelectedMenu.Enabled = validSelection;
            if (validSelection)
            {
                var chart = charts[MusicList.FocusedItem.Index];
                if (playback.Renderer == null || !playback.Renderer.IsRendering)
                {
                    playback.Track = MusicList.FocusedItem.Index;

                    var header = charts[playback.Track];
                    SetLayoutInfo(header);

                    BtnSaveThumbnail.Enabled = chart.ThumbnailData != null && chart.ThumbnailData.Length > 0;
                    BtnSaveCover.Enabled     = chart.CoverArtData != null && chart.CoverArtData.Length > 0;
                }
                
            }
            else
            {
                ResetState();
            }
        }

        private void OnMusicListColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (MusicList.ListViewItemSorter == null)
                MusicList.ListViewItemSorter = new ListViewColumnSorter();

            var sorter = MusicList.ListViewItemSorter as ListViewColumnSorter;
            var chartSorter = new ChartSorter();
            chartSorter.SortType = (ChartSortType)e.Column;
            if (e.Column != sorter.SortColumn)
            {
                sorter.SortColumn = e.Column;
                chartSorter.Order = sorter.Order = SortOrder.Ascending;
            }
            else
            {
                chartSorter.Order = sorter.Order = sorter.Order == SortOrder.Descending || sorter.Order == SortOrder.None ? 
                    SortOrder.Ascending : 
                    SortOrder.Descending;
            }

            MusicList.Sort();
            charts?.Sort(chartSorter);
        }

        private void OnMusicListMouseDoubleClick(object sender, MouseEventArgs e)
        {
            BtnPlay.PerformClick();
        }

        private void OnMusicListKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                BtnPlay.PerformClick();
        }

        private void OnBtnAddClick(object sender, EventArgs e)
        {
            var openDialog = new OpenFileDialog();
            openDialog.Filter = "O2Jam Chart File|*.ojn";
            openDialog.Multiselect = true;

            if (openDialog.ShowDialog() == DialogResult.OK)
                AddChart(openDialog.FileNames);
        }

        private void OnBtnRemoveClick(object sender, EventArgs e)
        {
            if (MusicList.FocusedItem != null)
            {
                MusicList.FocusedItem.Selected = true;
                foreach (ListViewItem item in MusicList.SelectedItems)
                {
                    int index = item.Index;
                    if (index <= playback.Track && playback.Track > 0)
                        playback.Track--;

                    charts.RemoveAt(index);
                    MusicList.Items.Remove(item);
                }

                MusicList.FocusedItem = charts.Count > 0 ? MusicList.Items[playback.Track] : null;
                OnMusicListSelectedIndexChange(sender, e);

                GenerateOJNListMenu.Enabled = SavePlaylistMenu.Enabled = BtnEditMeta.Enabled = GroupUtilities.Enabled = 
                    RenderMenu.Enabled = charts.Count > 0;

                if (playback.Renderer == null || !playback.Renderer.IsRendering)
                    GroupPlayback.Enabled = PlaybackMenu.Enabled = charts.Count > 0;
            }
        }

        private void OnBtnClearClick(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure want to clear the playlist?", "Clear Playlist", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            MusicList.Items.Clear();
            charts.Clear();

            BtnEditMeta.Enabled = SavePlaylistMenu.Enabled = GroupUtilities.Enabled =
                GenerateOJNListMenu.Enabled = RenderToFileMenu.Enabled = RenderSelectedMenu.Enabled = false;

            GroupPlayback.Enabled = PlaybackMenu.Enabled = 
                playback.Renderer != null && playback.Renderer.IsRendering;
        }
        #endregion

        #region --- Playback Controls ---
        async private void OnBtnPlayClick(object sender, EventArgs e)
        {
            if (playback.Renderer != null && playback.Renderer.IsPaused)
            {
                playback.Renderer.Resume();
                return;
            }

            if (MusicList.FocusedItem == null)
            {
                if (MusicList.Items.Count > 0)
                    MusicList.FocusedItem = MusicList.Items[0];
                else
                    return;
            }

            if (playback.Renderer != null && playback.Renderer.IsRendering &&
                MusicList.FocusedItem.Index == playback.Track)
            {
                return;
            }

            Chart chart = null;
            try
            {
                BtnEditMeta.Enabled = GroupUtilities.Enabled = RenderToFileMenu.Enabled = RenderSelectedMenu.Enabled = true;
                if (playback.Renderer == null)
                {
                    var renderer = new ChartRenderer();
                    renderer.Rendering += ((send, args) => {
                        string offset = string.Format("{0} / {1}", renderer.Elapsed.ToString(@"mm\:ss"), renderer.Duration.ToString(@"mm\:ss"));
                        string status = string.Format("{0} - {1}", offset, renderer.Chart.Title);

                        lblPlayingOffset.SetTextAsync(offset);
                        StatusStrip.Invoke(new Action(() => StatusLabel.Text = renderer.IsPaused ? "Paused - " + renderer.Chart.Title : status));
                    });

                    renderer.RenderComplete += OnRenderComplete;
                    playback.Renderer = renderer;
                }
                else
                {
                    playback.Renderer.Stop();
                    await renderTask;
                }

                OpenMenu.Enabled = GroupPlayback.Enabled = PlaybackMenu.Enabled = false;
                playback.Track = MusicList.FocusedItem.Index;

                var header = charts[playback.Track];
                SetLayoutInfo(header, true);
                SetHighlightState(true);

                BtnSaveThumbnail.Enabled = header.ThumbnailData != null && header.ThumbnailData.Length > 0;
                BtnSaveCover.Enabled     = header.CoverArtData != null  && header.CoverArtData.Length > 0;

                chart = await ChartDecoder.DecodeAsync(header.Filename, DifficultyBox.SelectedIndex);
                if (chart == null)
                    throw new Exception("Invalid / unsupported chart file.");

                //foreach (var ev in chart.Events)
                //{
                //    var sample = ev as Event.Sound;
                //    if (sample != null)
                //        sample.Payload = null;
                //}
                renderTask = playback.Renderer.RenderAsync(chart);
            }
            catch (Exception ex)
            {
                StatusLabel.Text = "Ready";
                MessageBox.Show(ex.Message, "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                OpenMenu.Enabled = GroupPlayback.Enabled = PlaybackMenu.Enabled = true;
                await renderTask;
            }
        }

        async private void OnBtnStopClick(object sender, EventArgs e)
        {
            if (playback.Renderer == null || !playback.Renderer.IsRendering)
                return;
            
            playback.Renderer.Stop();
            await renderTask;

            SetHighlightState(false);
            if (charts.Count == 0 || MusicList.FocusedItem == null || MusicList.FocusedItem.Index < 0)
            {
                BtnEditMeta.Enabled = GroupUtilities.Enabled =
                RenderMenu.Enabled  = false;

                if (charts.Count == 0)
                {
                    GroupPlayback.Enabled = PlaybackMenu.Enabled = false;
                    ResetState();
                }
            }
            else
            {
                lblPlayingOffset.Text = "--:-- / --:--";
                StatusLabel.Text = "Ready";
                OnMusicListSelectedIndexChange(sender, e);
                MusicList.Focus();
            }
        }

        private void OnBtnPauseClick(object sender, EventArgs e)
        {
            playback.Renderer?.Pause();
        }

        private void OnBtnNextClick(object sender, EventArgs e)
        {
            OnBtnStopClick(sender, e);

            if (charts.Count == 0)
                return;

            if (ShuffleBox.Checked)
            {
                var randomizer = new Random();
                int index = randomizer.Next(charts.Count);
                while (index == playback.Track)
                    index = randomizer.Next(charts.Count);

                playback.Track = index;
            }
            else
            {
                if (playback.Track >= charts.Count - 1)
                    playback.Track = 0;
                else
                    playback.Track++;
            }

            MusicList.FocusedItem = MusicList.Items[playback.Track];
            BtnPlay.PerformClick();
        }

        private void OnBtnPreviousClick(object sender, EventArgs e)
        {
            OnBtnStopClick(sender, e);

            if (charts.Count == 0)
                return;

            if (ShuffleBox.Checked)
            {
                var randomizer = new Random();
                playback.Track = randomizer.Next(charts.Count);
            }
            else
            {
                if (playback.Track <= 0)
                    playback.Track = charts.Count - 1;
                else
                    playback.Track--;
            }

            MusicList.FocusedItem = MusicList.Items[playback.Track];
            BtnPlay.PerformClick();
        }

        private void OnShuffleBoxCheckedChange(object sender, EventArgs e)
        {
            ShuffleMenu.Checked = ShuffleBox.Checked;
        }
        #endregion

        #region --- Utilities Controls --
        private void OnThumbnailBoxClick(object sender, EventArgs e)
        {
            if (playback.Track >= 0 && playback.Track < charts.Count)
            {
                var chart = charts[playback.Track];
                if (chart.CoverArtData != null && chart.CoverArtData.Length > 0)
                    new ArtForm(chart).ShowDialog();
            }
        }
        
        private void OnBtnSaveThumbnailClick(object sender, EventArgs e)
        {
            try
            {
                var chart = charts[playback.Track];
                var saveDialog = new SaveFileDialog();

                saveDialog.Filter = "Bitmap Image|*.bmp";
                saveDialog.FileName = chart.Title;

                if (chart.ThumbnailData == null || chart.ThumbnailData.Length == 0)
                    throw new Exception("Invalid thumbnail image.");

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllBytes(saveDialog.FileName, chart.ThumbnailData);
                    MessageBox.Show("Thumbnail was saved successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save thumbnail.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnBtnSaveCoverClick(object sender, EventArgs e)
        {
            try
            {
                var chart = charts[playback.Track];
                var saveDialog = new SaveFileDialog();

                saveDialog.Filter = "JPEG Image|*.jpg";
                saveDialog.FileName = chart.Title;

                if (chart.CoverArtData == null || chart.CoverArtData.Length == 0)
                    throw new Exception("Invalid cover image.");

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllBytes(saveDialog.FileName, chart.CoverArtData);
                    MessageBox.Show("Cover art was saved successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save cover art.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region --- Main Menu ---

        #region --- File Menu ---
        async private void OnOpenMenuClick(object sender, EventArgs e)
        {
            var openDialog = new OpenFileDialog();
            openDialog.Filter = "O2Jam Chart File|*.ojn";
            openDialog.Multiselect = true;

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                int count = charts.Count;
                AddChart(openDialog.FileNames);

                if (charts.Count > 0 && openDialog.FileNames.Length > 0)
                {
                    try
                    {
                        var header = await ChartDecoder.DecodeHeaderAsync(openDialog.FileNames[0]);
                        var item = MusicList.Items.Cast<ListViewItem>().FirstOrDefault((listItem) => listItem.Text == header.Title);

                        MusicList.FocusedItem = item;
                        BtnPlay.PerformClick();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void OnAddFolderMenuClick(object sender, EventArgs e)
        {
            var browserDialog = new FolderBrowserDialog();
            browserDialog.Description = "Select O2Jam Music Directory.";
            browserDialog.ShowNewFolderButton = false;

            var err = new List<string>();
            if (browserDialog.ShowDialog() == DialogResult.OK)
            {
                var dir = new DirectoryInfo(browserDialog.SelectedPath);
                AddChart(dir.GetFiles("*.ojn").Select((file) => file.FullName).ToArray());
            }
        }

        private void OnSavePlaylistMenuClick(object sender, EventArgs e)
        {
            try
            {
                if (MusicList.Items.Count == 0)
                {
                    MessageBox.Show("Nothing to save, playlist is empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "O2Jam Music Box Playlist|*.o2p";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    var filenames = charts.Select((chart) => chart.Filename);
                    File.WriteAllLines(saveDialog.FileName, filenames);

                    MessageBox.Show("Playlist has been saved successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save Playlist.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnLoadPlaylistMenuClick(object sender, EventArgs e)
        {
            try
            {
                if (MusicList.Items.Count > 0)
                {
                    BtnClear.PerformClick();
                }

                OpenFileDialog openDialog = new OpenFileDialog();
                openDialog.Filter = "O2Jam Music Box Playlist|*.o2p";

                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    string[] filenames = File.ReadAllLines(openDialog.FileName);
                    AddChart(filenames);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load Playlist.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnGenerateOJNListMenuClick(object sender, EventArgs e)
        {
            var saveDialog = new SaveFileDialog();
            saveDialog.FileName = "OJNList";
            saveDialog.Filter = "O2Jam Music List|*.dat";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                LoadingForm loader = null;
                loader = new LoadingForm(async () =>
                {
                    try
                    {
                        using (var stream = new FileStream(saveDialog.FileName, FileMode.Create, FileAccess.ReadWrite))
                        using (var writer = new BinaryWriter(stream))
                        {
                            int count = 0;
                            stream.Seek(4, SeekOrigin.Begin);
                            await Task.Run(() =>
                            {
                                foreach (var chart in charts)
                                {
                                    try
                                    {
                                        loader.SetStatus(string.Format("Processing {0} - {1}..", Path.GetFileName(chart.Filename), chart.Title));
                                        loader.SetProgress(((float)count / charts.Count) * 100f);

                                        if (File.Exists(chart.Filename))
                                        {
                                            var header = File.ReadAllBytes(chart.Filename).Take(300);
                                            writer.Write(header.ToArray());
                                            count++;
                                        }
                                    }
                                    finally
                                    {
                                    }
                                }
                            });

                            loader.SetStatus("Finalizing..");
                            loader.SetProgress(100f);

                            stream.Seek(0, SeekOrigin.Begin);
                            writer.Write(count);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        loader.DialogResult = DialogResult.Abort;
                    }
                    finally
                    {
                        loader.Close();
                    }
                });

                if (loader.ShowDialog() != DialogResult.Abort)
                    MessageBox.Show("OJNList has been saved successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion

        #region --- Edit Menu ---
        private void OnSelectAllMenuClick(object sender, EventArgs e)
        {
            foreach (ListViewItem item in MusicList.Items)
            {
                item.Selected = true;
            }
        }
        #endregion

        #region --- View Menu ---
        private void OnAlwaysOnTopMenuClick(object sender, EventArgs e)
        {
            AlwaysOnTopMenu.Checked = !AlwaysOnTopMenu.Checked;
            TopMost = !TopMost;
        }

        private void OnMinimizeMenuClick(object sender, EventArgs e)
        {
            MinimizeMenu.Checked = !MinimizeMenu.Checked;
        }
        #endregion

        #region --- Playback Menu ---
        private void OnShuffleMenuClick(object sender, EventArgs e)
        {
            ShuffleMenu.Checked = !ShuffleMenu.Checked;
            ShuffleBox.Checked = ShuffleMenu.Checked;
        }
        #endregion

        #region --- Render Menu ---
        private void OnBtnRenderToFileClick(object sender, EventArgs e)
        {
            var renderForm = new RenderForm(charts[playback.Track]);
            renderForm.ShowDialog();
        }

        private void OnRenderSelectedMenuClick(object sender, EventArgs e)
        {
            if (MusicList.FocusedItem == null || MusicList.SelectedItems.Count < 0)
            {
                MessageBox.Show("You have to select charts to render.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedCharts = new List<Chart>();
            foreach (int item in MusicList.SelectedIndices)
            {
                selectedCharts.Add(charts[item]);
            }

            var renderForm = new RenderForm(selectedCharts.ToArray());
            renderForm.ShowDialog();
        }

        private void OnRenderAllMenuClick(object sender, EventArgs e)
        {
            var renderForm = new RenderForm(charts.ToArray());
            renderForm.ShowDialog();
        }
        #endregion

        #region --- Help Menu ---
        private void OnAboutMenuClick(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog();
        }

        private void OnGithubMenuClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://github.com/SirusDoma/o2musicbox");
        }
        #endregion

        #endregion

        #region --- Functions ---
        private void AddChart(string[] filenames)
        {
            bool initial = MusicList.Items.Count == 0;
            var err = new List<string>();
            MusicList.BeginUpdate();
            {
                LoadingForm loader = null;
                loader = new LoadingForm(async () =>
                {
                    float progress = 0;
                    foreach (string filename in filenames)
                    {
                        Chart chart = null;
                        try
                        {
                            if (charts.FirstOrDefault((existing) => existing.Filename == filename) == null)
                            {
                                chart = await ChartDecoder.DecodeHeaderAsync(filename, (int)OJN.Difficulty.HX);
                                charts.Add(chart);

                                var item = new ListViewItem(new string[] {
                                    chart.Title,
                                    chart.Artist,
                                    chart.Pattern,
                                    chart.Duration.ToString(@"mm\:ss")
                                });

                                MusicList.Items.Add(item);
                            }
                        }
                        catch (Exception ex)
                        {
                            err.Add(string.Format("{0}: {1}", Path.GetFileName(filename), ex.Message));
                        }

                        progress++;
                        if (loader.IsHandleCreated)
                        {
                            loader.SetStatus(string.Format("Processing {0} - {1}", Path.GetFileName(filename), chart?.Title));
                            loader.SetProgress((progress / filenames.Length) * 100f);
                        }
                    }

                    loader.Close();
                });

                loader.ShowDialog();
                if (MusicList.ListViewItemSorter == null)
                {
                    var listSorter = new ListViewColumnSorter();
                    listSorter.Order = SortOrder.Ascending;

                    MusicList.ListViewItemSorter = listSorter;
                }

                var sorter = MusicList.ListViewItemSorter as ListViewColumnSorter;
                var chartSorter = new ChartSorter();
                chartSorter.SortType = (ChartSortType)sorter.SortColumn;
                chartSorter.Order = sorter.Order;

                MusicList.Sort();
                charts?.Sort(chartSorter);
            }
            MusicList.EndUpdate();

            if (err.Count > 0)
            {
                string errfn = "";
                foreach (string fn in err)
                    errfn += fn + Environment.NewLine;

                MessageBox.Show("Failed to decode following charts: " + Environment.NewLine + errfn, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (charts.Count > 0 && initial)
            {
                playback.Track = 0;
                GenerateOJNListMenu.Enabled = SavePlaylistMenu.Enabled = 
                    RenderMenu.Enabled = OpenMenu.Enabled = 
                    GroupPlayback.Enabled = PlaybackMenu.Enabled = true;
            }
        }

        private void SetLayoutInfo(Chart header, bool initializing = false)
        {
            lblTitle.Text = header.Title;
            lblArtist.Text = header.Artist;
            lblPattern.Text = header.Pattern;
            lblPlayingOffset.Text = "--:-- / --:--";

            if (header.ThumbnailData != null && header.ThumbnailData.Length > 0)
            {
                try
                {
                    using (var stream = new MemoryStream(header.ThumbnailData))
                        ThumbnailBox.Image = new Bitmap(stream);
                }
                catch (Exception)
                {
                    ThumbnailBox.Image = Properties.Resources.no_image;
                    header.ThumbnailData = new byte[0];
                    BtnSaveThumbnail.Enabled = false;
                }
            }
            else
            {
                ThumbnailBox.Image = Properties.Resources.no_image;
            }

            if (initializing)
                StatusLabel.Text = string.Format("Initializing - {0}", header.Title);
        }

        private void ResetState()
        {
            lblTitle.Text   = "Ready to play!";
            lblArtist.Text  = "Load a file or folder to begin!";
            lblPattern.Text = string.Empty;

            lblPlayingOffset.Text = "--:-- / --:--";
            ThumbnailBox.Image = Properties.Resources.no_image;
            StatusLabel.Text = "Ready";
        }

        private void SetHighlightState(bool highlighted)
        {
            if (playback.Track >= charts.Count)
                return;

            var backColor = highlighted ? Color.FromArgb(0, 48, 120) : MusicList.BackColor;
            var foreColor = highlighted ? Color.White  : Color.Black;

            MusicList.Invoke(new Action(() =>
            {
                foreach (ListViewItem item in MusicList.Items)
                {
                    if (item.Index != playback.Track)
                    {
                        item.BackColor = MusicList.BackColor;
                        item.ForeColor = Color.Black;
                    }
                }

                MusicList.Items[playback.Track].BackColor = backColor;
                MusicList.Items[playback.Track].ForeColor = foreColor;
            }));
        }

        async private void OnRenderComplete(object sender, EventArgs e)
        {
            await renderTask;

            if (charts.Count == 0)
            {
                GroupPlayback.Invoke(new Action(() => GroupPlayback.Enabled = false));
                return;
            }

            int repeat = (int)RepeatBox.Invoke(new Func<int>(() => RepeatBox.SelectedIndex));
            int difficulty = (int)DifficultyBox.Invoke(new Func<int>(() => DifficultyBox.SelectedIndex));

            if (repeat != 1)
            {
                SetHighlightState(false);

                if ((bool)ShuffleBox.Invoke(new Func<bool>(() => ShuffleBox.Checked)))
                {
                    var randomizer = new Random();
                    playback.Track = randomizer.Next(charts.Count);
                }
                else if (repeat == 2 && playback.Track >= charts.Count)
                {
                    playback.Track = 0;
                }
                else if (playback.Track < charts.Count - 1)
                {
                    playback.Track++;
                }
                else
                {
                    playback.Track = 0;
                    return;
                }
            }

            MusicList.Invoke(new Action(() => MusicList.FocusedItem = MusicList.Items[playback.Track]));
            BtnPlay.Invoke(new Action(() => BtnPlay.PerformClick()));
        }
        #endregion
    }
}
