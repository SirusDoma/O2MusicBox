#if !NO_GENODE
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Genode;
using Genode.Audio;

namespace O2MusicBox
{
    public class MP3Encoder : SoundEncoder
    {
        private Lame.LameMP3FileWriter _encoder;

        public override bool Check(string extension)
        {
            return extension.EndsWith("mp3");
        }

        public MP3Encoder(Stream stream, int sampleRate = 44100, int channelCount = 2, bool ownStream = false)
            : base(stream, sampleRate, channelCount, ownStream)
        {
        }

        protected override bool Initialize()
        {
            _encoder = new Lame.LameMP3FileWriter(BaseStream, WavFormat.PCM, Lame.LAMEPreset.STANDARD, SampleInfo.ChannelCount, SampleInfo.SampleRate);
            return true;
        }

        public override void Write(short[] samples, long count)
        {
            long writeCount = 0;
            for (int i = 0; i < count; i++)
            {
                if (i >= samples.Length)
                    break;

                var sample = BitConverter.GetBytes(samples[i]);
                _encoder.Write(sample, 0, sample.Length);
                writeCount++;
            }

            SampleInfo = new SampleInfo((int)(SampleInfo.SampleCount + writeCount), SampleInfo.ChannelCount, SampleInfo.SampleRate);
        }

        public override void Flush()
        {
            base.Flush();
            _encoder.Flush();
        }
    }
}
#endif
