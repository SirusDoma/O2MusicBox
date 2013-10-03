////////////////////////////////////////
// Sound Sample                       //
// Version              : 1.0         //
// Author               : CXO2        //
//////////////////////////////////////// 

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using FMOD;

namespace CXO2.Core.SoundSystem
{
    // ~ Sound Sample Class
    public class SoundSample
    {
        #region --- Variables ---
        FMOD.Sound _sound;
        int _id;
        byte[] _data;
        string _name;
        bool _isLooping;
        #endregion

        #region --- Constructor ---
        // Constructor #1       : Load from data bytes
        public SoundSample(byte[] data,int id, string name = "", bool loop = false, bool sample = false)
        {
            // Load data
            load(data, id, name, loop, sample);
        }

        // Constructor #2       : Load from path
        public SoundSample(string path)
        {
            // Set data to null because loaded from path
            _data = null;

            // Create the sound
	        SoundEngine.result = SoundEngine.getSystem().createSound(path, FMOD.MODE.CREATESTREAM, ref _sound);
        }
        #endregion

        #region --- Methods ---
        // Release the sound
        public void Dispose()
        {
            _sound.release();
            _data = null;
        }

        // load data
        void load(byte[] data, int id, string name, bool loop, bool sample)
        {
            // Assign it
            _data = data;
            _id = id;
            _name = name;

            // Create the exinfo for play the sound data
            FMOD.CREATESOUNDEXINFO exinfo = new CREATESOUNDEXINFO();
            exinfo.cbsize = Marshal.SizeOf(exinfo);
            exinfo.length = (uint)data.Length;

            // Set the proper mode
            FMOD.MODE mode = FMOD.MODE.HARDWARE | FMOD.MODE.OPENMEMORY;
            if (loop) mode |= FMOD.MODE.LOOP_NORMAL;
            if (sample) mode |= FMOD.MODE.CREATESAMPLE;
            else mode |= FMOD.MODE.CREATESTREAM;

            // is the data looping?
            // NOTE: looping cannot be changed after this
            _isLooping = loop;

            // Create the sound
            SoundEngine.result = SoundEngine.getSystem().createSound(_data, mode, ref exinfo, ref _sound);

            // if its sample, clear the data
            if (sample)
                _data = null;
        }

        // Get the data sound
        public FMOD.Sound getSound()
        {
            return _sound;
        }

        // Get the ID of Sample (used for #W (id with 1000+ = M#))
        public int getID()
        {
            return _id;
        }

        // Set the name of sample
        public void setName(string name)
        {
            _name = name;
        }

        // Get the name of sample
        public string getName()
        {
            return _name;
        }

        // Is the sound Looping?
        public bool isLoopng()
        {
            return _isLooping;
        }
        #endregion
    }
}
