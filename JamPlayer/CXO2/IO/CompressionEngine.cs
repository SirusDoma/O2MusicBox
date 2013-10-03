////////////////////////////////////////
// Compression Engine Class 
// Version 1.0
// Copyright (C) [CXO2] ChronoCross 2013
////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace CXO2.IO
{
    // ~ Compression Engine
    public static class CompressionEngine
    {
        // Compression Type
        public enum Compression
        {
            DeflateStream = 0,
            GZipStream = 1
        }

        // Compress data
        public static byte[] Compress(byte[] data, Compression comptype)
        {
            byte[] retVal;

            if (comptype == Compression.GZipStream)
            {
                MemoryStream compressedMemoryStream = new MemoryStream();
                GZipStream compressStream = new GZipStream(compressedMemoryStream, CompressionMode.Compress, true);
                compressStream.Write(data, 0, data.Length);
                compressStream.Close();
                retVal = new byte[compressedMemoryStream.Length];
                compressedMemoryStream.Position = 0L;
                compressedMemoryStream.Read(retVal, 0, retVal.Length);
                compressedMemoryStream.Close();
                compressStream.Close();
            }
            else
            {
                MemoryStream compressedMemoryStream = new MemoryStream();
                DeflateStream compressStream = new DeflateStream(compressedMemoryStream, CompressionMode.Compress, true);
                compressStream.Write(data, 0, data.Length);
                compressStream.Close();
                retVal = new byte[compressedMemoryStream.Length];
                compressedMemoryStream.Position = 0L;
                compressedMemoryStream.Read(retVal, 0, retVal.Length);
                compressedMemoryStream.Close();
                compressStream.Close();
            }

            return retVal;
        }

        // Decompress
        public static byte[] Decompress(byte[] data, int uncompressedsize, Compression comptype)
        {
            byte[] ReadData = new byte[uncompressedsize];

            if (comptype == Compression.GZipStream)
            {
                GZipStream instream = new GZipStream(new MemoryStream(data), CompressionMode.Decompress);
                BinaryReader reader = new BinaryReader(instream);
                ReadData = reader.ReadBytes(uncompressedsize);
                instream.Flush();
                instream.Close();
            }
            else
            {
                DeflateStream instream = new DeflateStream(new MemoryStream(data), CompressionMode.Decompress);
                BinaryReader reader = new BinaryReader(instream);
                ReadData = reader.ReadBytes(uncompressedsize);
                instream.Flush();
                instream.Close();
            }

            return ReadData;
        }

    }
}
