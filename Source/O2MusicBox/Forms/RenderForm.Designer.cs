namespace O2MusicBox
{
    partial class RenderForm
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
            this.SettingsGroup = new System.Windows.Forms.GroupBox();
            this.FormatLabel = new System.Windows.Forms.Label();
            this.FormatSelector = new System.Windows.Forms.ComboBox();
            this.FrequencyWarningLabel = new System.Windows.Forms.Label();
            this.FrequencyLabel = new System.Windows.Forms.Label();
            this.FrequencySelector = new System.Windows.Forms.ComboBox();
            this.StartButton = new System.Windows.Forms.Button();
            this.ChannelLabel = new System.Windows.Forms.Label();
            this.StereoRadio = new System.Windows.Forms.RadioButton();
            this.MonoRadio = new System.Windows.Forms.RadioButton();
            this.SettingsGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // SettingsGroup
            // 
            this.SettingsGroup.Controls.Add(this.MonoRadio);
            this.SettingsGroup.Controls.Add(this.StereoRadio);
            this.SettingsGroup.Controls.Add(this.ChannelLabel);
            this.SettingsGroup.Controls.Add(this.FormatLabel);
            this.SettingsGroup.Controls.Add(this.FormatSelector);
            this.SettingsGroup.Controls.Add(this.FrequencyLabel);
            this.SettingsGroup.Controls.Add(this.FrequencySelector);
            this.SettingsGroup.Controls.Add(this.FrequencyWarningLabel);
            this.SettingsGroup.Location = new System.Drawing.Point(26, 24);
            this.SettingsGroup.Name = "SettingsGroup";
            this.SettingsGroup.Size = new System.Drawing.Size(387, 200);
            this.SettingsGroup.TabIndex = 0;
            this.SettingsGroup.TabStop = false;
            this.SettingsGroup.Text = "Encoding Settings";
            // 
            // FormatLabel
            // 
            this.FormatLabel.AutoSize = true;
            this.FormatLabel.Location = new System.Drawing.Point(16, 30);
            this.FormatLabel.Name = "FormatLabel";
            this.FormatLabel.Size = new System.Drawing.Size(52, 17);
            this.FormatLabel.TabIndex = 0;
            this.FormatLabel.Text = "Format";
            // 
            // FormatSelector
            // 
            this.FormatSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FormatSelector.FormattingEnabled = true;
            this.FormatSelector.Items.AddRange(new object[] {
            "MP3 - MPEG-1 Audio Layer 3",
            "OGG - Vorbis Audio",
            "WAV - Waveform Audio"});
            this.FormatSelector.Location = new System.Drawing.Point(19, 50);
            this.FormatSelector.Name = "FormatSelector";
            this.FormatSelector.Size = new System.Drawing.Size(352, 24);
            this.FormatSelector.TabIndex = 1;
            // 
            // FrequencyWarningLabel
            // 
            this.FrequencyWarningLabel.AutoSize = true;
            this.FrequencyWarningLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.FrequencyWarningLabel.Location = new System.Drawing.Point(16, 194);
            this.FrequencyWarningLabel.Name = "FrequencyWarningLabel";
            this.FrequencyWarningLabel.Size = new System.Drawing.Size(316, 34);
            this.FrequencyWarningLabel.TabIndex = 7;
            this.FrequencyWarningLabel.Text = "Changing frequency may cause undesired result!\r\nProceed with caution!";
            // 
            // FrequencyLabel
            // 
            this.FrequencyLabel.AutoSize = true;
            this.FrequencyLabel.Location = new System.Drawing.Point(16, 139);
            this.FrequencyLabel.Name = "FrequencyLabel";
            this.FrequencyLabel.Size = new System.Drawing.Size(75, 17);
            this.FrequencyLabel.TabIndex = 5;
            this.FrequencyLabel.Text = "Frequency";
            // 
            // FrequencySelector
            // 
            this.FrequencySelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FrequencySelector.FormattingEnabled = true;
            this.FrequencySelector.Items.AddRange(new object[] {
            "44100 Hz",
            "44800 Hz",
            "96000 Hz"});
            this.FrequencySelector.Location = new System.Drawing.Point(19, 159);
            this.FrequencySelector.Name = "FrequencySelector";
            this.FrequencySelector.Size = new System.Drawing.Size(352, 24);
            this.FrequencySelector.TabIndex = 6;
            this.FrequencySelector.SelectedIndexChanged += new System.EventHandler(this.OnFrequencySelectorSelectedIndexChanged);
            // 
            // StartButton
            // 
            this.StartButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StartButton.Location = new System.Drawing.Point(26, 242);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(387, 48);
            this.StartButton.TabIndex = 1;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.OnStartButtonClick);
            // 
            // ChannelLabel
            // 
            this.ChannelLabel.AutoSize = true;
            this.ChannelLabel.Location = new System.Drawing.Point(16, 86);
            this.ChannelLabel.Name = "ChannelLabel";
            this.ChannelLabel.Size = new System.Drawing.Size(67, 17);
            this.ChannelLabel.TabIndex = 2;
            this.ChannelLabel.Text = "Channels";
            // 
            // StereoRadio
            // 
            this.StereoRadio.AutoSize = true;
            this.StereoRadio.Checked = true;
            this.StereoRadio.Location = new System.Drawing.Point(19, 106);
            this.StereoRadio.Name = "StereoRadio";
            this.StereoRadio.Size = new System.Drawing.Size(71, 21);
            this.StereoRadio.TabIndex = 3;
            this.StereoRadio.TabStop = true;
            this.StereoRadio.Text = "Stereo";
            this.StereoRadio.UseVisualStyleBackColor = true;
            // 
            // MonoRadio
            // 
            this.MonoRadio.AutoSize = true;
            this.MonoRadio.Location = new System.Drawing.Point(96, 106);
            this.MonoRadio.Name = "MonoRadio";
            this.MonoRadio.Size = new System.Drawing.Size(64, 21);
            this.MonoRadio.TabIndex = 4;
            this.MonoRadio.Text = "Mono";
            this.MonoRadio.UseVisualStyleBackColor = true;
            // 
            // RenderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 313);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.SettingsGroup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RenderForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Prerender samples";
            this.Load += new System.EventHandler(this.OnRenderFormLoad);
            this.SettingsGroup.ResumeLayout(false);
            this.SettingsGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox SettingsGroup;
        private System.Windows.Forms.Label FormatLabel;
        private System.Windows.Forms.ComboBox FormatSelector;
        private System.Windows.Forms.Label FrequencyWarningLabel;
        private System.Windows.Forms.Label FrequencyLabel;
        private System.Windows.Forms.ComboBox FrequencySelector;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.RadioButton MonoRadio;
        private System.Windows.Forms.RadioButton StereoRadio;
        private System.Windows.Forms.Label ChannelLabel;
    }
}