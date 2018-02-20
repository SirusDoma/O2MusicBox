using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Genode.Audio
{
    public class OggEncoder : SoundEncoder
    {
        /// <summary>
        /// Check if current <see cref="OggDecoder"/> object can handle a give data from specified extension.
        /// </summary>
        /// <param name="extension">The extension to check.</param>
        /// <returns><code>true</code> if supported, otherwise false.</returns>
        public override bool Check(string extension)
        {
            return extension.ToLower().EndsWith("ogg");
        }

        private VorbisEncoder.OggStream _ogg;
        private VorbisEncoder.VorbisInfo _vorbis;
        private VorbisEncoder.ProcessingState _state;

        /// <summary>
        /// Initializes a new instance of <see cref="OggEncoder"/> class.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="sampleRate"></param>
        /// <param name="channelCount"></param>
        /// <param name="ownStream"></param>
        public OggEncoder(Stream stream, int sampleRate, int channelCount, bool ownStream = false) 
            : base(stream, sampleRate, channelCount, ownStream)
        {
        }

        protected override bool Initialize()
        {
            // Reset stream position
            BaseStream.Seek(0, SeekOrigin.Begin);

            // Initialize the ogg/vorbis stream
            _ogg = new VorbisEncoder.OggStream(new Random().Next());

            // Setup the encoder: VBR, automatic bitrate management
            // Quality is in range [-1 .. 1], 0.4 gives ~128 kbps for a 44 KHz stereo sound
            _vorbis = VorbisEncoder.VorbisInfo.InitVariableBitRate(SampleInfo.ChannelCount, SampleInfo.SampleRate, 0.1f);

            // Generate header metadata (leave it empty)
            var headerBuilder = new VorbisEncoder.HeaderPacketBuilder();
            var comments = new VorbisEncoder.Comments();

            // Generate the header packets
            var infoPacket = headerBuilder.BuildInfoPacket(_vorbis);
            var commentsPacket = headerBuilder.BuildCommentsPacket(comments);
            var booksPacket = headerBuilder.BuildBooksPacket(_vorbis);

            // Write the header packets to the ogg stream
            _ogg.PacketIn(infoPacket);
            _ogg.PacketIn(commentsPacket);
            _ogg.PacketIn(booksPacket);

            // Flush to force audio data onto its own page per the spec
            VorbisEncoder.OggPage page;
            while (_ogg.PageOut(out page, true))
            {
                BaseStream.Write(page.Header, 0, page.Header.Length);
                BaseStream.Write(page.Body, 0, page.Body.Length);
            }

            _state = VorbisEncoder.ProcessingState.Create(_vorbis);
            return true;
        }

        public override void Write(short[] samples, long count)
        {
            if (_ogg.Finished)
                return;

            // Vorbis has issues with buffers that are too large, so we ask for 64K
            int bufferSize = 65536;

            // A frame contains a sample from each channel
            int frameCount = (int)count / SampleInfo.ChannelCount;

            var data = new List<byte>();
            for (int i = 0; i < samples.Length; i++)
                data.AddRange(BitConverter.GetBytes(samples[i]));

            bufferSize = data.Count / 4;
            long writeCount = 0;
            while (frameCount > 0)
            {
                // Prepare a buffer to hold samples
                var buffer = new float[SampleInfo.ChannelCount][];
                for (int channel = 0; channel < SampleInfo.ChannelCount; channel++)
                    buffer[channel] = new float[Math.Min(frameCount, bufferSize)];

                for (var i = 0; i < data.Count / 4; i++)
                {
                    // uninterleave samples
                    buffer[0][i] = (short)((data[i * 4 + 1] << 8) | (0x00ff & data[i * 4])) / 32768f;
                    buffer[1][i] = (short)((data[i * 4 + 3] << 8) | (0x00ff & data[i * 4 + 2])) / 32768f;

                    writeCount += 2;
                }

                // Tell the library how many samples we've written
                _state.WriteData(buffer, data.Count / 4);

                frameCount -= bufferSize;

                // Flush any produced block
                FlushBlock();
            }

            SampleInfo = new SampleInfo((int)(SampleInfo.SampleCount + writeCount), SampleInfo.ChannelCount, SampleInfo.SampleRate);
        }

        public void FlushBlock()
        {
            if (_ogg.Finished)
                return;

            // Get new packets from the bitrate management engine
            VorbisEncoder.OggPacket packet;
            while (_state.PacketOut(out packet))
            {
                // Write the packet to the ogg stream
                _ogg.PacketIn(packet);

                // If the stream produced new pages, write them to the output file
                VorbisEncoder.OggPage page;
                while (_ogg.PageOut(out page, false))
                {
                    BaseStream.Write(page.Header, 0, page.Header.Length);
                    BaseStream.Write(page.Body, 0, page.Body.Length);
                }
            }
        }

        public override void Flush()
        {
            base.Flush();

            _state.WriteEndOfStream();
        }

        public override void Dispose()
        {
            base.Dispose();

            _ogg    = null;
            _state  = null;
            _vorbis = null;
        }
    }
}
