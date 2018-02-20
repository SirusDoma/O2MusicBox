﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Genode.Audio
{
    /// <summary>
    /// Represents a encoder to decode specific audio format.
    /// </summary>
    public abstract class SoundEncoder : IDisposable
    {
        private bool _flushed = false;

        /// <summary>
        /// Gets the underlying <see cref="Stream"/> of the <see cref="SoundEncoder"/> instance.
        /// </summary>
        public Stream BaseStream
        {
            get; private set;
        }

        /// <summary>
        /// Gets the sample information of current <see cref="SoundDecoder"/> buffer.
        /// </summary>
        public SampleInfo SampleInfo
        {
            get; protected set;
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="SoundEncoder"/> should close the source <see cref="Stream"/> upon disposing the encoder.
        /// </summary>
        public bool OwnStream
        {
            get; private set;
        }

        /// <summary>
        /// Gets a value indicating whether the encoder has been provided with invalid <see cref="Stream"/>.
        /// </summary>
        protected internal bool Invalid
        {
            get; private set;
        }

        /// <summary>
        /// Check if current <see cref="WavDecoder"/> object can handle a give data from specified extension.
        /// </summary>
        /// <param name="stream">The extension to check.</param>
        /// <returns><code>true</code> if supported, otherwise false.</returns>
        public abstract bool Check(string extension);

        /// <summary>
        /// Initializes a new instance of the <see cref="SoundEncoder"/> class.
        /// </summary>
        /// <param name="stream"> The < see cref="Stream"/> to open.</param>
        /// <param name="ownStream">Specify whether the <see cref="SoundEncoder"/> should close the source <see cref="Stream"/> upon disposing the encoder.</param>
        public SoundEncoder(Stream stream, int sampleRate, int channelCount, bool ownStream = false)
        {
            BaseStream   = stream;
            SampleInfo   = new SampleInfo(0, channelCount, sampleRate);
            OwnStream    = ownStream;

            Invalid      = !Initialize();
        }

        /// <summary>
        /// Open a <see cref="Stream"/> of sound for writing.
        /// </summary>
        /// <returns>A value indicating whether the stream has been open successfully</returns>
        protected abstract bool Initialize();

        /// <summary>
        /// Write a block of audio samples to the current <see cref="Stream"/>.
        /// </summary>
        /// <param name="samples">The sample array to fill.</param>
        /// <param name="count">The maximum number of samples to write.</param>
        public abstract void Write(short[] samples, long count);

        /// <summary>
        /// Finalize the encoding process.
        /// It is recommended to call this before retrieving the encoded samples.
        /// </summary>
        public virtual void Flush()
        {
            _flushed = true;
        }

        /// <summary>
        /// Release all resources used by the <see cref="SoundEncoder"/>.
        /// </summary>
        public virtual void Dispose()
        {
            if (!_flushed)
                Flush();

            if (OwnStream && !Invalid)
                BaseStream?.Dispose();
        }
    }
}
