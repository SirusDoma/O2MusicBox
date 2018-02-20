using System;
using System.Collections.Generic;
using System.Text;

namespace CXO2.Charting.O2Jam
{
    /// <summary>
    /// Represents O2Jam <see cref="Chart"/> Samples Container.
    /// </summary>
    public partial class OJM
    {
        /// <summary>
        /// Represent <see cref="OJM"/> file format.
        /// </summary>
        public enum FileFormat
        {
            M30 = 0,
            OMC = 1
        }

        public FileFormat Format
        {
            get; set;
        }

        public string Filename
        {
            get; set;
        }

        public IChartSample[] Samples
        {
            get; set;
        }
    }
}
