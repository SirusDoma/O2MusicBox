////////////////////////////////////////
// OJN Struct                         //
// Version              : 1.0         //
// Author               : CXO2        //
//////////////////////////////////////// 

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;

namespace CXO2.Data
{
    // ~ OJN Structure
    public struct OJN
    {
        // ~ Enumeration Genres
        public enum Genres
        {
            Ballad = 0,
            Rock = 1,
            Dance = 2,
            Techno = 3,
            HipHop = 4,
            Soul = 5,
            Jazz = 6,
            Funk = 7,
            Classical = 8,
            Traditional = 9,
            Etc = 10
        }

        // ~ OJN Header
        public int id;
        public int sign;
        public float encodeOJN;
        public Genres genre;
        public float bpm;
        public short[] level;
        public int[] eventCount;
        public int[] noteCount;
        public int[] measureCount;
        public int[] packageCount;
        public short oldEncodeOJN;
        public short oldID;
        public string oldGenre;                 // 20 bytes size
        public int thumbnailSize;
        public int oldFileversion;
        public string title;                    // 64 bytes size
        public string artist;                   // 32 bytes size
        public string pattern;                  // 32 bytes size
        public string ojmFilename;              // 32 bytes size
        public int coverArtSize;
        public int[] duration;                  // Second format
        public int[] packageOffset;
        public int[] packageEndOffset;
        public int coverArtOffset;

        // ~ OJN Resources Files
        public Image coverArt;
        public Bitmap thumbail;
        public string ojm;
        public string file;

    }
}
