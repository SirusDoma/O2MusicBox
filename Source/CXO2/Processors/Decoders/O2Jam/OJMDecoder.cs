using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using CXO2;
using CXO2.Charting.O2Jam;

namespace CXO2.Processors.O2Jam
{
    public static class OJMDecoder
    {
        public static bool Check(string filename)
        {
            return OMCDecoder.Check(filename) || M30Decoder.Check(filename);
        }

        public static OJM Decode(string filename)
        {
            if (!File.Exists(filename))
                return null;

            var ojm = new OJM();
            ojm.Filename = filename;
            var samples = new List<IChartSample>();

            if (OMCDecoder.Check(filename))
            {
                ojm.Format = OJM.FileFormat.OMC;
                var omc = OMCDecoder.Decode(filename);
                foreach (var sample in omc.WaveSamples)
                    samples.Add(sample.Value);

                foreach (var sample in omc.OggSamples)
                    samples.Add(sample.Value);
            }
            else if (M30Decoder.Check(filename))
            {
                ojm.Format = OJM.FileFormat.M30;
                var m30 = M30Decoder.Decode(filename);
                foreach (var sample in m30.Samples)
                    samples.Add(sample.Value);
            }
            else
            {
                throw new FormatException("Invalid OJM Format.");
            }

            ojm.Samples = samples.ToArray();
            return ojm;
        }
    }
}
