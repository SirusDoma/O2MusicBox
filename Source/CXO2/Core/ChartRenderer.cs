using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

using Cgen;
using Cgen.Audio;

using CXO2;
using CXO2.Charting;
using CXO2.Processors;

namespace CXO2
{
    /// <summary>
    /// Provides automation process of <see cref="Chart"/> rendering.
    /// </summary>
    public class ChartRenderer : IDisposable
    {
        /*
        // Formula:
        // tick per minute = 600000000 ticks
        // 1 bpm = (1 beat / 1 minute) * (1 minute / 60 second) * (1 measure / 4 beat) * (192 cell / 1 measure) = 0.8 cell / second
        //
        //     (1 beat / 1 minute)    = Define beat per minute first (not quite neccessary tho, as the result is obviously 1)
        //     (1 minute / 60 second) = Define minutes per second, DO NOT treat them as time measurement, but as 2 integers (don't convert second to minute or vice versa)
        //     (1 measure / 4 beat)   = Define measure per beat, again do not treat them as note measurement
        //     (192 cell / 1 measure) = Define cell per measure, do not treat them as note measurement as well
        //
        // Summary:
        // cell per second = (1 / 1) * (1 / 60) * (1 / 4) * (192 / 1) = 0.8
        // tick signature = tick per minute - (tick per minute * cell per second)
        //
        // Result
        // 600000000 - (600000000 * 0.8) = 12500000
        //
        // Special Thanks to: Doaz (IBMSC Creator) and DJZMO (O2Live founder)
        */
        /// <summary>
        /// Gets the number of tick per cells.
        /// <para>Special Thanks to: Doaz (IBMSC Creator) and DJZMO (O2Live founder)</para>
        /// </summary>
        private const double TICK_SIGNATURE = 12500000D;

        private Task task;
        private double startTick = 0D, bpmOffset = 0D, timeOffset = 0D, pausedOffset = 0D;

        public event EventHandler RenderStart;
        public event EventHandler Rendering;
        public event EventHandler RenderComplete;

        /// <summary>
        /// Gets a value indicates whether the <see cref="ChartRenderer"/> is rendering.
        /// </summary>
        public bool IsRendering
        {
            get; private set;
        }

        /// <summary>
        /// Gets a value indicates whether the <see cref="ChartRenderer"/> is paused.
        /// <see cref="ChartRenderer.IsRendering"/> may still return true upon renderer is paused.
        /// </summary>
        public bool IsPaused
        {
            get; private set;
        }

        /// <summary>
        /// Gets a <see cref="Chart"/> that being rendered.
        /// </summary>
        public Chart Chart
        {
            get; private set;
        }

        /// <summary>
        /// Gets the current offset position of <see cref="ChartRenderer"/>.
        /// </summary>
        public double Offset
        {
            get
            {
                if (IsRendering && !IsPaused)
                {
                    return pausedOffset + (DateTime.Now.Ticks - startTick) / TICK_SIGNATURE * BPM + bpmOffset;
                }

                return 0f;
            }
        }

        /// <summary>
        /// Gets the current BPM of <see cref="ChartRenderer"/>.
        /// </summary>
        public double BPM
        {
            get; private set;
        }

        /// <summary>
        /// Gets the current Measure index of <see cref="ChartRenderer"/>.
        /// </summary>
        public int Measure
        {
            get; private set;
        }

        /// <summary>
        /// Gets the current Beat index of <see cref="ChartRenderer"/>.
        /// </summary>
        public int Beat
        {
            get; private set;
        }

        /// <summary>
        /// Gets the current Cell index of <see cref="ChartRenderer"/>.
        /// </summary>
        public int Cell
        {
            get; private set;
        }

        /// <summary>
        /// Gets the current time offset of <see cref="ChartRenderer"/>.
        /// </summary>
        public TimeSpan Elapsed
        {
            get; private set;
        }

        /// <summary>
        /// Gets the duration of the <see cref="Chart"/>.
        /// </summary>
        public TimeSpan Duration
        {
            get; private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartRenderer"/> class.
        /// </summary>
        public ChartRenderer()
        {
        }

        /// <summary>
        /// Initiate rendering process with the given <see cref="CXO2.Charting.Chart"/>.
        /// </summary>
        /// <param name="chart"><see cref="Charting.Chart"/> to render.</param>
        public async void Render(Chart chart)
        {
            // Wait task to complete and dispose current chart
            if (task?.Status == TaskStatus.Running)
                await task;

            // Validate chart
            Chart = chart;
            if (Chart == null || Chart.Events == null || Chart.Events.Length == 0)
                throw new Exception("No events to be rendered.");

            // Reset stats
            Measure     = 0;
            Beat        = 0;
            Cell        = 0;
            Elapsed     = TimeSpan.Zero;
            Duration    = Chart.Duration;
            BPM         = Chart.BPM;
            IsRendering = true;

            startTick    = DateTime.Now.Ticks;
            bpmOffset    = 0;
            timeOffset   = 0;
            pausedOffset = 0;

            RenderStart?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Initiate rendering process with the given <see cref="CXO2.Charting.Chart"/> asynchronously.
        /// </summary>
        /// <param name="chart"><see cref="Charting.Chart"/> to render.</param>
        /// <returns>Awaitable handle.</returns>
        public Task RenderAsync(Chart chart)
        {
            Render(chart);
            task = Task.Run(() => Stream());

            return task;
        }

        /// <summary>
        /// Pause the rendering process.
        /// </summary>
        public void Pause()
        {
            if (IsPaused)
                return;

            SoundSystem.Instance.Pause();
            pausedOffset = Offset;
            IsPaused = true;
        }

        /// <summary>
        /// Resume the paused rendering process.
        /// </summary>
        public void Resume()
        {
            if (!IsPaused)
                return;

            SoundSystem.Instance.Resume();
            startTick = DateTime.Now.Ticks;
            IsPaused = false;
        }

        /// <summary>
        /// Stop the rendering process.
        /// </summary>
        async public void Stop()
        {
            IsRendering = false;
            if (task != null)
                await task;
        }

        /// <summary>
        /// Update <see cref="ChartRenderer"/> instance to process pending rendering queue.
        /// </summary>
        /// <param name="delta">The number of elapsed milliseconds between current and last <see cref="Update(double)"/> call.</param>
        public void Update(double delta)
        {
            SoundSystem.Instance.Update();
            if (!IsRendering || IsPaused)
                return;

            // TODO: CONFIRM
            // In case rendering start off-sync when chart has so many bpm changes
            // Try to split the events between Time and Sound events and process them separately
            foreach (var ev in Chart.Events)
            {
                double offset = Offset;

                Measure = (int)(offset / 192f);
                Beat = (int)((offset % 192f) / (192f / 4f));
                Cell = (int)(offset % 192f);

                Elapsed = TimeSpan.FromSeconds(timeOffset + (((offset - bpmOffset) / (192f / 4f)) / BPM) * 60);
                Elapsed = Elapsed > Duration ? Duration : Elapsed;

                double latency = ev.Offset - offset;

                // Skip event
                if (ev.Judged || latency > 192 / 4)
                    continue;

                // Preload sample
                if (ev is Event.Sound sample && sample.Payload != null)
                {
                    Task.Run(() => sample.Preload());
                    continue;
                }

                if (ev is Event.Time time)
                {
                    if (latency <= 0)
                    {
                        if (time.Channel == Event.ChannelType.BPM)
                        {
                            // Update bpm offset footprint
                            startTick += (time.Offset - bpmOffset) / BPM * TICK_SIGNATURE;
                            timeOffset += (((offset - bpmOffset) / (192f / 4f)) / BPM) * 60;

                            // Update bpm information
                            BPM = time.Value;
                            bpmOffset = time.Offset;

                            // Apply current offset with current bpm
                            offset = Offset;
                            Elapsed = TimeSpan.FromSeconds(timeOffset + (((offset - bpmOffset) / (192f / 4f)) / BPM) * 60);

                            // Judge the event
                            ev.Judge();
                        }
                    }
                }
                else if (ev is Event.Sound sound)
                {
                    if (latency <= 0 && !ev.Judged)
                    {
                        if (sound.Signature != Event.SignatureType.Release)
                            sound.Play();

                        ev.Judge();
                    }
                }
            }

            // Invoke callback
            Rendering?.Invoke(this, EventArgs.Empty);

            // TODO: Do we need measure count to determine end of song to simulate o2jam gameplay?
            if (IsRendering && Elapsed >= Chart.Duration && Beat == 0 && SoundSystem.Instance.GetPlayingSources().Length == 0)
            {
                IsRendering = false;
                RenderComplete?.Invoke(this, EventArgs.Empty);
            }
        }

        async private void Stream()
        {
            do
            {
                // Simulate 60fps delta time (elapsed ms per frame)
                double delta = 1000f / 60f;

                // Prevent thread race by awaiting the update call
                await Task.Run(() => Update(delta));

                // Delay the current thread, necessary to reduce CPU utilization
                await Task.Delay(TimeSpan.FromMilliseconds(delta));
            }
            while (IsRendering);

            // Bingo, still same thread here
            SoundSystem.Instance.Stop();

            Dispose();
        }

        public void Dispose()
        {
            // Force to queue the playing sources
            SoundSystem.Instance.Update();
            var sources = SoundSystem.Instance.GetPlayingSources();

            foreach (var source in sources)
                source.Dispose();

            foreach (var ev in Chart.Events.Select((ev) => ev as Event.Sound))
                ev?.Dispose();
        }
    }
}