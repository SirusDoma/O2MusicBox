using System;
using System.Collections.Generic;
using System.Text;
using CXO2;
using CXO2.Core;
using CXO2.Core.SoundSystem;
using CXO2.Data;
using CXO2.IO;

namespace CXO2.Parser
{
    public static class OJMParser
    {
        #region --- Type / Enum ---
        public enum OJM_Format
        {
            Invalid = -1,
            M30 = 0,
            OMC = 1,
            OJM = 1
        }
        #endregion

        #region --- isOJM ---
        public static bool isOJM(byte[] data)
        {
            try
            {
                StreamMemoryEngine buffer = new StreamMemoryEngine(data);
                string fmt = buffer.getString(3);
                buffer.Free();

                foreach (OJM_Format n in Enum.GetValues(typeof(OJM_Format)))
                {
                    if (Enum.GetName(typeof(OJM_Format), n).ToLower() == fmt.ToLower())
                        return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static OJM_Format getFormat(byte[] data)
        {
            StreamMemoryEngine buffer = new StreamMemoryEngine(data);
            string fmt = buffer.getString(3);
            buffer.Free();

            foreach (OJM_Format n in Enum.GetValues(typeof(OJM_Format)))
            {
                if (Enum.GetName(typeof(OJM_Format), n).ToLower() == fmt.ToLower())
                    return n;
            }

            return OJM_Format.Invalid;
        }
        #endregion

        #region --- Parse ---
        public static bool parse(ref Chart chart, string path)
        {
            StreamMemoryEngine buffer = new StreamMemoryEngine(path);
            byte[] data = buffer.getAllBuffer();
            buffer.Free();

            return parse(ref chart, data);
        }

        public static bool parse(ref Chart chart, byte[] data)
        {
            OJM_Format format = getFormat(data);
            if (format == OJM_Format.Invalid)
                throw new Exception("Invalid OJM File");

            StreamMemoryEngine buffer = new StreamMemoryEngine(data);
            bool result = false;

            if (format == OJM_Format.M30)
                result = parseM30(ref chart, ref buffer);
            else if (format == OJM_Format.OMC | format == OJM_Format.OJM)
                result = parseOMC(ref chart, ref buffer);

            buffer.Free();
            return result;
        }
        #endregion

        #region --- Parse OMC ---
        public static bool parseOMC(ref Chart chart, ref StreamMemoryEngine buffer)
        {
            try
            {
                // Clear data samples in chart
                chart.dataSamples = new Dictionary<int, SoundSample>();

                // OMC / OJM string + null byte
                buffer.getBuffer(4);

                // Get OJM Header
                short totalwav  = buffer.getShort();
                short totalogg  = buffer.getShort();
                int wavOffset   = buffer.getInt();
                int oggOffset   = buffer.getInt();
                int filesize    = buffer.getInt();

                // Used for SampleName and SampleData
                string sampleName;
                byte[] sampledata;
                
                // Initialize buffer position
                buffer.Position = wavOffset;

                // Loop for #W sample
                for (int i = 0; i < totalwav; i++)
                {
                    // Get Sample Header
                    sampleName          = buffer.getString(32);
                    short audioFormat   = buffer.getShort();
                    short numChannels   = buffer.getShort();
                    int samplerate      = buffer.getInt();
                    int bitrate         = buffer.getInt();
                    short blockAlign    = buffer.getShort();
                    short bitpersample  = buffer.getShort();
                    string strdata      = buffer.getString(4);
                    int sampleSize      = buffer.getInt();

                    // Ignore empty sample
                    if (sampleSize == 0)
                        continue;

                    // Decode Wave Sample (This code based OPEN2JAM OJM Parser)
                    sampledata = decodeWave(buffer.getBuffer(sampleSize));

                    // Default pcm wave
                    int pcm = 16;

                    // Wave size
                    int wavesize = sampleSize + 16;

                    // Create Wave (Create Wave Header + Existing SampleData)
                    StreamMemoryEngine waveStream = new StreamMemoryEngine();
                    waveStream.write("RIFF");
                    waveStream.write((int)wavesize);
                    waveStream.write("WAVE");
                    waveStream.write("fmt ");
                    waveStream.write((int)pcm);
                    waveStream.write((short)audioFormat);
                    waveStream.write((short)numChannels);
                    waveStream.write((int)samplerate);
                    waveStream.write((int)bitrate);
                    waveStream.write((short)blockAlign);
                    waveStream.write((short)bitpersample);
                    waveStream.write(strdata);
                    waveStream.write((int)sampleSize);
                    waveStream.write(sampledata);

                    // Get Wave data
                    sampledata = waveStream.getAllBuffer();
                    
                    // Close the stream
                    waveStream.Free();

                    // Add sample to chart
                    // NOTE: this is using Sample mode which can save memory usage
                    // by deleting the sample after create the sound

                    // Refer to FMOD documentation for more details
                    chart.dataSamples.Add(i, new SoundSample(sampledata, i, sampleName, false, true));
                }

                // Make rest-ansured the buffer position in OGG Offset
                buffer.Position = oggOffset;

                // Loop for #M Sample
                for (int i = 0; i < totalogg; i++)
                {
                    // Get Sample Header
                    sampleName      = buffer.getString(32);
                    int sampleSize  = buffer.getInt();

                    // Ignore empty sample
                    if (sampleSize == 0)
                        continue;

                    // Get Sample data
                    sampledata      = buffer.getBuffer(sampleSize);

                    // Add sample to chart
                    // NOTE: this is using Sample mode which can save memory usage
                    // by deleting the sample after create the sound

                    // Read FMOD documentation for more details
                    chart.dataSamples.Add(i + 1000, new SoundSample(sampledata, i + 1000, sampleName, false, true));
                }

                // OJM Successfully parsed
                return true;
            }
            catch (Exception)
            {
                // Exception was thrown when parser trying to parse OJM
                // OJM Failed to parse
                return false;
            }
        }
        #endregion

        #region --- Parse Sound OMC ---
        public static bool parseSoundOMC(ref SoundSample sound, ref StreamMemoryEngine buffer, int refID, bool loop = true, bool sample = false)
        {
            try
            {
                // OMC / OJM string + null byte
                buffer.getBuffer(4);

                // Get OJM Header
                short totalwav      = buffer.getShort();
                short totalogg      = buffer.getShort();
                int wavOffset       = buffer.getInt();
                int oggOffset       = buffer.getInt();
                int filesize        = buffer.getInt();

                // Used for SampleName and SampleData
                string sampleName;
                byte[] sampledata;

                // Is the sample we looking for is #W sample?
                if (refID < 1000)
                {
                    // Initialize buffer position
                    buffer.Position = wavOffset;

                    // Look for #W Sample
                    for (int i = 0; i < totalwav; i++)
                    {
                        // Get Sample Header
                        sampleName          = buffer.getString(32);
                        short audioFormat   = buffer.getShort();
                        short numChannels   = buffer.getShort();
                        int samplerate      = buffer.getInt();
                        int bitrate         = buffer.getInt();
                        short blockAlign    = buffer.getShort();
                        short bitpersample  = buffer.getShort();
                        string strdata      = buffer.getString(4);
                        int sampleSize      = buffer.getInt();

                        // Is this sample that we looking for?
                        if (refID != i)
                            // No, this is not
                            continue;

                        // Ignore empty sample
                        if (sampleSize == 0)
                        {
                            // and this is sample we are looking for
                            // failed to parse sound

                            return false;
                        }

                        // Decode Wave Sample (This code based OPEN2JAM OJM Parser)
                        sampledata = decodeWave(buffer.getBuffer(sampleSize));

                        // Default pcm wave
                        int pcm = 16;

                        // Wave size
                        int wavesize = sampleSize + 16;

                        // Create Wave (Create Wave Header + Existing SampleData)
                        StreamMemoryEngine waveStream = new StreamMemoryEngine();
                        waveStream.write("RIFF");
                        waveStream.write((int)wavesize);
                        waveStream.write("WAVE");
                        waveStream.write("fmt ");
                        waveStream.write((int)pcm);
                        waveStream.write((short)audioFormat);
                        waveStream.write((short)numChannels);
                        waveStream.write((int)samplerate);
                        waveStream.write((int)bitrate);
                        waveStream.write((short)blockAlign);
                        waveStream.write((short)bitpersample);
                        waveStream.write(strdata);
                        waveStream.write((int)sampleSize);
                        waveStream.write(sampledata);

                        // Get Wave data
                        sampledata = waveStream.getAllBuffer();

                        // Close the stream
                        waveStream.Free();

                        // Get the sound
                        sound = new SoundSample(sampledata, i, sampleName, loop, sample);

                        // Successfully parse the sound
                        return true;
                    }
                }
                // We are looking for #M Sample
                else
                {
                    // Make rest-ansured the buffer position in OGG Offset
                    buffer.Position = oggOffset;

                    // Loop for #M Sample
                    for (int i = 0; i < totalogg; i++)
                    {
                        // Get Sample Header
                        sampleName = buffer.getString(32);
                        int sampleSize = buffer.getInt();

                        // Is this sample that we are looking for?
                        if (refID != (i + 1000))
                            continue;

                        // Ignore empty sample
                        if (sampleSize == 0)
                        {
                            // and this is sample we are looking for
                            // failed to parse sound

                            return false;
                        }

                        // Get Sample data
                        sampledata = buffer.getBuffer(sampleSize);

                        // Get the sound
                        sound = new SoundSample(sampledata, i + 1000, sampleName, loop, sample);

                        // Successfully parsed the sound
                        return true;
                    }
                }

                // Couldnt find the sample that we are looking for
                return false;
            }
            catch (Exception)
            {
                // Exception was thrown when parser trying to parse OJM
                // OJM Failed to parse
                return false;
            }
        }
        #endregion

        #region --- Parse M30 ---
        public static bool parseM30(ref Chart chart, ref StreamMemoryEngine buffer)
        {
            try
            {
                // M30 String + Null byte
                buffer.getString(4);

                // Get OJM Header Information
                int ojmver              = buffer.getInt();
                int encsign             = buffer.getInt();
                int samplecount         = buffer.getInt();
                int sampleoffset        = buffer.getInt();
                int samplestotalsize    = buffer.getInt();
                int padding             = buffer.getInt();

                // Initialize buffer position
                buffer.Position = sampleoffset;

                // Loop whole sample
                for (int i = 0; i < samplecount; i++)
                {
                    // Get Sample Header
                    string name         = buffer.getString(32, true);
                    int samplesize      = buffer.getInt();
                    short sampletype    = buffer.getShort();
                    short unkfixeddata  = buffer.getShort();
                    int unkmusicflag    = buffer.getInt();
                    short sampleidx     = buffer.getShort();
                    short unk_zero      = buffer.getShort();
                    int pcm_samples     = buffer.getInt();

                    // Empty data, ignore it
                    if (samplesize == 0)
                        continue;

                    // M### Sample
                    if (sampletype == 0)
                        sampleidx += 1000;

                    // Get Sample Data
                    byte[] sampledata = buffer.getBytes(samplesize);

                    // Decrypt data as Encryption that used in sample
                    // #TODO: I dont know any OJM encryption except nami
                    switch (encsign)
                    {
                        // Scramble1 Encryption
                        case 1:
                            break;
                        // Scramble2 Encryption
                        case 2:
                            break;
                        // Decode Encryption
                        case 4:
                            break;
                        // Decrypt Encryption
                        case 8:
                            break;
                        // Nami Encryption
                        case 16:
                            // Nami
                            byte[] nami = { 0x6E, 0x61, 0x6D, 0x69 };

                            // Loop whole data
                            for (int d = 0; d + 3 < samplesize; d += 4)
                            {
                                for (int j = 0; j < 4; j++)
                                    // Xor it
                                    sampledata[d + j] ^= nami[j];
                            }
                            break;
                        // Unknown Encryption
                        default:
                            break;
                    }

                    // Check if its valid data OGG
                    if (Encoding.UTF8.GetString(sampledata, 0, 4) != "OggS")
                    {
                        // Invalid data, delete data and go to next loop
                        sampledata = null;
                        continue;
                    }

                    // Add sample to chart
                    // NOTE: this is using Sample mode which can save memory usage
                    // by deleting the sample after create the sound

                    // Refer to FMOD documentation for more details
                    chart.dataSamples.Add(sampleidx, new SoundSample(sampledata, sampleidx, name, false, true));
                }

                // OJM Successfully Parsed
                return true;
            }
            catch (Exception)
            {
                // Exception was thrown when parser trying to parse OJM
                // OJM Failed to parse
                return false;
            }
        }
        #endregion

        #region --- Parse Sound M30 ---
        public static bool parseSoundM30(ref SoundSample sound, ref StreamMemoryEngine buffer, int refID, bool loop = true, bool sample = false)
        {
            try
            {
                // M30 String + Null byte
                buffer.getString(4);

                // Get OJM Header Information
                int ojmver              = buffer.getInt();
                int encsign             = buffer.getInt();
                int samplecount         = buffer.getInt();
                int sampleoffset        = buffer.getInt();
                int samplestotalsize    = buffer.getInt();
                int padding             = buffer.getInt();

                // Initialize buffer position
                buffer.Position = sampleoffset;

                // Loop whole sample
                for (int i = 0; i < samplecount; i++)
                {
                    // Get Sample Header
                    string name         = buffer.getString(32, true);
                    int samplesize      = buffer.getInt();
                    short sampletype    = buffer.getShort();
                    short unkfixeddata  = buffer.getShort();
                    int unkmusicflag    = buffer.getInt();
                    short sampleidx     = buffer.getShort();
                    short unk_zero      = buffer.getShort();
                    int pcm_samples     = buffer.getInt();

                    // is this sample that we looking for?
                    if (sampleidx != refID)
                    {
                        // No, its not, ignore it
                        buffer.Position += samplesize;
                        continue;
                    }

                    // is it empty data?
                    if (samplesize == 0)
                    {
                        // This is the sample we are looking for
                        // but the sample appear as empty data

                        // failed to parse the sound
                        return false;
                    }

                    // M### Sample
                    if (sampletype == 0)
                        sampleidx += 1000;

                    // Get Sample Data
                    byte[] sampledata = buffer.getBytes(samplesize);

                    // Decrypt data as Encryption that used in sample
                    // #TODO: I dont know any OJM encryption except nami
                    switch (encsign)
                    {
                        // Scramble1 Encryption
                        case 1:
                            break;
                        // Scramble2 Encryption
                        case 2:
                            break;
                        // Decode Encryption
                        case 4:
                            break;
                        // Decrypt Encryption
                        case 8:
                            break;
                        // Nami Encryption
                        case 16:
                            // Nami
                            byte[] nami = { 0x6E, 0x61, 0x6D, 0x69 };

                            // Loop whole data
                            for (int d = 0; d + 3 < samplesize; d += 4)
                            {
                                for (int j = 0; j < 4; j++)
                                    // Xor it
                                    sampledata[d + j] ^= nami[j];
                            }
                            break;
                        // Unknown Encryption
                        default:
                            break;
                    }

                    // Check if its valid data OGG
                    if (Encoding.UTF8.GetString(sampledata, 0, 4) != "OggS")
                    {
                        // Invalid data, delete data and go to next loop
                        sampledata = null;
                        
                        // This is the sample we are looking for
                        // but the sample appear as invalid OGG

                        // failed to parse the sound
                        return false;
                    }

                    // Set the sound and Sound was parsed successfully
                    sound = new SoundSample(sampledata, sampleidx, name, loop, sample);
                    return true;
                }

                // Couldnt find specified ID
                return false;
            }
            catch (Exception)
            {
                // Exception was thrown when parser trying to parse OJM
                // OJM Failed to parse
                return false;
            }
        }
        #endregion

        #region --- Decode Wav ---
        private static byte[] REARRANGE_TABLE = new byte[]
            {
                0x10, 0x0E, 0x02, 0x09, 0x04, 0x00, 0x07, 0x01,
                0x06, 0x08, 0x0F, 0x0A, 0x05, 0x0C, 0x03, 0x0D,
                0x0B, 0x07, 0x02, 0x0A, 0x0B, 0x03, 0x05, 0x0D,
                0x08, 0x04, 0x00, 0x0C, 0x06, 0x0F, 0x0E, 0x10,
                0x01, 0x09, 0x0C, 0x0D, 0x03, 0x00, 0x06, 0x09,
                0x0A, 0x01, 0x07, 0x08, 0x10, 0x02, 0x0B, 0x0E,
                0x04, 0x0F, 0x05, 0x08, 0x03, 0x04, 0x0D, 0x06,
                0x05, 0x0B, 0x10, 0x02, 0x0C, 0x07, 0x09, 0x0A,
                0x0F, 0x0E, 0x00, 0x01, 0x0F, 0x02, 0x0C, 0x0D,
                0x00, 0x04, 0x01, 0x05, 0x07, 0x03, 0x09, 0x10,
                0x06, 0x0B, 0x0A, 0x08, 0x0E, 0x00, 0x04, 0x0B,
                0x10, 0x0F, 0x0D, 0x0C, 0x06, 0x05, 0x07, 0x01,
                0x02, 0x03, 0x08, 0x09, 0x0A, 0x0E, 0x03, 0x10,
                0x08, 0x07, 0x06, 0x09, 0x0E, 0x0D, 0x00, 0x0A,
                0x0B, 0x04, 0x05, 0x0C, 0x02, 0x01, 0x0F, 0x04,
                0x0E, 0x10, 0x0F, 0x05, 0x08, 0x07, 0x0B, 0x00,
                0x01, 0x06, 0x02, 0x0C, 0x09, 0x03, 0x0A, 0x0D,
                0x06, 0x0D, 0x0E, 0x07, 0x10, 0x0A, 0x0B, 0x00,
                0x01, 0x0C, 0x0F, 0x02, 0x03, 0x08, 0x09, 0x04,
                0x05, 0x0A, 0x0C, 0x00, 0x08, 0x09, 0x0D, 0x03,
                0x04, 0x05, 0x10, 0x0E, 0x0F, 0x01, 0x02, 0x0B,
                0x06, 0x07, 0x05, 0x06, 0x0C, 0x04, 0x0D, 0x0F,
                0x07, 0x0E, 0x08, 0x01, 0x09, 0x02, 0x10, 0x0A,
                0x0B, 0x00, 0x03, 0x0B, 0x0F, 0x04, 0x0E, 0x03,
                0x01, 0x00, 0x02, 0x0D, 0x0C, 0x06, 0x07, 0x05,
                0x10, 0x09, 0x08, 0x0A, 0x03, 0x02, 0x01, 0x00,
                0x04, 0x0C, 0x0D, 0x0B, 0x10, 0x05, 0x06, 0x0F,
                0x0E, 0x07, 0x09, 0x0A, 0x08, 0x09, 0x0A, 0x00,
                0x07, 0x08, 0x06, 0x10, 0x03, 0x04, 0x01, 0x02,
                0x05, 0x0B, 0x0E, 0x0F, 0x0D, 0x0C, 0x0A, 0x06,
                0x09, 0x0C, 0x0B, 0x10, 0x07, 0x08, 0x00, 0x0F,
                0x03, 0x01, 0x02, 0x05, 0x0D, 0x0E, 0x04, 0x0D,
                0x00, 0x01, 0x0E, 0x02, 0x03, 0x08, 0x0B, 0x07,
                0x0C, 0x09, 0x05, 0x0A, 0x0F, 0x04, 0x06, 0x10,
                0x01, 0x0E, 0x02, 0x03, 0x0D, 0x0B, 0x07, 0x00,
                0x08, 0x0C, 0x09, 0x06, 0x0F, 0x10, 0x05, 0x0A,
                0x04, 0x00
            };

        private static byte[] decodeWave(byte[] wavedata)
        {
            byte[] res = wavedata;

            res = rearrange(res);
            res = acc_xor(res);

            return res;
        }

        private static byte[] rearrange(byte[] buf_encoded)
        {
            int length = buf_encoded.Length;
            int key = ((length % 17) << 4) + (length % 17);

            int block_size = length / 17;

            // Let's fill the buffer
            byte[] buf_plain = new byte[length];
            System.Array.Copy(buf_encoded, 0, buf_plain, 0, length);

            for (int block = 0; block < 17; block++) // loopy loop
            {
                int block_start_encoded = block_size * block;	// Where is the start of the enconded block
                int block_start_plain = block_size * REARRANGE_TABLE[key];	// Where the final plain block will be
                System.Array.Copy(buf_encoded, block_start_encoded, buf_plain, block_start_plain, block_size);

                key++;
            }
            return buf_plain;
        }


        private static int acc_keybyte = 0xFF;
        private static int acc_counter = 0;
        private static byte[] acc_xor(byte[] buf)
        {
            int temp = 0;
            byte this_byte = 0;
            for (int i = 0; i < buf.Length; i++)
            {
                temp = this_byte = buf[i];

                if (((acc_keybyte << acc_counter) & 0x80) != 0)
                {
                    this_byte = (byte)~this_byte;
                }

                buf[i] = this_byte;
                acc_counter++;
                if (acc_counter > 7)
                {
                    acc_counter = 0;
                    acc_keybyte = temp;
                }
            }
            return buf;
        }
        #endregion
    }
}
