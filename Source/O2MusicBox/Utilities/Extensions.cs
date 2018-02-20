using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace O2MusicBox
{
    public static class Extensions
    {
        public static void SetTextAsync(this Control control, string text)
        {
            if (control.Text != text)
            {
                if (control.InvokeRequired)
                    control.Invoke(new Action(() => control.Text = text));
                else
                    control.Text = text;
            }
        }
    }
}
