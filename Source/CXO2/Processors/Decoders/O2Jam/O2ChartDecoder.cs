using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using CXO2;
using CXO2.Charting;
using CXO2.Charting.O2Jam;

using Cgen;
using Cgen.Audio;

namespace CXO2.Processors.O2Jam
{
    public class O2ChartDecoder : ChartDecoder
    {
        public override bool Check(string filename)
        {
            return OJNDecoder.Check(filename);
        }

        public O2ChartDecoder(string filename)
            : base(filename)
        {
        }

        public override Chart Decode(int level)
        {
            var diff = (OJN.Difficulty)level;
            var ojn = OJNDecoder.Decode(Filename);
            var chart = new Chart(Filename)
            {
                Title = ojn.Title,
                Artist = ojn.Artist,
                Pattern = ojn.Pattern,
                BPM = ojn.BPM,
                Duration = TimeSpan.FromSeconds(ojn.Duration[diff]),

                ThumbnailData = ojn.Thumbnail,
                CoverArtData = ojn.CoverArt
            };

            OnChartHeaderDecoded(Filename, chart);

            OJM ojm = OJMDecoder.Decode(Filename.Replace(Path.GetFileName(Filename), ojn.OJMFileName));
            var events = ojn.GetEvents(diff);

            // Load OJM Samples and assign to events
            if (ojm != null)
            {
                ojn.OJM = ojm;
                foreach (var sample in ojm.Samples)
                {
                    var sampleEvents = Array.FindAll(events, (e) => {
                        var se = e as Event.Sound;
                        return se != null && se.Id == sample.Id && se.Signature != Event.SignatureType.Release;
                    });

                    if (sampleEvents != null)
                    {
                        foreach (var ev in sampleEvents)
                        {
                            var sound = ev as Event.Sound;
                            sound.Name = sample.Name;
                            sound.Payload = sample.Payload;

                            if (sound.Offset <= (192 / 4))
                                sound.Preload();
                        }

                        //bool bgm = sample.Id >= 1000;
                        //if (bgm)
                        //{
                        //    //var music = new Music(sample.Payload);
                        //    //var stream = new MemoryStream(sample.Payload);
                        //    foreach (var ev in sampleEvents)
                        //    {
                        //        var sound     = ev as Event.Sound;
                        //        sound.Name    = sample.Name;
                        //        sound.Payload = sample.Payload;
                        //        //sound.Sample  = new Music(stream);
                        //    }
                        //}
                        //else
                        //{
                        //    //var buffer = new SoundBuffer(sample.Payload);
                        //    foreach (var ev in sampleEvents)
                        //    {
                        //        var sound     = ev as Event.Sound;
                        //        sound.Name    = sample.Name;
                        //        sound.Payload = sample.Payload;
                        //        //sound.Sample  = new Sound(buffer);
                        //    }
                        //}
                    }
                }
            }

            chart.Events = events;
            return chart;
        }

        public override Chart DecodeHeader(int level)
        {
            var diff = (OJN.Difficulty)level;
            var ojn = OJNDecoder.DecodeHeader(Filename);
            var chart = new Chart(Filename)
            {
                Title = ojn.Title,
                Artist = ojn.Artist,
                Pattern = ojn.Pattern,
                BPM = ojn.BPM,
                Duration = TimeSpan.FromSeconds(ojn.Duration[diff]),

                ThumbnailData = ojn.Thumbnail,
                CoverArtData = ojn.CoverArt
            };

            OnChartHeaderDecoded(Filename, chart);
            return chart;
        }
    }
}
