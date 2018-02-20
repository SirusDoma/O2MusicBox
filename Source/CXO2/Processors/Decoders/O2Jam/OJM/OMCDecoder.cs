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
    public static class OMCDecoder
    {
        public static bool Check(string filename)
        {
            using (var stream = new BufferStream(filename))
            {
                stream.Seek(0, SeekOrigin.Begin);
                string sign = stream.ReadString(4, default(Encoding), false);
                return sign == "OMC\0" || sign == "OJM\0";
            }
        }

        public static OJM.OMC Decode(string filename)
        {
            if (!Check(filename))
                throw new NotSupportedException("Invalid OMC OJM");

            var omc = new OJM.OMC();
            using (var stream = new BufferStream(filename))
            {
                stream.Seek(4, SeekOrigin.Begin);

                short waveCount = stream.ReadInt16();
                short oggCount  = stream.ReadInt16();
                int waveOffset  = stream.ReadInt32();
                int oggOffset   = stream.ReadInt32();
                int fileSize    = stream.ReadInt32();

                var waveSamples = new Dictionary<int, OJM.OMC.WaveSample>();
                stream.Seek(waveOffset, SeekOrigin.Begin);
                for (short i = 0; i < waveCount; i++)
                {
                    var sample = new OJM.OMC.WaveSample()
                    {
                        Id            = i,
                        Name          = stream.ReadString(32),
                        AudioFormat   = stream.ReadInt16(),
                        ChannelCount  = stream.ReadInt16(),
                        SampleRate    = stream.ReadInt32(),
                        Bitrate       = stream.ReadInt32(),
                        BlockAlign    = stream.ReadInt16(),
                        BitsPerSample = stream.ReadInt16(),
                        UnkData       = stream.ReadInt32()
                    };

                    int size = stream.ReadInt32();
                    if (size > 0)
                    {
                        using (var waveStream = new BufferStream())
                        {
                            waveStream.Write("RIFF");
                            waveStream.Write((int)(size + 36));
                            waveStream.Write("WAVE");
                            waveStream.Write("fmt ");
                            waveStream.Write((int)16); // PCM OJM Format
                            waveStream.Write((short)sample.AudioFormat);
                            waveStream.Write((short)sample.ChannelCount);
                            waveStream.Write((int)sample.SampleRate);
                            waveStream.Write((int)sample.Bitrate);
                            waveStream.Write((short)sample.BlockAlign);
                            waveStream.Write((short)sample.BitsPerSample);
                            waveStream.Write("data");
                            waveStream.Write((int)size);
                            waveStream.Write(DecodeWave(stream.ReadBytes(size)));

                            sample.Payload = waveStream.ToArray();
                        }

                        waveSamples.Add(sample.Id, sample);
                    }
                }

                var oggSamples = new Dictionary<int, OJM.OMC.OggSample>();
                stream.Seek(oggOffset, SeekOrigin.Begin);
                for (short i = 1000; i < oggCount + 1000; i++)
                {
                    var sample = new OJM.OMC.OggSample()
                    {
                        Id   = i,
                        Name = stream.ReadString(32)
                    };

                    int size = stream.ReadInt32();
                    if (size > 0)
                    {
                        sample.Payload = stream.ReadBytes(size);
                        oggSamples.Add(sample.Id, sample);
                    }
                }

                omc.WaveSamples = waveSamples;
                omc.OggSamples  = oggSamples;
                return omc;
            }
        }

        #region DecodeWave
        private static byte[] REARRANGE_TABLE = new byte[]
        {
                0x10, 0x0E, 0x02, 0x09, 0x04, 0x00, 0x07, 0x01,
                0x06, 0x08, 0x0F, 0x0A, 0x05, 0x0C, 0x03, 0x0D,
                0x0B, 0x07, 0x02, 0x0A, 0x0B, 0x03, 0x05, 0x0D,
                0x08, 0x04, 0x00, 0x0C, 0x06, 0x0F, 0x0E, 0x10,
                0x01, 0x09, 0x0C, 0x0D, 0x03, 0x00, 0x06, 0x09,
                0x0A, 0x01, 0x07, 0x08, 0x10, 0x02, 0x0B, 0x0E,
                0x04, 0x0F, 0x05, 0x08, 0x03, 0x04, 0x0D, 0x06,
                0x05, 0x0B, 0x10, 0x02, 0x0C, 0x07, 0x09, 0x0A,
                0x0F, 0x0E, 0x00, 0x01, 0x0F, 0x02, 0x0C, 0x0D,
                0x00, 0x04, 0x01, 0x05, 0x07, 0x03, 0x09, 0x10,
                0x06, 0x0B, 0x0A, 0x08, 0x0E, 0x00, 0x04, 0x0B,
                0x10, 0x0F, 0x0D, 0x0C, 0x06, 0x05, 0x07, 0x01,
                0x02, 0x03, 0x08, 0x09, 0x0A, 0x0E, 0x03, 0x10,
                0x08, 0x07, 0x06, 0x09, 0x0E, 0x0D, 0x00, 0x0A,
                0x0B, 0x04, 0x05, 0x0C, 0x02, 0x01, 0x0F, 0x04,
                0x0E, 0x10, 0x0F, 0x05, 0x08, 0x07, 0x0B, 0x00,
                0x01, 0x06, 0x02, 0x0C, 0x09, 0x03, 0x0A, 0x0D,
                0x06, 0x0D, 0x0E, 0x07, 0x10, 0x0A, 0x0B, 0x00,
                0x01, 0x0C, 0x0F, 0x02, 0x03, 0x08, 0x09, 0x04,
                0x05, 0x0A, 0x0C, 0x00, 0x08, 0x09, 0x0D, 0x03,
                0x04, 0x05, 0x10, 0x0E, 0x0F, 0x01, 0x02, 0x0B,
                0x06, 0x07, 0x05, 0x06, 0x0C, 0x04, 0x0D, 0x0F,
                0x07, 0x0E, 0x08, 0x01, 0x09, 0x02, 0x10, 0x0A,
                0x0B, 0x00, 0x03, 0x0B, 0x0F, 0x04, 0x0E, 0x03,
                0x01, 0x00, 0x02, 0x0D, 0x0C, 0x06, 0x07, 0x05,
                0x10, 0x09, 0x08, 0x0A, 0x03, 0x02, 0x01, 0x00,
                0x04, 0x0C, 0x0D, 0x0B, 0x10, 0x05, 0x06, 0x0F,
                0x0E, 0x07, 0x09, 0x0A, 0x08, 0x09, 0x0A, 0x00,
                0x07, 0x08, 0x06, 0x10, 0x03, 0x04, 0x01, 0x02,
                0x05, 0x0B, 0x0E, 0x0F, 0x0D, 0x0C, 0x0A, 0x06,
                0x09, 0x0C, 0x0B, 0x10, 0x07, 0x08, 0x00, 0x0F,
                0x03, 0x01, 0x02, 0x05, 0x0D, 0x0E, 0x04, 0x0D,
                0x00, 0x01, 0x0E, 0x02, 0x03, 0x08, 0x0B, 0x07,
                0x0C, 0x09, 0x05, 0x0A, 0x0F, 0x04, 0x06, 0x10,
                0x01, 0x0E, 0x02, 0x03, 0x0D, 0x0B, 0x07, 0x00,
                0x08, 0x0C, 0x09, 0x06, 0x0F, 0x10, 0x05, 0x0A,
                0x04, 0x00
        };

        private static byte[] DecodeWave(byte[] encoded)
        {
            return AccXor(Rearrange(encoded));
        }

        private static byte[] Rearrange(byte[] encodedBuffer)
        {
            int length = encodedBuffer.Length;
            int key = ((length % 17) << 4) + (length % 17);

            int blockSize = length / 17;

            // Let's fill the buffer
            byte[] buffer = new byte[length];
            Array.Copy(encodedBuffer, 0, buffer, 0, length);

            for (int block = 0; block < 17; block++) // loopy loop
            {
                int encodedOffset = blockSize * block;   // Where is the start of the enconded block
                int bufferOffset  = blockSize * REARRANGE_TABLE[key];  // Where the final plain block will be
                Array.Copy(encodedBuffer, encodedOffset, buffer, bufferOffset, blockSize);

                key++;
            }

            return buffer;
        }


        private static byte[] AccXor(byte[] buffer)
        {
            int keybyte = 0xFF, counter = 0;
            for (int i = 0; i < buffer.Length; i++)
            {
                byte curByte = buffer[i];
                int accXor   = ((keybyte << counter) & 0x80);

                if (accXor != 0)
                    curByte = (byte)~curByte;

                buffer[i] = curByte;
                counter++;
                if (counter > 7)
                {
                    counter = 0;
                    keybyte = curByte;
                }
            }

            return buffer;
        }
        #endregion
    }
}
