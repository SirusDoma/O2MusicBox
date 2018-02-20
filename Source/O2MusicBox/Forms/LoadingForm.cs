using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace O2MusicBox
{
    public partial class LoadingForm : Form
    {
        private Action action;
        public LoadingForm(Action action)
        {
            InitializeComponent();
            this.action = action;
        }

        private void OnLoadingFormShown(object sender, EventArgs e)
        {
            this.action();
        }

        public void SetStatus(string text)
        {
            if (lblStatus.InvokeRequired)
            {
                lblStatus.Invoke(new Action<string>(SetStatus), text);
                return;
            }

            lblStatus.Text = text;
        }

        public void SetProgress(float percentage)
        {
            if (progressBar.InvokeRequired)
            {
                progressBar.Invoke(new Action<float>(SetProgress), percentage);
                return;
            }

            percentage = percentage > 100f ? 100f : percentage < 0f ? 0f : percentage;
            progressBar.Value = (int)percentage;
        }

        public new void Close()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(Close));
                return;
            }

            base.Close();
        }

        
    }
}
