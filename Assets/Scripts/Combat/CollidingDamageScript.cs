using EXPMetal.Combat;
using MostyProUI.Audio;
using MostyProUI.PrefsControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EXPMetal.Combat
{
    public class CollidingDamageScript : MonoBehaviour
    {
        [SerializeField] AudioClip hitSFX;
        [SerializeField] float damage = 30f;

        AudioSource mainAudio;

        private void Start()
        {
            mainAudio = MainAudioSource.Instance.value;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<Health>())
            {
                mainAudio.PlayOneShot(hitSFX, PrefsController.CharactersVolume);
                collision.GetComponent<Health>().TakeDamage(damage);
            }

        }
    }
}