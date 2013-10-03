using System;
using System.Collections.Generic;
using System.Text;
using CXO2;
using CXO2.Core;
using CXO2.Core.SoundSystem;

namespace CXO2.Data
{
    public class Chart
    {
        #region --- Type / Enum ---
        // ~ Enumeration Mode
        public enum Modes
        {
            EX = 0,
            NX = 1,
            HX = 2
        }

        // ~ Enumeration Chart format
        public enum ChartFormat
        {
            OJN = 0,
            BMS = 1
        }
        #endregion

        #region --- Variables ---
        public string title;
        public string artist;
        public string pattern;
        public double bpm;
        public double measureHeight = 384.0D;
        public ChartFormat chartType;
        public Dictionary<Modes, int> level;
        public Dictionary<Modes, XList<Event>> dataEvent;
        public Dictionary<int, SoundSample> dataSamples;
        #endregion

        #region --- Constructor ---
        public Chart()
        {
            this.title          = "Untitled";
            this.artist         = "Unknown";
            this.pattern        = "Unknown";
            this.level          = new Dictionary<Modes, int>();
            this.dataEvent      = new Dictionary<Modes, XList<Event>>();
            this.dataSamples    = new Dictionary<int, SoundSample>();
        }

        public Chart(string title, string artist, string pattern)
        {
            this.title          = title;
            this.artist         = artist;
            this.pattern        = pattern;
        }

        public Chart(string title, string artist, string pattern, float bpm, Dictionary<Modes, int> level, Dictionary<Modes, XList<Event>> dataEvent, Dictionary<int, SoundSample> dataSamples, ChartFormat type)
        {
            this.title          = title;
            this.artist         = artist;
            this.pattern        = pattern;
            this.bpm            = bpm;
            this.level          = level;
            this.dataEvent      = dataEvent;
            this.dataSamples    = dataSamples;
            this.chartType      = type;
        }
        #endregion

        #region --- Methods ---
        // Get Sample Event
        public  XList<Event.SampleEvent> getSampleEvents(Modes mode, int id = -1)
        {
            // List sample event to be returned
            XList<Event.SampleEvent> result = new XList<Event.SampleEvent>();

            // Loop for specified mode only
            foreach (Event e in dataEvent[mode])
            {
                // is the Event is SampleEvent?
                if (e.GetType() == typeof(Event.SampleEvent))
                {
                    // Convert it
                    Event.SampleEvent se = (Event.SampleEvent)e;

                    // if requested id is -1, then get all Sample Event
                    if (id == -1)
                        result.Add(se);
                    // no, its not -1, get only specified Sample Event
                    if (se.id == id)
                        result.Add(se);
                }
            }

            // return all list samples
            return result;
        }

        // Get the Time Event
        public XList<Event.TimeEvent> getTimeEvents(Modes mode)
        {
            // List time event to be returned
            XList<Event.TimeEvent> result = new XList<Event.TimeEvent>();

            // Loop for specified mode only
            foreach (Event e in dataEvent[mode])
            {
                // is the Event is TimeEvent?
                if (e.GetType() == typeof(Event.TimeEvent))
                {
                    // Convert it
                    Event.TimeEvent te = (Event.TimeEvent)e;
                    
                    // Add it
                    result.Add(te);
                }
            }

            // Return it
            return result;
        }

        // Get all Event in specified mode
        public XList<Event> getEvents(Modes mode)
        {
            // return it
            return dataEvent[mode];
        }
        #endregion
    }
}
