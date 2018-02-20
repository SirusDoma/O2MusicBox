using System;
using System.Collections.Generic;
using System.Text;

namespace CXO2.Charting.O2Jam
{
    public partial class OJM
    {
        /// <summary>
        /// Represents O2Jam <see cref="Chart"/> M30 Samples Container.
        /// </summary>
        public partial class M30
        {
            public const string HEADER_SIGNATURE = "M30\0";

            public enum EncodingType
            {
                None,
                Scramble1 = 1,
                Scramble2 = 2,
                Decode    = 4,
                Decrypt   = 8,
                Nami      = 16
            }

            public enum SampleType
            {
                BGM      = 0,
                Keysound = 5
            }

            public byte[] Signature
            {
                get
                {
                    return Encoding.UTF8.GetBytes(HEADER_SIGNATURE);
                }
            }

            public int Version
            {
                get; set;
            }

            public EncodingType Encryption
            {
                get; set;
            }

            public int SampleCount
            {
                get { return (int)Samples?.Count; }
            }

            public int SampleOffset
            {
                get { return 28; }
            }

            public int PayloadSize
            {
                get
                {
                    int payloadSize = 0;
                    if (Samples != null && Samples.Count > 0)
                    {
                        foreach (var sample in Samples.Values)
                        {
                            payloadSize += ((int)sample.Payload?.Length) + 52;
                        }
                    }

                    return payloadSize;
                }
            }

            public Dictionary<int, M30.Sample> Samples
            {
                get; set;
            }
        }
    }
}