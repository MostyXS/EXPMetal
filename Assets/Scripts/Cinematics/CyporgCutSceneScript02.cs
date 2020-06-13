using MostyProUI.Audio;
using MostyProUI.PrefsControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyporgCutSceneScript02 : MonoBehaviour
{
    [SerializeField] GameObject touchdownPoint, trashedCyporg, explosion;
    [SerializeField] float landingSpeed;
    [SerializeField] AudioClip explosionSFX;
    void Update()
    {
        if (transform.position==touchdownPoint.transform.position)
        {
            MainAudioSource.Instance.value.PlayOneShot(explosionSFX, PrefsController.CharactersVolume);
            explosion.SetActive(true);
            trashedCyporg.SetActive(true);
            Destroy(gameObject);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, touchdownPoint.transform.position, landingSpeed * Time.deltaTime);
        }
    }
}
