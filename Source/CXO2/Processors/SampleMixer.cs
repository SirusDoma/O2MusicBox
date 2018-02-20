using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Genode;
using Genode.Audio;

using CXO2;
using CXO2.Charting;

namespace CXO2.Processors
{
    /// <summary>
    /// Provides a mixing function for samples of <see cref="Chart"/>.
    /// </summary>
    public static class SampleMixer
    {
        public enum MixMode
        {
            /// <summary>
            /// Full - Mix all samples.
            /// </summary>
            Full     = 0,
            /// <summary>
            /// BGM - Only mix bgm samples.
            /// </summary>
            BGM      = 1,
            /// <summary>
            /// Keysound - Only mix keysound samples.
            /// </summary>
            Keysound = 2
        }

        /// <summary>
        /// Mix Sound sample of given <see cref="Chart"/>. 
        /// Resulting a single <see cref="MixedSample"/> of prerendered the <see cref="Chart"/>.
        /// </summary>
        /// <param name="chart">a <see cref="Chart"/> instance that contain samples to be mixed and prerendered.</param>
        /// <param name="mode">Specifies sample mixing mode.</param>
        /// <param name="channelCount">The number of channels that will be used for prerendering the <see cref="Chart"/>.</param>
        /// <param name="sampleRate">The rate of sample that will be used for prerendering the <see cref="Chart"/>.</param>
        /// <returns>a Single <see cref="MixedSample"/> of prerendered given <see cref="Chart"/></returns>
        public static MixedSample Mix(Chart chart, MixMode mode = MixMode.Full, int channelCount = 2, int sampleRate = 44100)
        {
            int sampleCount = (sampleRate * channelCount);
            var samples = new short[sampleCount * (int)chart.Duration.TotalSeconds];
            var events = Array.ConvertAll(Array.FindAll(chart.Events, (ev) => ev is Event.Sound), ((ev) => ev as Event.Sound));

            foreach (var ev in events)
            {
                if (ev.Payload == null || ev.Signature == Event.SignatureType.Release || (mode == MixMode.BGM  && ev.Channel != Event.ChannelType.BGM) || (mode == MixMode.Keysound && ev.Channel == Event.ChannelType.BGM))
                {
                    continue;
                }

                using (var stream = new MemoryStream(ev.Payload))
                using (var decoder = SoundProcessorFactory.CreateDecoder(stream))
                {
                    var info = decoder.SampleInfo;

                    var keysound = new short[info.SampleCount];
                    long offset = (long)(ev.Timestamp.TotalSeconds * sampleCount);
                    long read = 0;

                    while ((read = decoder.Read(keysound, info.SampleCount)) > 0)
                    {
                        if (offset + read >= samples.Length)
                        {
                            int newSize = samples.Length;
                            while (newSize < offset + read)
                            {
                                newSize += sampleRate * channelCount;
                            }

                            Array.Resize(ref samples, newSize);
                        }

                        for (int i = 0; i < read; i++)
                        {
                            int buffer = samples[offset + i] + keysound[i];
                            if (buffer > short.MaxValue)
                                buffer = short.MaxValue;
                            else if (buffer < short.MinValue)
                                buffer = short.MinValue;

                            samples[offset + i] = (short)buffer;
                        }

                        offset += read;
                    }
                }
            }

            return new MixedSample(samples, channelCount, sampleRate);
        }
    }
}
