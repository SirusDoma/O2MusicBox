namespace O2MusicBox
{
    partial class AboutForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblWeb = new System.Windows.Forms.LinkLabel();
            this.lblTrademarks = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblAppInfo = new System.Windows.Forms.Label();
            this.lblCopyright = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(15, 15);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // lblWeb
            // 
            this.lblWeb.AutoSize = true;
            this.lblWeb.Location = new System.Drawing.Point(54, 49);
            this.lblWeb.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWeb.Name = "lblWeb";
            this.lblWeb.Size = new System.Drawing.Size(264, 17);
            this.lblWeb.TabIndex = 2;
            this.lblWeb.TabStop = true;
            this.lblWeb.Text = "http://github.com/SirusDoma/o2musicbox";
            this.lblWeb.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OnWebLink);
            // 
            // lblTrademarks
            // 
            this.lblTrademarks.AutoSize = true;
            this.lblTrademarks.Location = new System.Drawing.Point(15, 112);
            this.lblTrademarks.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTrademarks.Name = "lblTrademarks";
            this.lblTrademarks.Size = new System.Drawing.Size(451, 34);
            this.lblTrademarks.TabIndex = 4;
            this.lblTrademarks.Text = "Use at your own risk without warranty of any kind.\r\nO2Jam and other trademarks ar" +
    "e property of their respective owner(s)";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(18, 153);
            this.btnOK.Margin = new System.Windows.Forms.Padding(4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(448, 44);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "Close";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.OnBtnCloseClick);
            // 
            // lblAppInfo
            // 
            this.lblAppInfo.AutoSize = true;
            this.lblAppInfo.Location = new System.Drawing.Point(54, 15);
            this.lblAppInfo.Name = "lblAppInfo";
            this.lblAppInfo.Size = new System.Drawing.Size(120, 34);
            this.lblAppInfo.TabIndex = 6;
            this.lblAppInfo.Text = "O2Jam Music Box\r\nVersion 0.85 rev2";
            // 
            // lblCopyright
            // 
            this.lblCopyright.AutoSize = true;
            this.lblCopyright.Location = new System.Drawing.Point(54, 68);
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Size = new System.Drawing.Size(200, 17);
            this.lblCopyright.TabIndex = 7;
            this.lblCopyright.Text = "Copyright © 2018 - SirusDoma";
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 210);
            this.Controls.Add(this.lblCopyright);
            this.Controls.Add(this.lblAppInfo);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lblTrademarks);
            this.Controls.Add(this.lblWeb);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About O2Jam Music Box";
            this.Load += new System.EventHandler(this.OnAboutFormLoad);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.LinkLabel lblWeb;
        private System.Windows.Forms.Label lblTrademarks;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblAppInfo;
        private System.Windows.Forms.Label lblCopyright;
    }
}