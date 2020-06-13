using EXPMetal.Saving;
using GameDevTV.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MostyProUI.Audio
{
    public class MainAudioSource : MonoBehaviour
    {
        public static LazyValue<AudioSource> Instance
        {
            get; private set;

        }


        private void Awake()
        {
            Instance = new LazyValue<AudioSource>(GetInitialAudio);
            Instance.ForceInit();
            
        }

        public AudioSource GetInitialAudio()
        {
            return GetComponent<AudioSource>();
        }

    }
}