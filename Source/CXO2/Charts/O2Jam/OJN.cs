using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CXO2.Charting.O2Jam
{
    /// <summary>
    /// Represents O2Jam Note <see cref="Chart"/> Container.
    /// </summary>
    public class OJN
    {
        /// <summary>
        /// Gets the Signature of OJN file.
        /// </summary>
        public const string HEADER_SIGNATURE = "ojn\0";

        /// <summary>
        /// Gets the NEW format Signature.
        /// </summary>
        public const string NEW_HEADER_SIGNATURE = "new";

        /// <summary>
        /// Represents O2Jam <see cref="Chart"/> genres.
        /// </summary>
        public enum Genres
        {
            /// <summary>
            /// Ballad.
            /// </summary>
            Ballad      = 0,
            /// <summary>
            /// Rock.
            /// </summary>
            Rock        = 1,
            /// <summary>
            /// Dance.
            /// </summary>
            Dance       = 2,
            /// <summary>
            /// Techno.
            /// </summary>
            Techno      = 3,
            /// <summary>
            /// Hip-Hop.
            /// </summary>
            HipHop      = 4,
            /// <summary>
            /// Soul.
            /// </summary>
            Soul        = 5,
            /// <summary>
            /// Jazz.
            /// </summary>
            Jazz        = 6,
            /// <summary>
            /// Funk.
            /// </summary>
            Funk        = 7,
            /// <summary>
            /// Classical.
            /// </summary>
            Classical   = 8,
            /// <summary>
            /// Traditional.
            /// </summary>
            Traditional = 9,
            /// <summary>
            /// Not specified.
            /// </summary>
            Etc         = 10
        }

        /// <summary>
        /// Represents O2Jam <see cref="Chart"/> difficulty.
        /// </summary>
        public enum Difficulty
        {
            /// <summary>
            /// Easy,
            /// </summary>
            EX = 0,
            /// <summary>
            /// Normal,
            /// </summary>
            NX = 1,
            /// <summary>
            /// Hard,
            /// </summary>
            HX = 2,
            /// <summary>
            /// Master - May (or may not) exists in newer version of <see cref="OJN"/>.
            /// If this does exists, only E.T would survive.
            /// </summary>
            MX = 3
        }

        public int Id
        {
            get; set;
        }
        
        public string Sign
        {
            get; set;
        }

        public float EncodingVersion
        {
            get; set;
        }

        public Genres Genre
        {
            get; set;
        }

        public float BPM
        {
            get; set;
        }

        public Dictionary<Difficulty, short> Level
        {
            get; set;
        }

        public Dictionary<Difficulty, int> EventCount
        {
            get; set;
        }

        public Dictionary<Difficulty, int> NoteCount
        {
            get; set;
        }

        public Dictionary<Difficulty, int> MeasureCount
        {
            get; set;
        }

        public Dictionary<Difficulty, int> BlockCount
        {
            get; set;
        }

        public short OldEncodingVersion
        {
            get; set;
        }

        public short OldId
        {
            get; set;
        }

        // 20 bytes
        public string OldGenre
        {
            get; set;
        }                 

        public int ThumbnailSize
        {
            get { return (int)Thumbnail?.Length; }
        }

        public int Version
        {
            get; set;
        }

        // 64 bytes size
        public string Title
        {
            get; set;
        }                    

        // 32 bytes size
        public string Artist
        {
            get; set;
        }                   

        // 32 bytes size
        public string Pattern
        {
            get; set;
        }                  

        // 32 bytes size
        public string OJMFileName
        {
            get; set;
        }             

        public int CoverArtSize
        {
            get { return (int)CoverArt?.Length; }
        }

        // In second format
        public Dictionary<Difficulty, int> Duration
        {
            get; set;
        }                  

        public Dictionary<Difficulty, int> BlockOffset
        {
            get; set;
        }

        public int CoverArtOffset
        {
            get; internal set;
        }

        public byte[] CoverArt
        {
            get; set;
        }

        public byte[] Thumbnail
        {
            get; set;
        }

        internal Dictionary<Difficulty, Event[]> Events
        {
            get; set;
        }

        public OJM OJM
        {
            get; set;
        }

        /// <summary>
        /// Initializes a new Instance of <see cref="OJN"/>.
        /// </summary>
        public OJN()
        {
        }

        /// <summary>
        /// Gets an array of <see cref="Event"/> of specified <see cref="Difficulty"/>.
        /// </summary>
        /// <param name="difficulty">Specifies the <see cref="Difficulty"/> of <see cref="Event"/> to retrieve.</param>
        /// <returns>An array of <see cref="Event"/> of specified <see cref="Difficulty"/>.</returns>
        public Event[] GetEvents(OJN.Difficulty difficulty)
        {
            return Events[difficulty];
        }

        /// <summary>
        /// Set the <see cref="Event"/> sets with specified array of <see cref="Event"/> for specified <see cref="Difficulty"/>.
        /// <para>
        ///     This method will recalculate <see cref="EventCount"/>, <see cref="NoteCount"/>, <see cref="MeasureCount"/>, 
        ///     <see cref="BlockCount"/>, <see cref="BlockOffset"/> and <see cref="CoverArtOffset"/>.
        /// </para>
        /// </summary>
        /// <param name="difficulty">Specifies the <see cref="Difficulty"/> to set.</param>
        /// <param name="events">New array of <see cref="Event"/> to subtitue the current event sets.</param>
        public void SetEvents(OJN.Difficulty difficulty, Event[] events)
        {
            Events[difficulty]     = events;
            EventCount[difficulty] = events.Length;
            NoteCount[difficulty]  = events.Where((ev) =>
                ev.Channel != Event.ChannelType.Measurement &&
                ev.Channel != Event.ChannelType.BPM &&
                ev.Channel != Event.ChannelType.BGM
            ).Count();

            MeasureCount[difficulty] = events.Max((ev) => ev.Measure) + 1;

            // Reset offset
            foreach (var diff in Enum.GetValues(typeof(OJN.Difficulty)) as OJN.Difficulty[])
            {
                // Skip MX as we dont really have it in current implementation
                if (diff == Difficulty.MX)
                    continue;

                // Get initial offset, typically offset or previous difficulty offset;
                // We'll calculate the rest offset later
                int offset = 300;
                if (diff == Difficulty.EX)
                    BlockOffset[diff] = offset;
                else
                    BlockOffset[diff] = BlockOffset[diff - 1];

                // Get the number of blocks
                BlockCount[diff] = Events[diff].GroupBy((ev) => new { ev.Measure, ev.Tempo, ev.LaneIndex }).Count();

                // No need to calculate the previous block size if it's EX difficulty
                // Because it is first difficulty and no previous difficulty
                if (diff == Difficulty.EX)
                    continue;

                // Calculate total block size of previous difficulty to get block offset of current difficulty
                var blocks = Events[diff - 1].GroupBy((ev) => new { ev.Measure, ev.Tempo, ev.LaneIndex });
                foreach (var block in blocks)
                {
                    // Calculate the event size, one event worth 4 bytes
                    // Regardless it is empty event or not
                    // Block header also contains 8 bytes
                    BlockOffset[diff] += 8 + (block.Count() * 4);
                }

                // Calculate the HX block size as well to get CoverArtOffset
                if (diff == Difficulty.HX)
                {
                    blocks = Events[diff].GroupBy((ev) => new { ev.Measure, ev.Tempo, ev.LaneIndex });
                    CoverArtOffset = BlockOffset[diff];

                    foreach (var block in blocks)
                    {
                        // Again, Calculate the block size from the sets of events.
                        CoverArtOffset += 8 + (block.Count() * 4);
                    }
                }
            }
        }
    }
}
