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
            this.btnPlay = new System.Windows.Forms.Button();
            this.groupDifficulty = new System.Windows.Forms.GroupBox();
            this.rdHX = new System.Windows.Forms.RadioButton();
            this.rdNX = new System.Windows.Forms.RadioButton();
            this.rdEX = new System.Windows.Forms.RadioButton();
            this.btnAbout = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.groupDifficulty.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPlay
            // 
            this.btnPlay.Location = new System.Drawing.Point(12, 12);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(419, 59);
            this.btnPlay.TabIndex = 0;
            this.btnPlay.Text = "Play";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // groupDifficulty
            // 
            this.groupDifficulty.Controls.Add(this.rdHX);
            this.groupDifficulty.Controls.Add(this.rdNX);
            this.groupDifficulty.Controls.Add(this.rdEX);
            this.groupDifficulty.Location = new System.Drawing.Point(12, 77);
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
            this.btnAbout.Location = new System.Drawing.Point(210, 94);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(143, 29);
            this.btnAbout.TabIndex = 2;
            this.btnAbout.Text = "About";
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(359, 94);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(72, 29);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // formMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 145);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnAbout);
            this.Controls.Add(this.groupDifficulty);
            this.Controls.Add(this.btnPlay);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "formMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Main Form";
            this.groupDifficulty.ResumeLayout(false);
            this.groupDifficulty.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.GroupBox groupDifficulty;
        private System.Windows.Forms.RadioButton rdHX;
        private System.Windows.Forms.RadioButton rdNX;
        private System.Windows.Forms.RadioButton rdEX;
        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.Button btnExit;

    }
}

