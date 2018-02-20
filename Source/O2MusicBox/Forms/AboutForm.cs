using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace O2MusicBox
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void OnAboutFormLoad(object sender, EventArgs e)
        {
        }

        private void OnWebLink(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(lblWeb.Text);
        }

        private void OnBtnCloseClick(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
