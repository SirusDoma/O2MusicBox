using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using CXO2;
using CXO2.Charting;

namespace O2MusicBox
{
    public class PlaybackState
    {
        public ChartRenderer Renderer
        {
            get; set;
        }

        public int Track
        {
            get; set;
        }

        public PlaybackState()
        {
        }
    }
}
