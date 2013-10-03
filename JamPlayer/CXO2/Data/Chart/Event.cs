////////////////////////////////////////
// Event                              //
// Version              : 1.0         //
// Author               : CXO2        //
//////////////////////////////////////// 

using System;
using System.Collections.Generic;
using System.Text;
using CXO2;
using CXO2.Core;
using CXO2.Core.SoundSystem;

namespace CXO2.Data
{
    // ~ Event Class
    public class Event
    {
        #region --- Type / Enums ---
        // ~ Enumeration Channels
        public enum Channels
        {
            Measurement = 0,
            BPM = 1,
            Note1 = 2,
            Note2 = 3,
            Note3 = 4,
            Note4 = 5,
            Note5 = 6,
            Note6 = 7,
            Note7 = 8,
            SampleNote = 99
        }

        // ~ Enumeration Flags
        public enum Flags
        {
            None = 0,   // None
            SLN = 1,    // Start Long Note
            ELN = 2     // End Long Note
        }
        #endregion

        #region --- Variables ---
        public int measure          ; 
        public int beat             ;
        public int cell             ;
        public Channels channel     ; 
        public double position      ;
        public Flags flag           ;
        public Event hold           ;
        public double pad           ;
        public bool judged = false  ;
        #endregion

        #region --- Constructor ---
        public Event()
        {

        }

        public Event(Event eventdata)
        {
            this.measure         = eventdata.measure;
            this.beat            = eventdata.beat;
            this.channel         = eventdata.channel;
            this.cell            = eventdata.cell;
            this.position        = eventdata.position;
            this.flag            = eventdata.flag;
            this.pad             = (double)((double)this.cell / (double)this.beat);
        }

        public Event(int measure, int beat, int pos, Channels ch)
        {
            this.measure         = measure;
            this.beat            = beat;
            this.cell            = pos;
            this.channel         = ch;
            this.flag            = Flags.None;
            this.pad             = (double)((double)this.cell / (double)this.beat);
        }

        public Event(int measure, int beat, int pos, Channels ch, Flags fl)
        {
            this.measure         = measure;
            this.beat            = beat;
            this.cell            = pos;
            this.channel         = ch;
            this.flag            = fl;
            this.pad             = (double)((double)this.cell / (double)this.beat);
        }
        #endregion

        #region --- Method ---
        // Calculate the position
        public void calculatePosition(float MEASURE_HEIGHT)
        {
            // this.position = ((this.measure * MEASURE_HEIGHT) + (((MEASURE_HEIGHT / 192f) * (192f / this.beat)) * this.cell));
            this.position = (((192 / this.beat) * this.cell) + (192 * this.measure));
        }
        #endregion

        #region --- Sample Event ---
        // ~ Sample Event Class
        public class SampleEvent : Event
        {
            #region --- Variables ---
            public int id                   ;
            public SoundSample sound        ;
            public float pan                ;
            public float vol                ;
            #endregion

            // NOTICE:
            // If the ID value equal or more than 1000, the sound was an OGG (Background Samples)

            #region --- Constructor ---
            public SampleEvent(int id, SoundSample sound, float pan, float vol, int measure, int beat, int position, Channels ch, Flags fl)
                : base(measure, beat, position, ch, fl)
            {
                // Check for Valid Channel
                if (ch == Channels.BPM | ch == Channels.Measurement)
                    throw new Exception("Invalid Event Data.");

                this.id     = id;
                this.sound  = sound;
                this.pan    = pan;

                // Volume Reduction
                this.vol    = vol / 2;
            }

            public SampleEvent(SampleEvent e)
                : base(e)
            {
                // Check for Valid Channel
                if (e.channel == Channels.BPM | e.channel == Channels.Measurement)
                    throw new Exception("Invalid Event Data.");

                this.id     = e.id;
                this.sound  = e.sound;
                this.pan    = e.pan;
                this.vol    = e.vol;
            }

            public SampleEvent(Event e, int id, float pan, float vol)
                : base(e)
            {
                // Check for Valid Channel
                if (e.channel == Channels.BPM | e.channel == Channels.Measurement)
                    throw new Exception("Invalid Event Data.");

                this.id     = id;
                this.sound  = null;
                this.pan    = pan;
                this.vol    = vol;
            }

            public SampleEvent(Event e, int id, float pan, float vol, SoundSample sound)
                : base(e)
            {
                // Check for Valid Channel
                if (e.channel == Channels.BPM | e.channel == Channels.Measurement)
                    throw new Exception("Invalid Event Data.");

                this.id     = id;
                this.sound  = sound;
                this.pan    = pan;
                this.vol    = vol;
            }
            #endregion

            #region --- Deconstructor ---
            // Release the related sound
            public void Dispose()
            {
                // Release if only the sound isnt null
                if (this.sound != null)
                    this.sound.Dispose();
            }
            #endregion
        }
        #endregion

        #region --- Time Event ---
        // ~ Time Event Class
        public class TimeEvent : Event
        {
            #region --- Variables ---
            public float Value ;
            #endregion

            #region --- Constructor ---
            public TimeEvent(float value, int measure, int beat, int position, Channels ch)
                : base(measure, beat, position, ch)
            {
                // Check for Valid Channel
                if (ch != Channels.BPM && ch != Channels.Measurement)
                    throw new Exception("Invalid Event Data.");

                Value = value;
            }

            public TimeEvent(float value, Event e)
                : base(e)
            {
                // Check for Valid Channel
                if (e.channel != Channels.BPM && e.channel != Channels.Measurement)
                    throw new Exception("Invalid Event Data.");

                Value = value;
            }
            #endregion
        }
        #endregion
    }
}
