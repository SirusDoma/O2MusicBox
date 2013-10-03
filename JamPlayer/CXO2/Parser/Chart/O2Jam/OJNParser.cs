////////////////////////////////////////
// OJN Parser                         //
// Version              : 1.2         //
// Author               : CXO2        //
//////////////////////////////////////// 

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using CXO2;
using CXO2.Core;
using CXO2.Core.SoundSystem;
using CXO2.Data;
using CXO2.IO;

namespace CXO2.Parser
{
    // ~ OJN Parser
    public static class OJNParser
    {
        #region --- Const ---
        // O2Jam Based (Because its OJN Parser lol)
        public const float MEASURE_HEIGHT   = 384.0f;
        public const int OJN_SIGNATURE      = 0x006E6A6F;
        #endregion

        #region --- isOJN ---

        // Validate OJN

        public static bool isOJN(string path, ref OJN h)
        {
            if (!parseHeader(path, ref h))
                return false;

            if (path.ToLower().EndsWith(".ojn") && h.sign == OJN_SIGNATURE)
                return true;

            return false;    
        }

        public static bool isOJN(byte[] data, ref OJN h)
        {
            if (!parseHeader(data, ref h))
                return false;

            if (h.sign == OJN_SIGNATURE)
                return true;

            return false;
        }
        #endregion

        #region --- Parse Header ---
        
        // Parse the OJN Header Struct

        public static bool parseHeader(string path, ref OJN h)
        {
            StreamEngine buffer = new StreamEngine(path);
            byte[] data = buffer.getAllBuffer();
            buffer.Free();

            bool result = parseHeader(data , ref h);

            if (result)
                h.file = path;
            else
                return false;

            return true;
        }

        public static bool parseHeader(byte[] data, ref OJN h)
        {
            StreamMemoryEngine buffer = null;

            try
            {
                buffer = new StreamMemoryEngine(data);

                buffer.Position = 0;
                h.id                                    = buffer.getInt();
                h.sign                                  = buffer.getInt();
                h.encodeOJN                             = buffer.getFloat();
                h.genre                                 = (OJN.Genres)buffer.getInt();
                h.bpm                                   = buffer.getFloat();
                h.level                                 = new short[3];
                h.level[(int)Chart.Modes.EX]            = buffer.getShort();
                h.level[(int)Chart.Modes.NX]            = buffer.getShort();
                h.level[(int)Chart.Modes.HX]            = buffer.getShort();
                buffer.getBytes(2); // unknown field
                h.noteCount                             = new int[3];
                h.noteCount[(int)Chart.Modes.EX]        = buffer.getInt();
                h.noteCount[(int)Chart.Modes.NX]        = buffer.getInt();
                h.noteCount[(int)Chart.Modes.HX]        = buffer.getInt();
                h.eventCount                            = new int[3];
                h.eventCount[(int)Chart.Modes.EX]       = buffer.getInt();
                h.eventCount[(int)Chart.Modes.NX]       = buffer.getInt();
                h.eventCount[(int)Chart.Modes.HX]       = buffer.getInt();
                h.measureCount                          = new int[3];
                h.measureCount[(int)Chart.Modes.EX]     = buffer.getInt();
                h.measureCount[(int)Chart.Modes.NX]     = buffer.getInt();
                h.measureCount[(int)Chart.Modes.HX]     = buffer.getInt();
                h.packageCount                          = new int[3];
                h.packageCount[(int)Chart.Modes.EX]     = buffer.getInt();
                h.packageCount[(int)Chart.Modes.NX]     = buffer.getInt();
                h.packageCount[(int)Chart.Modes.HX]     = buffer.getInt();
                h.oldEncodeOJN                          = buffer.getShort();
                h.oldID                                 = buffer.getShort();
                h.oldGenre                              = buffer.getString(20);
                h.thumbnailSize                         = buffer.getInt();
                h.oldFileversion                        = buffer.getInt();
                h.title                                 = buffer.getString(64);
                h.artist                                = buffer.getString(32);
                h.pattern                               = buffer.getString(32);
                h.ojmFilename                           = buffer.getString(32);
                h.coverArtSize                          = buffer.getInt();
                h.duration                              = new int[3];
                h.duration[(int)Chart.Modes.EX]         = buffer.getInt();
                h.duration[(int)Chart.Modes.NX]         = buffer.getInt();
                h.duration[(int)Chart.Modes.HX]         = buffer.getInt();
                h.packageOffset                         = new int[3];
                h.packageOffset[(int)Chart.Modes.EX]    = buffer.getInt();
                h.packageOffset[(int)Chart.Modes.NX]    = buffer.getInt();
                h.packageOffset[(int)Chart.Modes.HX]    = buffer.getInt();
                h.coverArtOffset = buffer.getInt();

                // Misc
                h.packageEndOffset = new int[3];
                h.packageEndOffset[(int)Chart.Modes.EX] = h.packageOffset[(int)Chart.Modes.NX];
                h.packageEndOffset[(int)Chart.Modes.NX] = h.packageOffset[(int)Chart.Modes.HX];
                h.packageEndOffset[(int)Chart.Modes.HX] = h.coverArtOffset;


                // Covert Art
                if (h.coverArtSize > 0)
                {
                    buffer.Position = h.coverArtOffset;
                    h.coverArt = Image.FromStream(new StreamMemoryEngine(buffer.getBytes(h.coverArtSize)));
                }
                else
                    h.coverArt = null;

                // Thumbnail
                if (h.thumbnailSize > 0)
                {
                    buffer.Position = h.coverArtOffset + h.coverArtSize;
                    h.thumbail = (Bitmap)Bitmap.FromStream(new StreamMemoryEngine(buffer.getBytes(h.thumbnailSize)));
                }
                else
                    h.thumbail = null;

                buffer.Free();
                return true;
            }
            catch (Exception)
            {
                buffer.Free();
                return false;
            }
        }
        #endregion

        #region --- Parse Header To Chart ---
        // Parse header to Chart Class

        public static bool parseHeaderToChart(ref Chart chart, OJN h)
        {
            chart.title = h.title;
            chart.artist = h.artist;
            chart.pattern = h.pattern;
            chart.bpm = h.bpm;
            for (int i = 0; i < 3; i++)
                chart.level[(Chart.Modes)i] = h.level[i];

            chart.chartType = Chart.ChartFormat.OJN;
            chart.measureHeight = MEASURE_HEIGHT;

            return true;
        }
        #endregion

        #region --- Parse Event ---
        // Parse Event

        public static bool parseEvents(ref Chart chart, OJN h, string path, Chart.Modes mode)
        {
            StreamMemoryEngine buffer = new StreamMemoryEngine(path);
            bool result = parseEvents(ref chart, h, buffer.getAllBuffer(), mode);
            buffer.Free();

            return result;
        }

        public static bool parseEvents(ref Chart chart, OJN h, byte[] data, Chart.Modes mode)
        {
            // Initialize buffer
            StreamMemoryEngine buffer = new StreamMemoryEngine(data);

            try
            {
                // Get start and stop offset
                int start = h.packageOffset[(int)mode];
                int stop = h.packageEndOffset[(int)mode];

                // Get Package Count
                int packageCount = h.packageCount[(int)mode];

                // Calculate size Chart
                int sizechart = stop - start;

                // Check if the chart isnt empty
                if (sizechart == 0)
                    return false;
                
                // Add new List if the List isnt exist in current difficulty
                if (!chart.dataEvent.ContainsKey(mode))
                    chart.dataEvent.Add(mode, new XList<Event>());

                // Clear the Events in specified difficulty
                chart.dataEvent[mode].Clear();

                // Temporary Sample Event for SLN
                Event.SampleEvent[] holdSLN = new Event.SampleEvent[Enum.GetValues(typeof(Event.Channels)).Length - 2];

                // Empty it
                for (int i = 0; i < holdSLN.Length; i++)
                    holdSLN[i] = null;

                // Set buffer position to start offset
                buffer.Position = start;

                // Loop to all package as the package count said
                for (int package = 0; package < packageCount; package++)
                {
                    // Create temporary Event
                    Event tmp = new Event();
                    short numch, eventcount;

                    // Get Package Information
                    tmp.measure     = buffer.getInt();
                    numch           = buffer.getShort();

                    // Get Event Count in package
                    eventcount      = buffer.getShort();

                    // Convert Channel Number to Channel Enumeration
                    Event.Channels channel;
                    switch (numch)
                    {
                        case 0: channel = Event.Channels.Measurement; break;
                        case 1: channel = Event.Channels.BPM; break;
                        case 2: channel = Event.Channels.Note1; break;
                        case 3: channel = Event.Channels.Note2; break;
                        case 4: channel = Event.Channels.Note3; break;
                        case 5: channel = Event.Channels.Note4; break;
                        case 6: channel = Event.Channels.Note5; break;
                        case 7: channel = Event.Channels.Note6; break;
                        case 8: channel = Event.Channels.Note7; break;
                        default: channel = Event.Channels.SampleNote; break;
                    }

                    // Loop all Event
                    for (int i = 0; i < eventcount; i++)
                    {
                        // Assign Event Information
                        tmp.cell        = i;
                        tmp.beat        = eventcount;
                        tmp.channel     = channel;

                        // Calculate Note Position (for rendering)
                        tmp.calculatePosition(MEASURE_HEIGHT);

                        // Measurement / BPM Event
                        if (channel == Event.Channels.BPM || channel == Event.Channels.Measurement)
                        {
                            // Get Value of Event
                            float value = buffer.getFloat();

                            // Invalid Value, ignore it
                            if (value <= 0)
                                continue;

                            // Create Time Event from temporary Event
                            Event.TimeEvent te = new Event.TimeEvent(value, tmp);

                            // Time Event flag Always None
                            te.flag = Event.Flags.None;

                            // Add to the Chart
                            chart.dataEvent[mode].Add(te);
                        }
                        else
                        {
                            // Get Event Information
                            short value     = buffer.getShort();
                            int volume_pan  = buffer.get();
                            int type        = buffer.get();

                            // Empty Event, ignore it
                            if (value == 0)
                                continue;

                            // This ID, Volume and Pan Calculation is based OPEN2JAM
                            // ---- START OPEN2JAM CODE --- //
                            float vol = ((volume_pan >> 4) & 0x0F) / 16f;
                            if (vol == 0) vol = 1;

                            float pan = (volume_pan & 0x0F);
                            if (pan == 0) pan = 8;
                            pan -= 8;
                            pan /= 8f;

                            value--;

                            if (type % 8 > 3)
                                value += 1000;

                            type %= 4;
                            // --- END OPEN2JAM CODE --- //

                            // Create Sample Event
                            Event.SampleEvent se;

                            // if Chart have Sample Sound in OJM, pair event with sample sound
                            if (chart.dataSamples.ContainsKey(value))
                                se = new Event.SampleEvent(tmp, value, pan, vol, chart.dataSamples[value]);
                            else
                            // OJM not have it, Unkeysounded event
                                se = new Event.SampleEvent(tmp, value, pan, vol);

                            // This code modification from Open2Jam
                            switch (type)
                            {
                                // None flag
                                case 0:
                                    // Set the flag
                                    se.flag = Event.Flags.None;

                                    // Add to the chart
                                    chart.dataEvent[mode].Add(se);
                                    break;
                                // Unused (#W Normal displayed in NoteTool)
                                case 1:
                                    break;
                                // SLN (Start Long Note) flag
                                case 2:
                                    // Assign to the flag
                                    se.flag = Event.Flags.SLN;
                                    
                                    // Add to temporary SLN variable
                                    holdSLN[numch - 2] = se;
                                    break;
                                // ELN (End Long Note) flag
                                case 3:
                                    // Set the flag
                                    se.flag = Event.Flags.ELN;

                                    // Add it if there are pair SLN note
                                    if (holdSLN[numch - 2] != null)
                                    {
                                        // Add and pair the Long Note
                                        chart.dataEvent[mode].Add(new Event.SampleEvent(se) { hold = new Event.SampleEvent(holdSLN[numch - 2]) });
                                        chart.dataEvent[mode].Add(new Event.SampleEvent(holdSLN[numch - 2]) { hold = new Event.SampleEvent(se) });
                                    }

                                    // Empty the buffer
                                    holdSLN[numch = 2] = null;
                                    break;
                                // Invalid flag
                                default:
                                    break;
                            }
                        }

                    } // --- END LOOPING EVENT
                } // --- END LOOPING PACKAGE

                // Sort event acording the position Event
                chart.dataEvent[mode].Sort(delegate(Event e1, Event e2) { return e1.position.CompareTo(e2.position); });
                
                // Release stream
                buffer.Free();

                // Event parsed successfully
                return true;
            }
            catch (Exception)
            {
                // Parser having exception during parsing the event
                // Event parsed unsuccessfully
                return false;
            }
        }
        #endregion

        #region --- Parse ---
        // Parse the chart

        public static bool parse(ref Chart chart, ref OJN h, string path, Chart.Modes mode)
        {
            if (isOJN(path, ref h))
            {
                chart.title = h.title;
                chart.artist = h.artist;
                chart.pattern = h.pattern;
                chart.bpm = h.bpm;
                for (int i = 0; i < 3; i++)
                    chart.level[(Chart.Modes)i] = h.level[i];

                chart.chartType = Chart.ChartFormat.OJN;
                chart.measureHeight = MEASURE_HEIGHT;

                return parseEvents(ref chart, h, path, mode);
            }

            return false;
        }

        public static bool parse(ref Chart chart, ref OJN h, byte[] data, Chart.Modes mode)
        {
            if (isOJN(data, ref h))
            {
                chart.title = h.title;
                chart.artist = h.artist;
                chart.pattern = h.pattern;
                chart.bpm = h.bpm;
                for (int i = 0; i < 3; i++)
                    chart.level[(Chart.Modes)i] = h.level[i];

                chart.chartType = Chart.ChartFormat.OJN;
                chart.measureHeight = MEASURE_HEIGHT;

                return parseEvents(ref chart, h, data, mode);
            }

            return false;
        }
        #endregion
    }
}
