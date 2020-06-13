using System.Collections.Generic;
using UnityEngine;


namespace MostyProUI.PrefsControl
{
    public static class PrefsController 
    {
        const string MASTER_VOLUME_KEY = "master volume";
        const string ENVIRONMENT_VOLUME_KEY = "environment volume";
        const string MUSIC_VOLUME_KEY = "music volume";
        const string CHARACTERS_VOLUME_KEY = "characters volume";
        public static float EnvironmentVolume
        {
            get
            {
                return GetVolumeByKey(PrefKey.EnvironmentVolume);
            }
        }
        public static float CharactersVolume
        {
            get
            {
                return GetVolumeByKey(PrefKey.CharactersVolume);
            }
        }
        static Dictionary<PrefKey, string> prefKeys = new Dictionary<PrefKey, string>()
        {
            { PrefKey.MasterVolume, MASTER_VOLUME_KEY },
            { PrefKey.EnvironmentVolume, ENVIRONMENT_VOLUME_KEY },
            { PrefKey.MusicVolume, MUSIC_VOLUME_KEY },
            { PrefKey.CharactersVolume,CHARACTERS_VOLUME_KEY }
        };
        public static void SetRawVolumeByKey(PrefKey key, float volume)
        {
            PlayerPrefs.SetFloat(prefKeys[key], Mathf.Clamp(volume, 0, 1));
            
        }

        /// <summary>
        ///  returns raw volume without any multiplies by VolumeKey.value;
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static float GetDefaultVolumeByKey(PrefKey key)
        {
            return Mathf.Clamp(PlayerPrefs.GetFloat(prefKeys[key]), 0, 1);
        }
       
        private static bool HasKey(PrefKey prefKey)
        {
            return PlayerPrefs.HasKey(prefKeys[prefKey]);
        }

        /// <summary>
        /// returns volumeKey multiplied by MasterVolume, use VolumeKey.value. Has protection from miss variable
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static float GetVolumeByKey(PrefKey key)
        {
            if(!HasKey(key))
            {
                PlayerPrefs.SetFloat(prefKeys[key], 1f);

            }
            if (prefKeys[key] == MASTER_VOLUME_KEY)
            {
                return Mathf.Clamp(PlayerPrefs.GetFloat(prefKeys[key]), 0, 1);
            }
            return Mathf.Clamp(PlayerPrefs.GetFloat(prefKeys[key]) * PlayerPrefs.GetFloat(MASTER_VOLUME_KEY), 0, 1);
        }
    }
}