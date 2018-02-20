using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace Genode.IO
{
    /// <summary>
    /// Represent a Stream that using memory as the backing store.
    /// </summary>
    public class BufferStream : MemoryStream
    {
        private BinaryReader _reader;
        private BinaryWriter _writer;

        /// <summary>
        /// Gets or Sets the current position within the stream.
        /// </summary>
        public new long Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                base.Position = value;
            }
        }

        /// <summary>
        /// Gets the length of the Stream in bytes.
        /// </summary>
        public new long Length
        {
            get
            {
                return base.Length;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current position of stream is less than the length of stream.
        /// </summary>
        public bool HasRemaining
        {
            get
            {
                int count = (int)(Length - Position);
                return count > 0;
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="BufferStream"/>.
        /// </summary>
        public BufferStream()
            : base()
        {
            _reader = new BinaryReader(this);
            _writer = new BinaryWriter(this);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="BufferStream"/> from existing <see cref="Stream"/> instance.
        /// </summary>
        /// <param name="stream">Existing <see cref="Stream"/> instance.</param>
        public BufferStream(Stream stream)
            : this()
        {
            stream.CopyTo(this);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="BufferStream"/> from an array of byte.
        /// </summary>
        /// <param name="data">Initial array of bytes for the stream.</param>
        public BufferStream(byte[] data)
            : base()
        {
            _reader = new BinaryReader(this);
            _writer = new BinaryWriter(this);

            Write(data);
        }

        /// <summary>
        /// Initializes a new instance of <see cref="BufferStream"/> from existing file.
        /// </summary>
        /// <param name="fileName">Path to the existing file.</param>
        public BufferStream(string fileName)
            : this(File.ReadAllBytes(fileName))
        {
        }

        /// <summary>
        /// Returns the next available character and does not advance the byte or character position.
        /// </summary>
        /// <returns></returns>
        public int Peek()
        {
            return _reader.PeekChar();
        }

        /// <summary>
        /// Reads a boolean value from the current stream and advances the current position by one byte.
        /// </summary>
        public bool ReadBool()
        {
            return _reader.ReadBoolean();
        }

        /// <summary>
        /// Writes a boolean value from the current stream and advances the current position by one byte.
        /// </summary>
        public void Write(bool value)
        {
            _writer.Write(value);
        }

        /// <summary>
        /// Reads the next character from the current stream
        /// and advance the current position of the stream in accordance with the Encoding used
        /// and the specific character being read from the stream.
        /// </summary>
        /// <returns></returns>
        public char ReadChar()
        {
            return _reader.ReadChar();
        }

        /// <summary>
        /// Writes a unicode characters to the current stream
        /// and advances the current position  of the stream in accordance with the Encoding used
        /// and the specific character being read from the stream.
        /// </summary>
        /// <param name="ch">The non-surrogate, Unicode charater to write.</param>
        public void Write(char ch)
        {
            _writer.Write(ch);
        }

        /// <summary>
        /// Reads count characters from the current stream,
        /// return data in character array,
        /// and advance the current position of the stream in accordance with the Encoding used
        /// and the specific character being read from the stream.
        /// </summary>
        /// <param name="count">The number of characters to read.</param>
        /// <returns></returns>
        public char[] ReadChars(int count)
        {
            return _reader.ReadChars(count);
        }

        /// <summary>
        /// Writes a section of a character array to the current stream,
        /// and advances the current position of the stream in accordance with the Encoding used
        /// and perhaps the specific characters being written to the stream.
        /// </summary>
        /// <param name="chars">A characters array containing the data to write.</param>
        public void Write(char[] chars)
        {
            _writer.Write(chars);
        }

        /// <summary>
        /// Writes a section of a character array to the current stream,
        /// and advances the current position of the stream in accordance with the Encoding used
        /// and perhaps the specific characters being written to the stream.
        /// </summary>
        /// <param name="chars">A characters array containing the data to write.</param>
        /// <param name="index">The starting point in buffer at which to begin writing.</param>
        /// <param name="count">The number of characters to write.</param>
        public void Write(char[] chars, int index, int count)
        {
            _writer.Write(chars, index, count);
        }

        /// <summary>
        /// Reads a string from the current stream. The string is prefixed with the length, encoded as an integer seven bits at a time.
        /// </summary>
        /// <returns></returns>
        public string ReadString()
        {
            return _reader.ReadString();
        }

        /// <summary>
        /// Reads count characters as a string with specified encoding and advances the current position by count characters.
        /// </summary>
        /// <param name="count">The number of characters to read.</param>
        /// <param name="encoding">The Encoding to decodes the array of bytes into a string, null to use default encoding.</param>
        /// <returns>Null terminated string.</returns>
        public string ReadString(int count, Encoding encoding = default(Encoding), bool nullTerminated = true)
        {
            if (encoding == default(Encoding))
                encoding = Encoding.Default;

            string result = encoding.GetString(ReadBytes(count)).TrimStart('\0');
            int index = result.IndexOf('\0');

            if (nullTerminated)
            {
                result = result.Trim('\0');
                if (index > 0)
                    result = result.Substring(0, index);
            }

            return result;
        }

        /// <summary>
        /// Writes a string with specified encoding and advances the current position by count characters.
        /// </summary>
        /// <param name="value">The string to written.</param>
        /// <param name="encoding">The Encoding to decodes the array of bytes into a string, null to use default encoding.</param>
        public void Write(string value, Encoding encoding = default(Encoding))
        {
            if (encoding == default(Encoding))
            {
                encoding = Encoding.Default;
            }

            byte[] strData = encoding.GetBytes(value);
            Write(strData);
        }

        /// <summary>
        /// Reads the next bytes from the current stream and advances the current position by one byte.
        /// </summary>
        /// <returns></returns>
        public new byte ReadByte()
        {
            return _reader.ReadByte();
        }

        /// <summary>
        /// Writes an unsigned byte to the current stream and advances the current position by one byte.
        /// </summary>
        /// <param name="value">The unsigned byte to write.</param>
        /// <returns></returns>
        public void Write(byte value)
        {
            _writer.Write(value);
        }

        /// <summary>
        /// Reads the signed bytes from the current stream and advances the current position by one byte.
        /// </summary>
        /// <returns></returns>
        public sbyte ReadSByte()
        {
            return _reader.ReadSByte();
        }

        /// <summary>
        /// Writes an signed byte to the current stream and advances the current position by one byte.
        /// </summary>
        /// <param name="value">The signed byte to write.</param>
        /// <returns></returns>
        public void Write(sbyte value)
        {
            _writer.Write(value);
        }

        /// <summary>
        /// Reads count bytes from the current stream into byte array and advances the current position by count bytes.
        /// </summary>
        /// <param name="count">The number of bytes to read.</param>
        /// <returns></returns>
        public byte[] ReadBytes(int count)
        {
            return _reader.ReadBytes(count);
        }

        /// <summary>
        /// Writes a byte array to the underlying stream.
        /// </summary>
        /// <param name="buffer">A byte array containing the data to write.</param>
        /// <returns></returns>
        public void Write(byte[] buffer)
        {
            _writer.Write(buffer);
        }

        /// <summary>
        /// Returns the next available bytes from the current position and does not advances the current position.
        /// </summary>
        /// <returns>Remaining array of bytes from the current position.</returns>
        public byte[] ReadRemaining()
        {
            long current = Position;

            int count = (int)(Length - Position);
            byte[] remaining = ReadBytes(count);
            Position = current;

            return remaining;
        }

        /// <summary>
        /// Reads a 2-bytes signed integer from the current stream and advances the current position by two bytes.
        /// </summary>
        /// <returns></returns>
        public short ReadShort()
        {
            return _reader.ReadInt16();
        }

        /// <summary>
        /// Writes a 2-bytes signed integer from the current stream and advances the current position by two bytes.
        /// </summary>
        /// <param name="value">The 2-bytes signed integer to write.</param>
        public void Write(short value)
        {
            _writer.Write(value);
        }

        /// <summary>
        /// Reads a 4-bytes signed integer from the current stream and advances the current position by four bytes.
        /// </summary>
        /// <returns></returns>
        public int ReadInteger()
        {
            return _reader.ReadInt32();
        }

        /// <summary>
        /// Writes a 4-bytes signed integer from the current stream and advances the current position by four bytes.
        /// </summary>
        /// <param name="value">The 4-bytes signed integer to write.</param>
        public void Write(int value)
        {
            _writer.Write(value);
        }

        /// <summary>
        /// Reads a 8-bytes signed integer from the current stream and advances the current position by eight bytes.
        /// </summary>
        /// <returns></returns>
        public long ReadLong()
        {
            return _reader.ReadInt64();
        }

        /// <summary>
        /// Writes a 8-bytes signed integer from the current stream and advances the current position by eight bytes.
        /// </summary>
        /// <param name="value">The 8-bytes signed integer to write.</param>
        public void Write(long value)
        {
            _writer.Write(value);
        }

        /// <summary>
        /// Reads a 2-bytes signed integer from the current stream and advances the current position by two bytes.
        /// </summary>
        /// <returns></returns>
        public short ReadInt16()
        {
            return _reader.ReadInt16();
        }

        /// <summary>
        /// Reads a 4-bytes signed integer from the current stream and advances the current position by four bytes.
        /// </summary>
        /// <returns></returns>
        public int ReadInt32()
        {
            return _reader.ReadInt32();
        }

        /// <summary>
        /// Reads a 8-bytes signed integer from the current stream and advances the current position by eight bytes.
        /// </summary>
        /// <returns></returns>
        public long ReadInt64()
        {
            return _reader.ReadInt64();
        }

        /// <summary>
        /// Reads a 2-bytes unsigned integer from the current stream and advances the current position by two bytes.
        /// </summary>
        /// <returns></returns>
        public ushort ReadUShort()
        {
            return _reader.ReadUInt16();
        }

        /// <summary>
        /// Writes a 2-bytes unsigned integer from the current stream and advances the current position by four bytes.
        /// </summary>
        /// <param name="value">The 2-bytes unsigned integer to write.</param>
        public void Write(ushort value)
        {
            _writer.Write(value);
        }

        /// <summary>
        /// Reads a 4-bytes unsigned integer from the current stream and advances the current position by two bytes.
        /// </summary>
        /// <returns></returns>
        public uint ReadUInteger()
        {
            return _reader.ReadUInt32();
        }

        /// <summary>
        /// Writes a 4-bytes unsigned integer from the current stream and advances the current position by four bytes.
        /// </summary>
        /// <param name="value">The 4-bytes unsigned integer to write.</param>
        public void Write(uint value)
        {
            _writer.Write(value);
        }

        /// <summary>
        /// Reads a 8-bytes unsigned integer from the current stream and advances the current position by eight bytes.
        /// </summary>
        /// <returns></returns>
        public ulong ReadULong()
        {
            return _reader.ReadUInt64();
        }

        /// <summary>
        /// Writes a 8-bytes unsigned integer from the current stream and advances the current position by eight bytes.
        /// </summary>
        /// <param name="value">The 8-bytes unsigned integer to write.</param>
        public void Write(ulong value)
        {
            _writer.Write(value);
        }

        /// <summary>
        /// Reads a 2-bytes unsigned integer from the current stream and advances the current position by two bytes.
        /// </summary>
        /// <returns></returns>
        public ushort ReadUInt16()
        {
            return _reader.ReadUInt16();
        }

        /// <summary>
        /// Reads a 4-bytes unsigned integer from the current stream and advances the current position by four bytes.
        /// </summary>
        /// <returns></returns>
        public uint ReadUInt32()
        {
            return _reader.ReadUInt32();
        }

        /// <summary>
        /// Reads a 8-bytes unsigned integer from the current stream and advances the current position by eight bytes.
        /// </summary>
        /// <returns></returns>
        public ulong ReadUInt64()
        {
            return _reader.ReadUInt64();
        }

        /// <summary>
        /// Reads a 4-bytes floating point value from the current stream and advances the current position by four bytes.
        /// </summary>
        /// <returns></returns>
        public float ReadFloat()
        {
            return _reader.ReadSingle();
        }

        /// <summary>
        /// Reads a 4-bytes floating point value from the current stream and advances the current position by four bytes.
        /// </summary>
        /// <returns></returns>
        public float ReadSingle()
        {
            return _reader.ReadSingle();
        }

        /// <summary>
        /// Writes a 4-bytes floating point value from the current stream and advances the current position by four bytes.
        /// </summary>
        /// <param name="value">The 4-bytes floating point value to write.</param>
        public void Write(float value)
        {
            _writer.Write(value);
        }

        /// <summary>
        /// Reads a 8-bytes floating point value from the current stream and advances the current position by eight bytes.
        /// </summary>
        /// <returns></returns>
        public double ReadDouble()
        {
            return _reader.ReadDouble();
        }

        /// <summary>
        /// Writes a 8-bytes floating point value from the current stream and advances the current position by eight bytes.
        /// </summary>
        /// <param name="value">The 8-bytes floating point value to write.</param>
        public void Write(double value)
        {
            _writer.Write(value);
        }

        /// <summary>
        /// Reads a decimal from the current stream and advances the current position by sixteen bytes.
        /// </summary>
        /// <returns></returns>
        public decimal ReadDecimal()
        {
            return _reader.ReadDecimal();
        }

        /// <summary>
        /// Writes a decimal from the current stream and advances the current position by sixteen bytes.
        /// </summary>
        /// <param name="value">The decimal value to write.</param>
        public void Write(decimal value)
        {
            _writer.Write(value);
        }

        /// <summary>
        /// Reads a block of bytes from the current stream and writes the data to buffer.
        /// </summary>
        /// <param name="buffer">When this method return, Has specified array with the values between offset and (offset + count - 1) replaced by characters read from the current stream.</param>
        /// <param name="offset">The byte offset in buffer at which to begin reading.</param>
        /// <param name="count">The maximum number of bytes to read.</param>
        /// <returns>The number of bytes successfully read.</returns>
        public new int Read(byte[] buffer, int offset, int count)
        {
            return base.Read(buffer, offset, count);
        }

        /// <summary>
        /// Writes a block of bytes to the current stream using data read from buffer.
        /// </summary>
        /// <param name="buffer">The buffer to write data from.</param>
        /// <param name="offset">The byte offset in buffer at which to begin writing from.</param>
        /// <param name="count">The maximum number of bytes to write.</param>
        public new void Write(byte[] buffer, int offset, int count)
        {
            base.Write(buffer, offset, count);
        }

        /// <summary>
        /// Sets the position within the current stream to the specified value.
        /// </summary>
        /// <param name="offset">The new position within the stream, this is relative to the loc parameter and can be positve or negative.</param>
        /// <param name="loc">The seek reference point.</param>
        /// <returns>The new position within the stream, calculated by combining the initial reference point and the offset.</returns>
        public new long Seek(long offset, SeekOrigin loc = SeekOrigin.Current)
        {
            return base.Seek(offset, loc);
        }

        /// <summary>
        /// Writes the stream contents to array of bytes, regardless of the <see cref="BufferStream.Position"/> property.
        /// </summary>
        /// <returns></returns>
        public new byte[] ToArray()
        {
            return base.ToArray();
        }

        /// <summary>
        /// Release all resources used by <see cref="BufferStream"/>.
        /// </summary>
        public new void Dispose()
        {
            _reader.Close();
            _writer.Close();

            base.Dispose();
        }

        /// <summary>
        /// Release all resources used by <see cref="BufferStream"/>.
        /// </summary>
        public new void Dispose(bool disposing)
        {
            _reader.Close();
            _writer.Close();

            base.Dispose(disposing);
        }
    }
}