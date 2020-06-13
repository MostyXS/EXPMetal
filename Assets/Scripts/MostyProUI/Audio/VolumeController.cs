using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MostyProUI.PrefsControl
{
    public class VolumeController : MonoBehaviour
    {
        public static AudioSource MusicSource { get; private set; }

        [Header("Add all relevant audiosources here. Don't use this for one shot audios")]
        [SerializeField] PrefKey prefkey;
        AudioSource[] audioSources;
        private void Awake()
        {
            audioSources = GetComponents<AudioSource>();
           
        }
        private void Start()
        {
            UpdateVolume();
            if (prefkey == PrefKey.MusicVolume)
                MusicSource = GetComponent<AudioSource>();
        }
        public void Stop()
        {
            for (int i = 0; i < audioSources.Length; i++)
            {
                audioSources[i].Stop();
            }
        }
        public void Play()
        {
            for (int i = 0; i < audioSources.Length; i++)
            {
                audioSources[i].Play();
            }
        }

        private void UpdateVolume()
        {
            for (int i = 0; i < audioSources.Length; i++)
            {
                audioSources[i].volume = PrefsController.GetVolumeByKey(prefkey);
            }
        }
        public void ChangeVolumeEvent(float volume) // defaults event;
        {
            UpdateVolume();
        }

    }
}