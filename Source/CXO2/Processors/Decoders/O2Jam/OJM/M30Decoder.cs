using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Genode;
using Genode.IO;

using CXO2;
using CXO2.Charting;
using CXO2.Charting.O2Jam;

namespace CXO2.Processors.O2Jam
{
    public static class M30Decoder
    {
        public static bool Check(string filename)
        {
            using (var stream = new BufferStream(filename))
            {
                stream.Seek(0, SeekOrigin.Begin);
                return stream.ReadString(4, default(Encoding), false) == OJM.M30.HEADER_SIGNATURE;
            }
        }

        public static OJM.M30 Decode(string filename)
        {
            if (!Check(filename))
                throw new NotSupportedException("Invalid M30 OJM");

            var m30 = new OJM.M30();
            using (var stream = new BufferStream(filename))
            {
                stream.Seek(4, SeekOrigin.Begin);

                m30.Version     = stream.ReadInt32();
                m30.Encryption  = (OJM.M30.EncodingType)stream.ReadInt32();
                int count       = stream.ReadInt32();
                int offset      = stream.ReadInt32();
                int payloadSize = stream.ReadInt32();

                stream.Seek(m30.SampleOffset, SeekOrigin.Begin);

                var samples = new Dictionary<int, OJM.M30.Sample>();
                while (stream.HasRemaining)
                {
                    string name   = stream.ReadString(32);
                    int size      = stream.ReadInt32();
                    var type      = (OJM.M30.SampleType)stream.ReadInt16();
                    short fix     = stream.ReadInt16(); // const 2
                    int musicFlag = stream.ReadInt32(); // const 1
                    short id      = stream.ReadInt16();
                    short zero    = stream.ReadInt16(); // const 0
                    int pcm       = stream.ReadInt32();

                    var sample = new OJM.M30.Sample()
                    {
                        Id   = id,
                        Type = type,
                        Name = name,
                        PCM  = pcm
                    };

                    if (size > 0)
                    {
                        sample.Id += (short)(sample.Type == OJM.M30.SampleType.BGM ? 1000 : 0);
                        if (samples.ContainsKey(sample.Id))
                            throw new Exception("Sample duplication found.");

                        byte[] payload = stream.ReadBytes(size);
                        if (m30.Encryption == OJM.M30.EncodingType.Nami)
                        {
                            byte[] nami = new byte[4] { 0x6E, 0x61, 0x6D, 0x69 };
                            for (int n = 0; n + 4 <= payload.Length; n += 4)
                            {
                                for (int x = 0; x < nami.Length; x++) 
                                    payload[n + x] ^= nami[x];
                            }
                        }

                        sample.Payload = payload;
                        samples.Add(sample.Id, sample);
                    }
                }

                m30.Samples = samples;
                return m30;
            }
        }
    }
}
