using MostyProUI.Audio;
using MostyProUI.PrefsControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCinematic : MonoBehaviour
{
    [Tooltip("Dialogue to wait before explosion")]
    [SerializeField] GameObject dialogueToWait;
    [SerializeField] AudioClip explosionSFX;
    Animator myAnimator;

    bool activated = false;
    private void Start()
    {
        myAnimator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (dialogueToWait || activated) return;

        ActivateExplosion();
        
    }

    private void ActivateExplosion()
    {
        activated = true;
        MainAudioSource.Instance.value.PlayOneShot(explosionSFX,PrefsController.CharactersVolume);
        myAnimator.SetTrigger("Explosion");
        transform.GetChild(0).gameObject.SetActive(true);
        FindObjectOfType<CyporgCutSceneScript>().Explode();
    }

}
