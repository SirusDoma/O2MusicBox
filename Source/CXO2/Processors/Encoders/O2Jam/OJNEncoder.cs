using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

using Cgen;
using Cgen.IO;

using CXO2;
using CXO2.Charting;
using CXO2.Charting.O2Jam;

namespace CXO2.Processors.O2Jam
{
    public static class OJNEncoder
    {
        public static bool Check(string extension)
        {
            return extension == "ojn" || extension == "new.ojn";
        }

        public static byte[] Encode(OJN ojn)
        {
            byte[] header = EncodeHeader(ojn);
            using (var stream = new BufferStream(header))
            {
                foreach (var difficulty in Enum.GetValues(typeof(OJN.Difficulty)) as OJN.Difficulty[])
                {
                    if (difficulty == OJN.Difficulty.MX)
                        continue;

                    var events  = new List<Event>();
                    long offset = ojn.BlockOffset[difficulty];
                    int count   = ojn.BlockCount[difficulty];

                    stream.Seek(offset, SeekOrigin.Begin);
                    var blocks = ojn.Events[difficulty].GroupBy((ev) => new { ev.Measure, ev.Tempo, ev.LaneIndex });
                    foreach (var block in blocks)
                    {
                        stream.Write(block.Key.Measure);
                        stream.Write((short)block.Key.LaneIndex);
                        stream.Write((short)block.Key.Tempo);

                        for (int i = 0; i < block.Key.Tempo; i++)
                        {
                            var ev = block.FirstOrDefault((e) => e.Cell == i);
                            if (ev != null)
                            {
                                if (ev.Channel == Event.ChannelType.Measurement ||
                                    ev.Channel == Event.ChannelType.BPM)
                                {
                                    var time = ev as Event.Time;
                                    stream.Write(time.Value);
                                }
                                else
                                {
                                    var sound = ev as Event.Sound;
                                    int id  = sound.Id;
                                    int vol = sound.Volume == 100 ? 0 : (int)((sound.Volume / 100f) * 16f);
                                    int pan = (int)sound.Pan;

                                    id  = (id >= 1000 ? id - 1000 : id) + 1;
                                    pan = pan == 0 ? pan : ((pan / 100) * 8) + 8;

                                    stream.Write((short)id);
                                    stream.Write((byte)((vol << 8) | pan & 0x00FF));
                                    stream.Write((byte)((int)sound.Signature));
                                }
                            }
                            else
                            {
                                stream.Write(0);
                            }
                        }
                    }
                }

                stream.Seek(ojn.CoverArtOffset, SeekOrigin.Begin);
                stream.Write(ojn.CoverArt);
                stream.Write(ojn.Thumbnail);

                return stream.ToArray();
            }
        }

        public static byte[] EncodeHeader(OJN ojn)
        {
            using (var stream = new BufferStream())
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.Write(ojn.Id);
                stream.Write(ojn.Sign.PadNull(4));
                stream.Write(ojn.EncodingVersion);
                stream.Write((int)ojn.Genre);
                stream.Write(ojn.BPM);

                foreach (var diff in Enum.GetValues(typeof(OJN.Difficulty)) as OJN.Difficulty[])
                {
                    if (diff != OJN.Difficulty.MX)
                        stream.Write(ojn.Level[diff]);
                    else
                        stream.Write((short)0);
                }

                foreach (var diff in Enum.GetValues(typeof(OJN.Difficulty)) as OJN.Difficulty[])
                {
                    if (diff != OJN.Difficulty.MX)
                        stream.Write(ojn.EventCount[diff]);
                }

                foreach (var diff in Enum.GetValues(typeof(OJN.Difficulty)) as OJN.Difficulty[])
                {
                    if (diff != OJN.Difficulty.MX)
                        stream.Write(ojn.NoteCount[diff]);
                }

                foreach (var diff in Enum.GetValues(typeof(OJN.Difficulty)) as OJN.Difficulty[])
                {
                    if (diff != OJN.Difficulty.MX)
                        stream.Write(ojn.MeasureCount[diff]);
                }

                foreach (var diff in Enum.GetValues(typeof(OJN.Difficulty)) as OJN.Difficulty[])
                {
                    if (diff != OJN.Difficulty.MX)
                        stream.Write(ojn.BlockCount[diff]);
                }

                stream.Write(ojn.OldEncodingVersion);
                stream.Write(ojn.OldId);
                stream.Write(ojn.OldGenre.PadNull(20));

                stream.Write(ojn.ThumbnailSize);
                stream.Write(ojn.Version);

                stream.Write(ojn.Title.PadNull(64));
                stream.Write(ojn.Artist.PadNull(32));
                stream.Write(ojn.Pattern.PadNull(32));
                stream.Write(ojn.OJMFileName.PadNull(32));

                stream.Write(ojn.CoverArtSize);
                foreach (var diff in Enum.GetValues(typeof(OJN.Difficulty)) as OJN.Difficulty[])
                {
                    if (diff != OJN.Difficulty.MX)
                        stream.Write(ojn.Duration[diff]);
                }

                foreach (var diff in Enum.GetValues(typeof(OJN.Difficulty)) as OJN.Difficulty[])
                {
                    if (diff != OJN.Difficulty.MX)
                        stream.Write(ojn.BlockOffset[diff]);
                }

                stream.Write(ojn.CoverArtOffset);

                //stream.Seek(ojn.CoverArtOffset, SeekOrigin.Begin);
                //stream.Write(ojn.CoverArt);
                //stream.Write(ojn.Thumbnail);

                return stream.ToArray();
            }
        }
    }
}
