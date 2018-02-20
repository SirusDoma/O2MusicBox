using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

using Genode;
using Genode.Audio;
using Genode.IO;

using CXO2;
using CXO2.Charting.O2Jam;

namespace CXO2.Processors.O2Jam
{
    public static class OMCEncoder
    {
        public static bool Check(string extension)
        {
            return extension == "omc.ojm";
        }

        public static byte[] Encode(OJM.OMC omc)
        {
            using (var stream = new BufferStream())
            {
                stream.Write("OMC\0");
                stream.Write(omc.WaveCount);
                stream.Write(omc.OggCount);
                stream.Write(omc.WaveOffset);
                stream.Write(omc.OggOffset);
                stream.Write(omc.FileSize);

                stream.Seek(omc.WaveOffset, SeekOrigin.Begin);
                for (short i = 0; i < omc.WaveSamples.Max((sample) => sample.Value.Id); i++)
                {
                    OJM.OMC.WaveSample sample = null;
                    string name         = string.Empty.PadNull(32);
                    short audioFormat   = 0;
                    short channelCount  = 0;
                    int sampleRate      = 0;
                    int bitrate         = 0;
                    short blockAlign    = 0;
                    short bitsPerSample = 0;
                    int unkData         = 0;
                    int size            = 0;

                    if (omc.WaveSamples.ContainsKey(i))
                    {
                        sample        = omc.WaveSamples[i];
                        name          = sample.Name.PadNull(32);
                        audioFormat   = sample.AudioFormat;
                        channelCount  = sample.ChannelCount;
                        sampleRate    = sample.SampleRate;
                        bitrate       = sample.Bitrate;
                        blockAlign    = sample.BlockAlign;
                        bitsPerSample = sample.BitsPerSample;
                        unkData       = sample.UnkData;
                        size          = sample.Size;
                    }

                    stream.Write(name);
                    stream.Write(audioFormat);
                    stream.Write(channelCount);
                    stream.Write(sampleRate);
                    stream.Write(bitrate);
                    stream.Write(blockAlign);
                    stream.Write(bitsPerSample);
                    stream.Write(unkData);
                    stream.Write(size);

                    if (sample != null && size > 0)
                    {
                        byte[] payload = sample.Payload;
                        var info = EncodeTo<WavEncoder>(payload);

                        sample.Payload = payload;
                        using (var waveStream = new BufferStream(payload))
                        {
                            waveStream.Seek(44, SeekOrigin.Begin);
                            stream.Write(EncodeWave(waveStream.ReadRemaining()));
                        }
                    }
                }

                stream.Seek(omc.OggOffset, SeekOrigin.Begin);
                for (short i = 1000; i < omc.OggSamples.Max((sample) => sample.Value.Id); i++)
                {
                    var sample = omc.OggSamples.ContainsKey(i) ? omc.OggSamples[i] : null;
                    if (sample == null)
                        continue;

                    stream.Write(sample.Name.PadNull(32));
                    

                    if (sample.Size > 0)
                    {
                        byte[] payload = sample.Payload;
                        EncodeTo<OggEncoder>(payload);
                        sample.Payload = payload;


                        stream.Write(sample.Size);
                        stream.Write(payload);
                    }
                }

                return stream.ToArray();
            }
        }

        private static SampleInfo EncodeTo<T>(byte[] payload)
            where T : SoundEncoder
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
                else if (!(decoder is T))
                {
                    short[] buffer = new short[44100 * 2];
                    using (var sampleStream = new MemoryStream())
                    using (var encoder = SoundProcessorFactory.CreateEncoder<T>(sampleStream, 44100, 2))
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

        private static byte[] EncodeWave(byte[] encoded)
        {
            return Rearrange(AccXor(encoded));
        }

        private static byte[] Rearrange(byte[] decodedBuffer)
        {
            int length = decodedBuffer.Length;
            int key = ((length % 17) << 4) + (length % 17);

            int blockSize = length / 17;

            // Let's fill the buffer
            byte[] buffer = new byte[length];
            Array.Copy(decodedBuffer, 0, buffer, 0, length);

            for (int block = 0; block < 17; block++) // loopy loop
            {
                int decodedOffset = blockSize * REARRANGE_TABLE[key];   // Where is the start of the enconded block
                int bufferOffset = blockSize * block;  // Where the final plain block will be

                Array.Copy(decodedBuffer, decodedOffset, buffer, bufferOffset, blockSize);

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
                int accXor = ((keybyte << counter) & 0x80);

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
