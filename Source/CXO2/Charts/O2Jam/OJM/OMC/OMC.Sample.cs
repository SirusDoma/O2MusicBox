using System;
using System.Collections.Generic;
using System.Text;

namespace CXO2.Charting.O2Jam
{
    public partial class OJM
    {
        public partial class OMC
        {
            public class WaveSample : IChartSample
            {
                public short Id
                {
                    get; set;
                }

                public string Name
                {
                    get; set;
                }

                public short AudioFormat
                {
                    get; set;
                }

                public short ChannelCount
                {
                    get; set;
                }

                public int SampleRate
                {
                    get; set;
                }

                public int Bitrate
                {
                    get; set;
                }

                public short BlockAlign
                {
                    get; set;
                }

                public short BitsPerSample
                {
                    get; set;
                }

                public int UnkData
                {
                    get; set;
                }

                public int Size
                {
                    get
                    {
                        return (int)Payload?.Length - 44; // 44 = wave size header
                    }
                }

                public byte[] Payload
                {
                    get; set;
                }
            }

            public class OggSample : IChartSample
            {
                public short Id
                {
                    get; set;
                }

                public string Name
                {
                    get; set;
                }

                public int Size
                {
                    get
                    {
                        return (int)Payload?.Length;
                    }
                }

                public byte[] Payload
                {
                    get; set;
                }
            }
        }
    }
}
