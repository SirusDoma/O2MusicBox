using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using Genode;
using Genode.Audio;

using CXO2;
using CXO2.Charting;
using CXO2.Processors;

namespace O2MusicBox
{
    public partial class RenderForm : Form
    {
        private Chart[] charts;
        public RenderForm(params Chart[] charts)
        {
            InitializeComponent();
            this.charts = charts;
        }

        private void OnRenderFormLoad(object sender, EventArgs e)
        {
            FormatSelector.SelectedIndex = 0;
            FrequencySelector.SelectedIndex = 0;
        }

        private void OnFrequencySelectorSelectedIndexChanged(object sender, EventArgs e)
        {
            FrequencyWarningLabel.Visible = FrequencySelector.SelectedIndex != 0;
            SettingsGroup.Height = FrequencySelector.SelectedIndex == 0 ? 200 : 242;
            Height = FrequencySelector.SelectedIndex == 0 ? 348 : 388;
        }

        private void OnStartButtonClick(object sender, EventArgs e)
        {
            string filename  = "";
            string directory = "";
            string err = "";

            int format  = FormatSelector.SelectedIndex;
            int freq    = int.Parse(FrequencySelector.SelectedItem.ToString().Split(' ').First());
            int channel = StereoRadio.Checked ? 2 : 1;

            if (charts.Length == 1)
            {
                var saveDialog = new SaveFileDialog();
                saveDialog.FileName = string.Format("{0} - {1}", charts[0].Artist, charts[0].Title);

                if (format == 0)
                {
                    saveDialog.FileName += ".mp3";
                    saveDialog.Filter = "MP3 - MPEG-1 Audio Layer 3|*.mp3";
                }
                else if (format == 1)
                {
                    saveDialog.FileName += ".ogg";
                    saveDialog.Filter = "OGG - Vorbis Audio|*.ogg";
                }
                else
                {
                    saveDialog.FileName += ".wav";
                    saveDialog.Filter = "WAV - Waveform Audio|*.wav";
                }

                if (saveDialog.ShowDialog() != DialogResult.OK)
                    return;

                filename = saveDialog.FileName;
            }
            else if (charts.Length > 1)
            {
                var browserDialog = new FolderBrowserDialog();
                browserDialog.ShowNewFolderButton = true;

                if (browserDialog.ShowDialog() != DialogResult.OK)
                    return;

                directory = browserDialog.SelectedPath;
            }

            LoadingForm loader = null;
            loader = new LoadingForm(async () =>
            {
                float progress = 0;
                loader.SetStatus("Initializing..");

                foreach (var chart in charts)
                {
                    try
                    {
                        loader.SetStatus(string.Format("Prerendering {0}..", chart.Title));

                        var fullChart = await ChartDecoder.DecodeAsync(chart.Filename, 2);
                        var data = await Prerender(fullChart, format, channel, freq);

                        if (charts.Length == 1)
                        {
                            File.WriteAllBytes(filename, data);
                        }
                        else if (charts.Length > 1)
                        {
                            filename = string.Format("{0} - {1}", chart.Artist, chart.Title);
                            if (format == 0)
                                filename += ".mp3";
                            else if (format == 1)
                                filename += ".ogg";
                            else
                                filename += ".wav";

                            filename = string.Format("{0}{1}{2}", 
                                directory, 
                                Path.DirectorySeparatorChar, 
                                filename
                            );

                            File.WriteAllBytes(filename, data);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (charts.Length == 1)
                        {
                            err = ex.Message;
                            loader.DialogResult = DialogResult.Cancel;
                            loader.Close();
                        }
                    }
                    finally
                    {
                        progress++;
                        loader.SetProgress((progress / charts.Length) * 100f);
                    }
                }

                loader.Close();
            });

            if (loader.ShowDialog() == DialogResult.Abort)
            {
                if (charts.Length == 1)
                    MessageBox.Show("Failed to prerender the chart into file\n." + err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else if (!string.IsNullOrEmpty(directory))
                    MessageBox.Show("Failed to render following charts:\n." + err, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
                MessageBox.Show("Prerender has been completed successfully.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Close();
        }

        async private Task<byte[]> Prerender(Chart chart, int format = 0, int channel = 2, int sampleRate = 44100)
        {
            var mixed = await Task.Run(() => SampleMixer.Mix(chart, SampleMixer.MixMode.Full, channel, sampleRate));
            var buffer = new SoundBuffer(mixed.GetSamples(), channel, sampleRate);
            byte[] data = null;

            if (format == 0)
                data = await Task.Run(() => buffer.Save<MP3Encoder>());
            else if (format == 1)
                data = await Task.Run(() => buffer.Save<OggEncoder>());
            else
                data = await Task.Run(() => buffer.Save<WavEncoder>());

            foreach (var ev in chart.Events)
            {
                var sound = ev as Event.Sound;
                if (sound == null)
                    continue;

                sound.Payload = null;
                sound.Dispose();
            }

            return data;
        }
    }
}
