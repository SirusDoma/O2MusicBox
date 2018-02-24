using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Cgen;
using Cgen.Audio;

namespace CXO2.Charting
{
    /// <summary>
    /// Represents a prerendererd music of a <see cref="Chart"/>.
    /// </summary>
    public class MixedSample : SoundStream
    {
        private TimeSpan _duration;
        private short[] _samples;
        private int _sampleCount;
        private long _offset;
        private SampleInfo _info;

        /// <summary>
        /// Gets total duration of current <see cref="MixedSample"/> object.
        /// </summary>
        public override TimeSpan Duration
        {
            get
            {
                return _duration;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MixedSample"/> class.
        /// </summary>
        public MixedSample(short[] samples, int channelCount, int sampleRate)
            : base()
        {
            _samples = samples;
            _info = new SampleInfo(samples.Length, channelCount, sampleRate);
            Initialize(_info.ChannelCount, _info.SampleRate);
        }

        /// <summary>
        /// Request a new chunk of audio samples from the stream source.
        /// </summary>
        /// <param name="samples">The audio chunk that contains audio samples.</param>
        /// <returns><code>true</code> if reach the end of stream, otherwise false.</returns>
        protected override bool OnGetData(out short[] samples)
        {
            // Fill the chunk parameters
            samples     = new short[_sampleCount];
            long count  = _offset + _sampleCount < _samples.Length ? _sampleCount : _samples.Length - _offset;

            //long count = _decoder.Read(samples, samples.Length);
            //Array.Copy(_samples, _offset, samples, 0, count);
            for (int i = 0; i < count; i++)
                samples[i] = _samples[_offset + i];
            _offset += count;

            // Remove the gap when processing last buffer
            Array.Resize(ref samples, (int)count);

            // Check if we have reached the end of the audio file
            return count == _sampleCount;
        }

        /// <summary>
        /// Change the current playing position in the stream source.
        /// </summary>
        /// <param name="time">Seek to specified time.</param>
        protected override void OnSeek(TimeSpan time)
        {
            _offset = (long)time.TotalSeconds * SampleRate * ChannelCount;
        }

        /// <summary>
        /// Performs initialization steps by providing the audio stream parameters.
        /// </summary>
        /// <param name="channelCount">The number of channels of current <see cref="SoundStream"/> object.</param>
        /// <param name="sampleRate">The sample rate, in samples per second.</param>
        protected override void Initialize(int channelCount, int sampleRate)
        {
            // Compute the music duration
            _duration = TimeSpan.FromSeconds(_info.SampleCount / channelCount / (float)sampleRate);

            // Set the internal buffer size so that it can contain 1 second of audio samples
            _sampleCount = sampleRate * channelCount;

            // Initialize the stream
            base.Initialize(channelCount, sampleRate);
        }

        /// <summary>
        /// Get prerendered mixed samples.
        /// </summary>
        /// <returns>Prerendered mixed xamples.</returns>
        public short[] GetSamples()
        {
            return _samples;
        }

        /// <summary>
        /// Releases all resources used by the <see cref="MixedSample"/>.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
