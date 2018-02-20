using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CXO2.Charting.O2Jam
{
    public partial class OJM
    {
        /// <summary>
        /// Represents O2Jam <see cref="Chart"/> OMC Samples Container.
        /// </summary>
        public partial class OMC
        {
            public string Signature
            {
                get
                {
                    return "OMC\0";
                }
            }

            public short WaveCount
            {
                get { return (short)WaveSamples?.Count; }
            }

            public short OggCount
            {
                get { return (short)OggSamples?.Count; }
            }

            public int WaveOffset
            {
                get
                {
                    return 20;
                }
            }

            public int OggOffset
            {
                get
                {
                    int offset = 20;
                    if (WaveSamples != null)
                    {
                        int max = WaveSamples.Max((sample) => sample.Value.Id);
                        for (int i = 0; i < max; i++)
                        {
                            var sample = WaveSamples.ContainsKey(i) ? WaveSamples[i] : null;
                            offset += 56 + (int)sample?.Size;
                        }
                    }

                    return offset;
                }
            }

            public int FileSize
            {
                get
                {
                    int offset = OggOffset;
                    if (OggSamples != null)
                    {
                        foreach (var sample in OggSamples.Values)
                        {
                            offset += 36 + sample.Size;
                        }
                    }

                    return offset;
                }
            }

            public Dictionary<int, OMC.WaveSample> WaveSamples
            {
                get; set;
            }

            public Dictionary<int, OMC.OggSample> OggSamples
            {
                get; set;
            }

            public OMC()
            {
            }
        }
    }
}
