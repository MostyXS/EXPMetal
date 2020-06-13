using MostyProUI.Audio;
using MostyProUI.PrefsControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EXPMetal.UI
{
    public class ButtonSFX : MonoBehaviour
    {
        [SerializeField] AudioClip buttonSFX;


        public void ClickSFX()
        {
            
            MainAudioSource.Instance.value.PlayOneShot(buttonSFX, PrefsController.EnvironmentVolume);
        }
    }
}