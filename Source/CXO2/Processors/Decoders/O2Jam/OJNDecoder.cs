using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Cgen;
using Cgen.IO;

using CXO2;
using CXO2.Charting;
using CXO2.Charting.O2Jam;

namespace CXO2.Processors.O2Jam
{
    public static class OJNDecoder
    {
        public static bool Check(string filename)
        {
            using (var stream = new BufferStream(filename))
            {
                stream.Seek(0, SeekOrigin.Begin);

                int id = stream.ReadInt32();
                string sign = stream.ReadString(4, default(Encoding), false);

                return id > 0 && sign == OJN.HEADER_SIGNATURE;
            }
        }

        public static OJN Decode(string filename)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException("OJN File not found", filename);

            if (!Check(filename))
                throw new FormatException("Invalid O2Jam Chart.");

            var ojn = DecodeHeader(filename);
            using (var stream = new BufferStream(filename))
            {
                ojn.Events = new Dictionary<OJN.Difficulty, Event[]>();
                foreach (var difficulty in Enum.GetValues(typeof(OJN.Difficulty)) as OJN.Difficulty[])
                {
                    if (difficulty == OJN.Difficulty.MX)
                        continue;

                    var events = new List<Event>();
                    int offset = ojn.BlockOffset[difficulty];
                    int count  = ojn.BlockCount[difficulty];

                    stream.Seek(offset, SeekOrigin.Begin);
                    for (int block = 0; block < count; block++)
                    {
                        var ev = new Event
                        {
                            Measure   = stream.ReadInt32(),
                            LaneIndex = stream.ReadInt16(),
                            Tempo     = stream.ReadInt16()
                        };

                        // We don't convert the lane index directly to channel, we keep them both
                        // This allow the Event class to be usable for various number of channels (e.g: 3key / 5key / 10key)
                        // The decoder is responsible to convert this index into correct channel enum
                        // Lane Index then still preserved, this might needed in certain cases such as encoding the chart back to the file or identify the lane index of BGM.
                        // While channel may make judgement and rendering process easier by distinguish event types between playable and background events
                        ev.Channel = ev.LaneIndex > (int)Event.ChannelType.Note7 ? Event.ChannelType.BGM : (Event.ChannelType)ev.LaneIndex;

                        for (int i = 0; i < ev.Tempo; i++)
                        {
                            ev.Beat = (int)(i * (192f / ev.Tempo));
                            ev.Cell = i;

                            if (ev.Channel == Event.ChannelType.BPM ||
                                ev.Channel == Event.ChannelType.Measurement)
                            {
                                var time = new Event.Time(ev) {
                                    Value = stream.ReadSingle()
                                };

                                if (time.Value != 0)
                                    events.Add(time);
                            }
                            else
                            {
                                int refId = stream.ReadInt16();
                                if ((int)ev.Channel < 0 || refId <= 0)
                                {
                                    stream.Seek(2, SeekOrigin.Current);
                                    continue;
                                }

                                int audio = stream.ReadByte();
                                int flag = stream.ReadByte();
                                var type = (Event.SignatureType)flag;

                                float volume = ((audio >> 4) & 0x0F);
                                volume = volume == 0 ? 100 : ((volume / 16f) * 100f);

                                float pan = (audio & 0x0F);
                                if (pan == 0)
                                    pan = 8;
                                pan = ((pan - 8) / 8f) * 100;

                                int id = (refId - 1) + (flag % 8 > 3 ? 1000 : 0);
                                var sample = new Event.Sound(ev)
                                {
                                    Id = id,
                                    Signature = type,
                                    Pan = pan,
                                    Volume = volume
                                };


                                events.Add(sample);
                            }
                        }
                    }

                    // Long Note Pairing
                    foreach (var ev in events.FindAll((e) => e is Event.Sound && e.Signature == Event.SignatureType.Hold))
                    {
                        var sln = ev as Event.Sound;
                        var eln = events.Find((e) =>
                        {
                            var se = e as Event.Sound;
                            return se != null &&
                                   se.Pair == null &&
                                   se.Signature == Event.SignatureType.Release &&
                                   se.Channel == sln.Channel;

                        }) as Event.Sound;

                        if (eln != null)
                        {
                            sln.Pair = eln;
                            eln.Pair = sln;
                        }
                    }

                    // Clean up
                    events.RemoveAll((e) => {
                        var se = e as Event.Sound;
                        return se != null && ((se.Signature == Event.SignatureType.Hold && se.Pair == null) /*|| se.Signature == Event.SignatureType.Release*/);
                    });

                    // Sort event based on offset
                    events.Sort((a, b) => a.Offset.CompareTo(b.Offset));

                    // Calculate Timestamp
                    var timeEvents = events.FindAll((e) => e.Channel == Event.ChannelType.BPM);
                    for (int i = 0; i < events.Count; i++)
                    {
                        var ev = events[i] as Event.Sound;
                        if (ev == null)
                            continue;

                        double bpm         = ojn.BPM;
                        double timeOffset  = 0;
                        double bpmPosition = 0;
                        foreach (var time in timeEvents)
                        {
                            if (time.Offset < ev.Offset)
                            {
                                timeOffset += ((time.Offset - bpmPosition) / (192f / 4f)) * (60f / bpm);

                                bpm         = ((Event.Time)time).Value;
                                bpmPosition = time.Offset;
                            }
                            else if (time.Offset > ev.Offset)
                            {
                                break;
                            }
                        }

                        var timestamp = TimeSpan.FromSeconds(timeOffset + (((ev.Offset - bpmPosition) / (192f / 4f)) * (60f / bpm))); ;
                        events[i]     = new Event.Sound(ev, timestamp);
                    }

                    ojn.Events[difficulty] = events.ToArray();
                }

                return ojn;
            }
        }

        public static OJN DecodeHeader(string filename)
        {
            var ojn = new OJN();
            using (var stream = new BufferStream(filename))
            {
                stream.Seek(0, SeekOrigin.Begin);
                ojn.Id    = stream.ReadInt32();
                ojn.Sign  = stream.ReadString(4);
                ojn.EncodingVersion = stream.ReadSingle();
                ojn.Genre = (OJN.Genres)stream.ReadInt32();
                ojn.BPM   = stream.ReadSingle();

                ojn.Level = new Dictionary<OJN.Difficulty, short>
                {
                    [OJN.Difficulty.EX] = stream.ReadInt16(),
                    [OJN.Difficulty.NX] = stream.ReadInt16(),
                    [OJN.Difficulty.HX] = stream.ReadInt16(),
                    [OJN.Difficulty.MX] = stream.ReadInt16()
                };

                ojn.EventCount = new Dictionary<OJN.Difficulty, int>
                {
                    [OJN.Difficulty.EX] = stream.ReadInt32(),
                    [OJN.Difficulty.NX] = stream.ReadInt32(),
                    [OJN.Difficulty.HX] = stream.ReadInt32()
                };

                ojn.NoteCount = new Dictionary<OJN.Difficulty, int>
                {
                    [OJN.Difficulty.EX] = stream.ReadInt32(),
                    [OJN.Difficulty.NX] = stream.ReadInt32(),
                    [OJN.Difficulty.HX] = stream.ReadInt32()
                };

                ojn.MeasureCount = new Dictionary<OJN.Difficulty, int>
                {
                    [OJN.Difficulty.EX] = stream.ReadInt32(),
                    [OJN.Difficulty.NX] = stream.ReadInt32(),
                    [OJN.Difficulty.HX] = stream.ReadInt32()
                };

                ojn.BlockCount = new Dictionary<OJN.Difficulty, int>
                {
                    [OJN.Difficulty.EX] = stream.ReadInt32(),
                    [OJN.Difficulty.NX] = stream.ReadInt32(),
                    [OJN.Difficulty.HX] = stream.ReadInt32()
                };

                ojn.OldEncodingVersion = stream.ReadInt16();
                ojn.OldId = stream.ReadInt16();
                ojn.OldGenre = stream.ReadString(20);

                int thumbnailSize = stream.ReadInt32();
                ojn.Version = stream.ReadInt32();

                ojn.Title = stream.ReadString(64);
                ojn.Artist = stream.ReadString(32);
                ojn.Pattern = stream.ReadString(32);
                ojn.OJMFileName = stream.ReadString(32);

                int coverArtSize = stream.ReadInt32();

                ojn.Duration = new Dictionary<OJN.Difficulty, int>
                {
                    [OJN.Difficulty.EX] = stream.ReadInt32(),
                    [OJN.Difficulty.NX] = stream.ReadInt32(),
                    [OJN.Difficulty.HX] = stream.ReadInt32()
                };

                ojn.BlockOffset = new Dictionary<OJN.Difficulty, int>
                {
                    [OJN.Difficulty.EX] = stream.ReadInt32(),
                    [OJN.Difficulty.NX] = stream.ReadInt32(),
                    [OJN.Difficulty.HX] = stream.ReadInt32()
                };

                ojn.CoverArtOffset = stream.ReadInt32();

                stream.Seek(ojn.CoverArtOffset, SeekOrigin.Begin);
                ojn.CoverArt = stream.ReadBytes(coverArtSize);
                ojn.Thumbnail = stream.ReadBytes(thumbnailSize);

                return ojn;
            }
        }
    }
}
