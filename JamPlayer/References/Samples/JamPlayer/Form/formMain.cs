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
using System.Threading;
using System.Diagnostics;
using CXO2;
using CXO2.Core;
using CXO2.Core.Renderer;
using CXO2.Core.SoundSystem;
using CXO2.Data;
using CXO2.Parser;

namespace JamPlayer
{
    public partial class formMain : Form
    {

        #region --- Variables ---
        bool playing = false;
        System.Windows.Forms.Timer renderTimer = new System.Windows.Forms.Timer();
        #endregion

        #region --- Constructor ---
        // Just Ordinary Constructor
        public formMain()
        {
            InitializeComponent();

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
            {
                // Something wrong with SoundSystem
                // Check FMOD DLL and / or Version
                MessageBox.Show("Failed to Initialize Sound System\nMake sure fmodex.dll is located in same directory", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
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
                if (playing)
                {
                    // Reset
                    btnPlay.Text = "Start";

                    // Stop
                    playing = false;
                    renderTimer.Stop();
                }
            }
        }
        #endregion

        #region --- Play ---
        private void btnPlay_Click(object sender, EventArgs e)
        {
            // When Button play clicked
            try
            {
                // Play Button
                if (!playing)
                {
                    // Create open dialog
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.Filter = "O2Jam Chart Music|*.ojn";

                    // User press cancel
                    if (ofd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                        return;

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
                    if (!ChartParser.parse(ref chart, ofd.FileName, mode))
                    {
                        // Failed to parse
                        MessageBox.Show("Failed to parse chart", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

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

                    // Reset
                    btnPlay.Text = "Start";
                    
                }
            }
            catch (Exception)
            {
                // Failed to parse
                MessageBox.Show("Failed to parse chart", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region --- About ---
        private void btnAbout_Click(object sender, EventArgs e)
        {
            // Just messagebox, too lazy to add new form lol
            MessageBox.Show("Sample of usage JamPlayer Engine.\nWritten by CXO2", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
