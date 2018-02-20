using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;

using CXO2;
using CXO2.Charting;
using CXO2.Processors.O2Jam;

namespace CXO2.Processors
{
    /// <summary>
    /// Provides a decoding mechanism for <see cref="Chart"/> file.
    /// </summary>
    public abstract class ChartDecoder : IDisposable
    {
        /// <summary>
        /// Occurs when the <see cref="Chart"/> header has been decoded successfully.
        /// </summary>
        public event EventHandler<ChartDecoderEventArgs> ChartHeaderDecoded = delegate { };

        /// <summary>
        /// Decode <see cref="Chart"/> from given <see cref="File"/>.
        /// The decoder will be automatically determined by performing check against installed <see cref="ChartDecoder"/>.
        /// </summary>
        /// <param name="filename">Path of <see cref="File"/> to be decoded.</param>
        /// <param name="level">Specify level of chart to be decoded, will most likely ignored on file type that only contain one level at a time.</param>
        /// <returns><see cref="Chart"/> data from decoded <see cref="File"/>.</returns>
        public static Chart Decode(string filename, int level)
        {
            using (var decoder = ChartProcessorFactory.GetDecoder(filename))
            {
                if (decoder == null)
                    throw new NotSupportedException("Specified file does not contain supported chart data.");

                return decoder.Decode(level);
            }
        }

        /// <summary>
        /// Decode <see cref="Chart"/> header with current decoder.
        /// This will not load events not sample data, this scenario best for certain cases such as chart listing.
        /// </summary>
        /// <param name="level">
        /// Specify level / stage of chart to be decoded, will most likely ignored on file type that only contain one level / stage at a time.
        /// Decoder may identify the filename based on level, thus may result different <see cref="Chart"/> header for each level / stage.
        /// </param>
        /// <returns>Decoded <see cref="Chart"/> data from given filename in constructor.</returns>
        public static Chart DecodeHeader(string filename, int level = 0)
        {
            using (var decoder = ChartProcessorFactory.GetDecoder(filename))
            {
                if (decoder == null)
                    throw new NotSupportedException("Specified file does not contain supported chart data.");

                return decoder.DecodeHeader(level);
            }
        }

        /// <summary>
        /// Decode <see cref="Chart"/> header with current decoder.
        /// This will not load events not sample data, this scenario best for certain cases such as chart listing.
        /// </summary>
        /// <param name="level">
        /// Specify level / stage of chart to be decoded, will most likely ignored on file type that only contain one level / stage at a time.
        /// Decoder may identify the filename based on level, thus may result different <see cref="Chart"/> header for each level / stage.
        /// </param>
        /// <returns>Decoded <see cref="Chart"/> data from given filename in constructor.</returns>
        async public static Task<Chart> DecodeHeaderAsync(string filename, int level = 0)
        {
            using (var decoder = ChartProcessorFactory.GetDecoder(filename))
            {
                if (decoder == null)
                    throw new NotSupportedException("Specified file does not contain supported chart data.");

                return await decoder.DecodeHeaderAsync(level);
            }
        }

        /// <summary>
        /// Decode <see cref="Chart"/> from given <see cref="File"/> asynchronously.
        /// The decoder will be automatically determined by performing check against installed <see cref="ChartDecoder"/>.
        /// </summary>
        /// <param name="filename">Path of <see cref="File"/> to be decoded.</param>
        /// <param name="level">Specify level of chart to be decoded, will most likely ignored on file type that only contain one level at a time.</param>
        /// <returns><see cref="Chart"/> data from decoded <see cref="File"/>.</returns>
        async public static Task<Chart> DecodeAsync(string filename, int level)
        {
            using (var decoder = ChartProcessorFactory.GetDecoder(filename))
            {
                if (decoder == null)
                    throw new NotSupportedException("Specified file does not contain supported chart data.");

                return await decoder.DecodeAsync(level);
            }
        }

        /// <summary>
        /// Gets the source filename of the <see cref="Chart"/>.
        /// </summary>
        public string Filename
        {
            get; private set;
        }

        /// <summary>
        /// Gets a value indicating whether the decoder has been provided with invalid <see cref="Stream"/>.
        /// </summary>
        protected internal bool Invalid
        {
            get; private set;
        }

        /// <summary>
        /// Check if current <see cref="ChartDecoder"/> object can handle a give data from specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to check.</param>
        /// <returns><code>true</code> if supported, otherwise false.</returns>
        public abstract bool Check(string filename);

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartDecoder"/> class.
        /// </summary>
        /// <param name="filename">Specify source path of <see cref="File"/> to be decoded with current <see cref="ChartDecoder"/> instance.</param>
        public ChartDecoder(string filename)
        {
            if (Check(filename))
                Filename = filename;
            else
                Invalid = true;
        }

        /// <summary>
        /// Decode <see cref="Chart"/> with current decoder.
        /// </summary>
        /// <param name="level">
        /// Specify level / stage of chart to be decoded, will most likely ignored on file type that only contain one level / stage at a time.
        /// Decoder may identify the filename based on level, thus may result different <see cref="Chart"/> header for each level / stage.
        /// </param>
        /// <returns>Decoded <see cref="Chart"/> data from given filename in constructor.</returns>
        public abstract Chart Decode(int level = 0);

        /// <summary>
        /// Decode <see cref="Chart"/> header with current decoder.
        /// This will not load events not sample data, this scenario best for certain cases such as chart listing.
        /// </summary>
        /// <param name="level">
        /// Specify level / stage of chart to be decoded, will most likely ignored on file type that only contain one level / stage at a time.
        /// Decoder may identify the filename based on level, thus may result different <see cref="Chart"/> header for each level / stage.
        /// </param>
        /// <returns>Decoded <see cref="Chart"/> data from given filename in constructor.</returns>
        public abstract Chart DecodeHeader(int level = 0);

        /// <summary>
        /// Decode <see cref="Chart"/> with current decoder asynchronously.
        /// </summary>
        /// <param name="level">
        /// Specify level / stage of chart to be decoded, will most likely ignored on file type that only contain one level / stage at a time.
        /// Decoder may identify the filename based on level, thus may result different <see cref="Chart"/> header for each level / stage.
        /// </param>
        /// <returns>Decoded <see cref="Chart"/> data from given filename in constructor.</returns>
        async public Task<Chart> DecodeAsync(int level = 0)
        {
            return await Task.Run(() => Decode(level));
        }

        /// <summary>
        /// Decode <see cref="Chart"/> header with current decoder.
        /// This will not load events not sample data, this scenario best for certain cases such as chart listing.
        /// </summary>
        /// <param name="level">
        /// Specify level / stage of chart to be decoded, will most likely ignored on file type that only contain one level / stage at a time.
        /// Decoder may identify the filename based on level, thus may result different <see cref="Chart"/> header for each level / stage.
        /// </param>
        /// <returns>Decoded <see cref="Chart"/> data from given filename in constructor.</returns>
        async public Task<Chart> DecodeHeaderAsync(int level = 0)
        {
            return await Task.Run(() => DecodeHeader(level));
        }

        /// <summary>
        /// Raise <see cref="ChartHeaderDecoded"/> event.
        /// </summary>
        /// <param name="filename">The decoded <see cref="Chart"/> filename.</param>
        /// <param name="chart">Decoded <see cref="Chart"/> instance.</param>
        protected void OnChartHeaderDecoded(string filename, Chart chart)
        {
            ChartHeaderDecoded?.Invoke(this, new ChartDecoderEventArgs(filename, chart));
        }

        /// <summary>
        /// Release all resources used by the <see cref="ChartDecoder"/>.
        /// </summary>
        public virtual void Dispose()
        {
        }
    }
}
