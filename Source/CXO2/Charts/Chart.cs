using System;
using System.Collections.Generic;
using System.Text;

namespace CXO2.Charting
{
    public class Chart
    {
        public string Title
        {
            get; set;
        }

        public string Artist
        {
            get; set;
        }

        public string Pattern
        {
            get; set;
        }

        public double BPM
        {
            get; set;
        }

        public TimeSpan Duration
        {
            get; set;
        }

        public byte[] ThumbnailData
        {
            get; set;
        }

        public byte[] CoverArtData
        {
            get; set;
        }

        public Event[] Events
        {
            get; set;
        }

        public string Filename
        {
            get; private set;
        }

        public Chart()
        {
        }

        public Chart(string filename, int level)
        {
            var chart = CXO2.Processors.ChartDecoder.Decode(filename, level);

            Title    = chart.Title;
            Artist   = chart.Artist;
            Pattern  = chart.Pattern;
            BPM      = chart.BPM;
            Events   = chart.Events;
            Duration = chart.Duration;
            Filename = filename;

            ThumbnailData = chart.ThumbnailData;
            CoverArtData  = chart.CoverArtData;
        }

        internal Chart(string filename)
        {
            Filename = filename;
        }
    }
}
