////////////////////////////////////////
// Sound Engine                       //
// Version              : 1.0         //
// Author               : CXO2        //
//////////////////////////////////////// 

using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using FMOD;

namespace CXO2.Core.SoundSystem
{
    // Actually this my old class that I saved few months ago 
    // its used for old my CygnusJam project lol

    // ~ Sound Engine Class
    public static class SoundEngine
    {
        #region --- Variables ---
        static FMOD.System _system;
        static List<FMOD.Channel> _channels;
        static FMOD.RESULT _result;
        #endregion

        #region --- Init / Dispose ---
        // Initialize FMOD System
        public static bool init()
        {
            try
            {
                _system = null;
                result = FMOD.Factory.System_Create(ref _system);
                result = _system.init(100, INITFLAGS.NORMAL, IntPtr.Zero);

                _channels = new List<Channel>();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Release FMOD Sound System
        public static void Dispose()
        {
            // dispose it
            result = _system.release();
        }
        #endregion

        #region --- Methods ---
        public static void update()
        {
            // Variable for playing status
            bool playing = false;

            // Loop backward to remove item from list
            for (int i = _channels.Count - 1; i >= 0; i--)
            {
                // Check for playing Channel
                _channels[i].isPlaying(ref playing);

                // Remove unplaying sound
                if (!playing)
                    _channels.RemoveAt(i);
            }

            // Update Sound System FMOD for Housekeeping
            _system.update();
        }

        // play the sound
        public static FMOD.Channel playSound(SoundSample sound)
        {
            return playSound(sound, false);
        }

        // play the sound with paused status
        public static FMOD.Channel playSound(SoundSample sound, bool paused)
        {
            // Channel used for play the sound
            FMOD.Channel sndch = null;
            
            // is the sound empty?
            if (sound == null)
                result = RESULT.ERR_MUSIC_NOTFOUND;
            else
                // no, lets play the sound
                result = _system.playSound(CHANNELINDEX.FREE, sound.getSound(), paused, ref sndch);

            // sound isnt empty, add it to list channels
            if (sndch != null)
                _channels.Add(sndch);

            // return the channel
            return sndch;
        }

        // Stop the sound
        public static bool stopSound(SoundSample sound)
        {
            // get channel
            FMOD.Channel sndch = getPlayingCh(sound);

            // is channel exist?
            if (sndch != null)
            {
                // stop the channel
                sndch.stop();
                return true;
            }

            // no, channel isnt exist
            return false;
        }

        // Get position of playing sound (in Millisecond)
        public static uint getPosition(SoundSample sound)
        {
            // Get Channel
            FMOD.Channel sndch = getPlayingCh(sound);
            uint position = 0;

            // is channel exist?
            if (sndch != null)
                sndch.getPosition(ref position, TIMEUNIT.MS);

            // return the Position
            return position;
        }

        // Set the playing sound position (in Millisecond)
        public static bool setPosition(SoundSample sound, uint position)
        {
            // Get playing sound
            FMOD.Channel sndch = getPlayingCh(sound);

            // is channel exist?
            if (sndch != null)
            {
                // Set the channel position
                sndch.setPosition(position, TIMEUNIT.MS);
                return true;
            }

            // No, channel isnt exist
            return false;
        }

        // Get the volume of playing sound
        public static float getVolume(SoundSample sound)
        {
            // Get the channel
            FMOD.Channel sndch = getPlayingCh(sound);
            float vol = -1;

            // Is channel exist?
            if (sndch != null)
                //  Set channel position
                sndch.getVolume(ref vol);

            // no, channel isnt exist
            return vol;
        }

        // Set Volume of playing sound
        public static bool setVolume(SoundSample sound, float vol)
        {
            // get playing channel
            FMOD.Channel sndch = getPlayingCh(sound);

            // is channel exist?
            if (sndch != null)
            {
                // set channel volume
                sndch.setVolume(vol);
                return true;
            }

            // no, channel isnt exist
            return false;
        }

        // Get Pan of playing sound
        public static float getPan(SoundSample sound)
        {
            // get playing channel
            FMOD.Channel sndch = getPlayingCh(sound);
            float pan = -1;

            // is channel exist?
            if (sndch != null)
                // get the pan
                sndch.getPan(ref pan);

            // return it
            return pan;
        }

        // Set Pan of playing sound
        public static bool setPan(SoundSample sound, float pan)
        {
            // get playing channel
            FMOD.Channel sndch = getPlayingCh(sound);

            // is channel exist?
            if (sndch != null)
            {
                // set the pan
                sndch.setPan(pan);
                return true;
            }

            // no, channel isnt exist
            return false;
        }

        // Stop all playing sound
        public static void stopAllSound()
        {
            // Iterate to all sound and stop the channel
            for (int i = 0; i < _channels.Count; i++)
                _channels[i].stop();

            // clear the channel
            _channels.Clear();
        }

        // Get playing sound channel
        public static FMOD.Channel getPlayingCh(SoundSample sound)
        {
            // Iterate the channel
            for (int i = 0; i < _channels.Count; i++)
            {
                // get the sound of channel
                FMOD.Sound fsound = null;
                _channels[i].getCurrentSound(ref fsound);

                // is the sound same?
                if (sound.getSound() == fsound)
                    // return the channel
                    return _channels[i];
            }

            // couldnt find the channel
            return null;
        }

        // Get the FMOD System
        public static FMOD.System getSystem()
        {
            // return it
            return _system;
        }

        // Get the FMOD Channel
        public static List<FMOD.Channel> getChannels()
        {
            // return it
            return _channels;
        }
        #endregion

        #region --- Members ---
        // Used for assign and get result FMOD Sound System
        // Should I put error checking here?
        public static FMOD.RESULT result
        {
            get
            {
                return _result;
            }
            set
            {
                _result = value;

                // Should I put this?
                //if (result != RESULT.OK)
                //    Trace.WriteLine("Fatal error at SoundSystem: " + FMOD.Error.String(result), "FATAL ERROR");
            }
        }
        #endregion
    }
}
