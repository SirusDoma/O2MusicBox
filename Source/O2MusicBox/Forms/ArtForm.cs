using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CXO2;
using CXO2.Charting;

namespace O2MusicBox
{
    public partial class ArtForm : Form
    {
        public ArtForm(Chart chart)
        {
            InitializeComponent();

            using (var stream = new System.IO.MemoryStream(chart.CoverArtData))
            {
                Text = chart.Title;
                CoverBox.Image = new Bitmap(stream);
            }
        }
    }
}
