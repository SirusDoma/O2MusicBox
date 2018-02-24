using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Cgen;
using Cgen.Audio;
using Cgen.IO;

using CXO2;
using CXO2.Charting;
using CXO2.Charting.O2Jam;

namespace CXO2.Processors.O2Jam
{
    public static class M30Encoder
    {
        public static bool Check(string extension)
        {
            return extension == "m30.ojm";
        }

        public static byte[] Encode(OJM.M30 m30)
        {
            using (var stream = new BufferStream())
            {
                stream.Write(OJM.M30.HEADER_SIGNATURE);
                stream.Write(m30.Version);
                stream.Write((int)m30.Encryption);
                stream.Write(m30.SampleCount);
                stream.Write(m30.SampleOffset);
                stream.Write(m30.PayloadSize);

                stream.Seek(m30.SampleOffset, SeekOrigin.Begin);

                var samples = m30.Samples;
                foreach (var sampleKeyPair in samples)
                {
                    var sample = sampleKeyPair.Value;
                    byte[] payload = sample.Payload;

                    var info = EncodeToOGG(payload);
                    sample.Payload = payload;
                    sample.PCM     = (int)(info.SampleCount * info.ChannelCount);

                    stream.Write(sample.Name.PadNull(32));
                    stream.Write(payload.Length);
                    stream.Write((short)sample.Type);
                    stream.Write(sample.Fix);
                    stream.Write(sample.Flag);
                    stream.Write((short)(sample.Type == OJM.M30.SampleType.BGM ? sample.Id - 1000 : sample.Id));
                    stream.Write(sample.Zero);
                    stream.Write(sample.PCM);

                    if (payload != null && payload.Length > 0)
                    {
                        if (m30.Encryption == OJM.M30.EncodingType.Nami)
                        {
                            byte[] nami = new byte[4] { 0x6E, 0x61, 0x6D, 0x69 };
                            for (int n = 0; n + 4 <= payload.Length; n += 4)
                            {
                                for (int x = 0; x < nami.Length; x++)
                                    payload[n + x] ^= nami[x];
                            }
                        }

                        stream.Write(payload);
                    }
                }

                return stream.ToArray();
            }
        }

        private static SampleInfo EncodeToOGG(byte[] payload)
        {
            payload = payload == null ? new byte[0] : payload;
            if (payload == null || payload.Length == 0)
                return default(SampleInfo);

            using (var decoder = SoundProcessorFactory.CreateDecoder(new MemoryStream(payload)))
            {
                if (decoder == null)
                {
                    return default(SampleInfo);
                }
                else if (!(decoder is OggDecoder))
                {
                    short[] buffer = new short[44100 * 2];
                    using (var sampleStream = new MemoryStream())
                    using (var encoder = new OggEncoder(sampleStream, 44100, 2))
                    {
                        long read = 0;
                        do
                        {
                            read = decoder.Read(buffer, buffer.Length);
                            encoder.Write(buffer, read);
                        }
                        while (read > 0);

                        encoder.Flush();
                        payload = sampleStream.ToArray();
                        return encoder.SampleInfo;
                    }
                }

                return decoder.SampleInfo;
            }
        }
    }
}
