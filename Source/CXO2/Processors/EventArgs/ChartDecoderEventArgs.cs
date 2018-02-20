using System;
using System.Collections.Generic;
using System.Text;

using CXO2;
using CXO2.Charting;

namespace CXO2.Processors
{
    /// <summary>
    /// Provides data for <see cref="ChartDecoder.ChartHeaderDecoded"/> event.
    /// </summary>
    public class ChartDecoderEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the filename of the <see cref="Chart"/>.
        /// </summary>
        public string Filename
        {
            get; private set;
        }

        /// <summary>
        /// Gets the <see cref="Charting.Chart"/> to decode.
        /// May not contains any <see cref="Event"/> data for <see cref="ChartDecoder.ChartHeaderDecoded"/> event.
        /// </summary>
        public Chart Chart
        {
            get; private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartDecoderEventArgs"/> class.
        /// </summary>
        /// <param name="filename">The <see cref="Charting.Chart"/> filename to decode.</param>
        /// <param name="chart">Decoded <see cref="Charting.Chart"/>.</param>
        public ChartDecoderEventArgs(string filename, Chart chart)
        {
            Filename = filename;
            Chart    = chart;
        }
    }
}
