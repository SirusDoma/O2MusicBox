////////////////////////////////////////
// Stream Memory Engine               //
// Version              : 2.0         //
// Author               : CXO2        //
//////////////////////////////////////// 

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CXO2.IO
{
    // ~ Stream Engine Class
    public class StreamMemoryEngine : MemoryStream
    {
        // The Reader of Stream
        BinaryReader rdr;

        // The writer Of Stream
        BinaryWriter rw;

        // ~ Constructor #1
        public StreamMemoryEngine()
            : base()
        {
            rdr = new BinaryReader(this);
            rw = new BinaryWriter(this);
        }

        // ~ Constructor #2
        public StreamMemoryEngine(byte[] buffer)
            : base(buffer, true)
        {
            rdr = new BinaryReader(this);
            rw = new BinaryWriter(this);
        }

        // ~ Constructor #3
        public StreamMemoryEngine(string filename)
            : base(File.ReadAllBytes(filename), true)
        {
            rdr = new BinaryReader(this);
            rw = new BinaryWriter(this);
        }

        // Reset the Position
        public void basePos()
        {
            this.Position = 0;
        }

        // Check there are data after current ofthiset position
        public bool hasRemaining()
        {
            return (this.Position < this.Length);
        }

        // Get Byte data from current position
        public int get()
        {
            byte value = rdr.ReadByte();
            return value;
        }

        // Get Byte data from current position (Same like get())
        public byte getByte()
        {
            byte value = rdr.ReadByte();
            return value;
        }

        // Get ammount bytes data from current position
        public byte[] getBytes(int size)
        {
            byte[] value = rdr.ReadBytes(size);
            return value;
        }

        // Get ammount bytes data from current position (same like getBytes())
        public byte[] getBuffer(int size)
        {
            return rdr.ReadBytes(size);
        }

        // Get ammount bytes data from current position with return to last position option
        public byte[] getBuffer(int size, bool returnposition)
        {
            long lastpos;
            lastpos = this.Position;
            byte[] data = rdr.ReadBytes(size);

            if (returnposition == true)
            {
                this.Position = lastpos;
            }

            return data;
        }

        // Get integer 32 bit from current position
        public int getInt()
        {
            int value = rdr.ReadInt32();
            return value;
        }

        // Get integer 16 bit from current position
        public short getShort()
        {
            short value = rdr.ReadInt16();
            return value;
        }

        // Get integer 64 bit from current position
        public long getLong()
        {
            long value = rdr.ReadInt64();
            return value;
        }

        // Get string data from current position
        public string getString(int size)
        {
            byte[] data = rdr.ReadBytes(size);
            // Save to temp variable
            string tmp = Encoding.UTF8.GetString(data).Trim();
            string result = string.Empty;

            foreach (char n in tmp)
            {
                // Check for Null Terminating String
                if (n == '\0')
                    break;
                else
                    result += n;
            }

            return result;
        }

        // Get string data from current position with null terminating string option
        public string getString(int size, bool nullteriminatingstring)
        {
            byte[] data = rdr.ReadBytes(size);
            // Save to temp variable
            string tmp = Encoding.UTF8.GetString(data).Trim();
            string result = string.Empty;

            // Use terminating string
            if (nullteriminatingstring)
            {
                foreach (char n in tmp)
                {
                    // Check for Null Terminating String
                    if (n == '\0')
                        break;
                    else
                        result += n;
                }
            }
            else
                result = tmp;

            return result;
        }

        // Get single precision floating point value data from current position
        public float getFloat()
        {
            return rdr.ReadSingle();
        }

        // Get single precision floating point value data from current position (same like getFloat())
        public float getSingle()
        {
            return rdr.ReadSingle();
        }

        // Get decimal value from current position
        public decimal getDecimal()
        {
            return rdr.ReadDecimal();
        }

        // Get double precision floating point value data from current position
        public double getDouble()
        {
            return rdr.ReadDouble();
        }

        // Get all data from stream
        public byte[] getAllBuffer()
        {
            long curpos = this.Position;
            this.Position = 0;
            byte[] data = rdr.ReadBytes((int)this.Length);
            this.Position = curpos;
            return data;
        }

        // Get all remaining data from current position
        public byte[] getRemaining()
        {
            return rdr.ReadBytes((int)(this.Length - this.Position));
        }

        // Write byte data in current position
        public void write(byte data)
        {
            rw.Write(data);
        }

        // Write integer 32 bit in current position
        public void write(int data)
        {
            rw.Write(data);
        }

        // Write integer 16 bit in current position
        public void write(short data)
        {
            rw.Write(data);
        }

        // Write integer 64 bit in current position
        public void write(long data)
        {
            rw.Write(data);
        }

        // Write string data with Encoding 8-UTF in current position
        public void write(string data)
        {
            if (data == null)
                data = String.Empty;
            rw.Write(Encoding.UTF8.GetBytes(data));
        }

        // Write single precision floating point value in current position
        public void write(float data)
        {
            rw.Write(data);
        }

        // Write decimal value in current position
        public void write(decimal data)
        {
            rw.Write(data);
        }

        // Write double precision floating point value in current position
        public void write(double data)
        {
            rw.Write(data);
        }

        // Write ammount of bytes in current position
        public void write(byte[] data)
        {
            rw.Write(data);
        }

        public static StreamMemoryEngine getStreamMemoryEngine(Stream input)
        {
            StreamMemoryEngine SME = new StreamMemoryEngine();
            BinaryReader rdr = new BinaryReader(input);
            byte[] data = rdr.ReadBytes((int)input.Length);
            rdr.Close();
            input.Close();

            SME = new StreamMemoryEngine(data);
            return SME;
        }

        public new void Free()
        {
            rdr.Close();
            rw.Close();
            this.Dispose(true);
        }

    }
}
