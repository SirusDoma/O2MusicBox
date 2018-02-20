using System;
using System.Collections.Generic;
using System.Text;

namespace CXO2.Charting.O2Jam
{
    public partial class OJM
    {
        public partial class M30
        {
            public class Sample : IChartSample
            {
                public string Name
                {
                    get; set;
                }

                public int Size
                {
                    get { return (int)Payload?.Length; }
                }

                public SampleType Type
                {
                    get; set;
                }

                public short Fix
                {
                    get { return 2; }
                }

                public int Flag
                {
                    get { return 1; }
                }

                public short Id
                {
                    get; set;
                }

                public short Zero
                {
                    get { return 0; }
                }

                public int PCM
                {
                    get; set;
                }

                public byte[] Payload
                {
                    get; set;
                }
            }
        }
    }
}