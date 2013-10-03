
// This code is modification rendering code from Doaz
// It only working that XList<>

// Thank you Doaz! :3

using System;
using System.Collections.Generic;
using System.Text;
using CXO2;
using CXO2.Core;
using CXO2.Core.SoundSystem;
using CXO2.Data;

namespace CXO2.Core.Renderer
{
    public static class ChartRenderer
    {
        #region --- Variables ---
        static Chart _chart;
        static Chart.Modes _mode;

        // Rendering Purpose
        static double _bpm = 0;
        static double _startTick = 0;
        static double _startPosition = 0;
        static double _currentPosition = 0;

        static bool _isRunning = false;
        
        #endregion

        #region --- Initialization ---
        public static void init(Chart chart, float hispeed = 4.0f, Chart.Modes mode = Chart.Modes.HX)
        {
            // Initialization Chart Renderer
            _chart              = chart;
            _mode               = mode;
            _bpm                = _chart.bpm;

            _startTick          = 0;
            _startPosition      = 0;
            _currentPosition    = 0;

        }
        #endregion

        #region --- Update ---
        // Get running status
        public static bool getRunningState()
        {
            return _isRunning;
        }

        // Update start tick
        public static void updateStartTick()
        {
           _startTick = DateTime.Now.Ticks;
        }

        // Update Chart Renderer
        public static void update()
        {
            // Check Running Status
            if (_chart.dataEvent[_mode].HasElementLeft)
                _isRunning = true;
            else
                _isRunning = false;

            // Get Current Position
            long currentTick = DateTime.Now.Ticks;
            _currentPosition = (currentTick - _startTick) / 12500000D * _bpm + _startPosition;

            // Looping to Whole Event
            while (_chart.dataEvent[_mode].HasElementLeft && _chart.dataEvent[_mode].ElementAtPointer.position <= _currentPosition)
            {
                // Get Event
                Event e = _chart.dataEvent[_mode].ElementAtPointer;

                // BPM Event
                if (e.channel == Event.Channels.BPM)
                {
                    // Catch new BPM Information
                    Event.TimeEvent newBPM  = (Event.TimeEvent)e;
                    _startTick             += (newBPM.position - _startPosition) / _bpm * 12500000D;
                    _startPosition          = newBPM.position;
                    _bpm                    = newBPM.Value;

                    // Recalculate current position
                    _currentPosition        = (currentTick - _startTick) / 12500000D * _bpm + _startPosition;
                    e.judged = true;
                }
                // Sample Event
                else
                {
                    // Get Event Sample
                    Event.SampleEvent sample = (Event.SampleEvent)e;

                    // Check if the sample already judged and the sample is ELN and the sample sound is null before play the sound
                    if (!sample.judged && sample.flag != Event.Flags.ELN && sample.sound != null)
                        SoundEngine.playSound(sample.sound);

                    // Set judged status
                    e.judged = true;
                }

                // Jump to next event
                _chart.dataEvent[_mode].IncreasePointer();
            }
        }
        #endregion
    }
}
