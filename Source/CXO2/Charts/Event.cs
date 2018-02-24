using System;
using System.Collections.Generic;
using System.Text;

using Cgen;
using Cgen.Audio;

namespace CXO2.Charting
{
    public class Event
    {
        public enum ChannelType : short
        {
            Measurement = 0,
            BPM    = 1,
            Note1  = 2,
            Note2  = 3,
            Note3  = 4,
            Note4  = 5,
            Note5  = 6,
            Note6  = 7,
            Note7  = 8,
            Note8  = 9,
            Note9  = 10,
            Note10 = 11,
            BGM
        }

        public enum SignatureType
        {
            Normal  = 0,
            Hold    = 2,
            Release = 3,
            BGM     = 4
        }

        public int Tempo
        {
            get; set;
        }

        public int Measure
        {
            get; set;
        }

        public int Beat
        {
            get; set;
        }

        public int Cell
        {
            get; set;
        }

        public ChannelType Channel
        {
            get; set;
        }

        public int LaneIndex
        {
            get; set;
        }

        public SignatureType Signature
        {
            get; set;
        }

        public double Offset
        {
            get
            {
                return ((192 / Tempo) * Cell) + (192 * Measure);
            }
        }

        public TimeSpan Timestamp
        {
            get; private set;
        }

        public bool Judged
        {
            get; protected set;
        }

        public Event()
        {
            Measure   = 0;
            Beat      = 0;
            Cell      = 0;
            Tempo     = 4;
            Channel   = ChannelType.BGM;
            Signature = SignatureType.Normal;
        }

        public Event(Event data)
        {
            Measure   = data.Measure;
            Beat      = data.Beat;
            Cell      = data.Cell;
            Tempo     = data.Tempo;
            Channel   = data.Channel;
            Signature = data.Signature;
        }

        public Event(Event data, TimeSpan timestamp)
            : this(data)
        {
            Timestamp = timestamp;
        }

        public Event(Event.ChannelType channel, Event.SignatureType flag)
            : this()
        {
            Channel   = channel;
            Signature = flag;
        }

        public void Judge()
        {
            Judged = true;
        }

        public class Sound : Event, IDisposable
        {
            public int Id
            {
                get; set;
            }

            public string Name
            {
                get; set;
            }

            public Event.Sound Pair
            {
                get; set;
            }

            public float Pan
            {
                get; set;
            }

            public float Volume
            {
                get; set;
            }

            public byte[] Payload
            {
                get; set;
            }

            public SoundSource Sample
            {
                get; set;
            }

            public Sound()
                : base()
            {
            }

            public Sound(Event ev)
                : base(ev)
            {
            }

            public Sound(Event.Sound ev)
                : this(ev, TimeSpan.Zero)
            {
            }

            public Sound(Event.Sound ev, TimeSpan timestamp)
                : base(ev, timestamp)
            {
                Id      = ev.Id;
                Pair    = ev.Pair;
                Volume  = ev.Volume;
                Pan     = ev.Pan;
                Payload = ev.Payload;
                Sample  = ev.Sample;
            }

            public Sound(Event.ChannelType channel, Event.SignatureType flag)
                : base(channel, flag)
            {
            }

            public void Play()
            {
                if (Sample == null && Payload != null)
                {
                    Sample = new Music(Payload);
                    SoundSystem.Instance.Play(Sample);
                }

                if (Sample != null)
                {
                    SoundSystem.Instance.Play(Sample);
                    //Sample.Volume = Volume;
                    //Sample.Pan    = Pan;
                }
            }

            public void Stop()
            {
                if (Sample != null)
                    SoundSystem.Instance.Stop(Sample);
            }

            public void Dispose()
            {
                Sample?.Dispose();
            }
        }

        public class Time : Event
        {
            public float Value
            {
                get; set;
            }

            public Time(Event ev)
                : base(ev)
            {
            }

            public Time(Event.Time ev)
                : this(ev, TimeSpan.Zero)
            {
            }

            public Time(Event.Time ev, TimeSpan timestamp)
                : base(ev, timestamp)
            {
                Value = ev.Value;
            }

            public Time(Event.ChannelType channel, Event.SignatureType flag)
                : base(channel, flag)
            {
            }
        }
    }
}
