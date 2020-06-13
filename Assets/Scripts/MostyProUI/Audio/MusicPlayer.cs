using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MostyProUI
{
    public class MusicPlayer : MonoBehaviour
    {
        [SerializeField] AudioClip[] music;
        AudioSource myAudioSource;
        int currentPlaying = 0;

        private void Awake()
        {
            myAudioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            PlayMusic();
        }
        private void PlayMusic()
        {
            if (myAudioSource.isPlaying) return;

            if (!(currentPlaying == music.Length - 1))
            {
                myAudioSource.clip = music[currentPlaying + 1];
                myAudioSource.Play();
                currentPlaying++;
            }
            else
            {
                myAudioSource.clip = music[0];
                myAudioSource.Play();
                currentPlaying = 0;
            }

        }
    }
}