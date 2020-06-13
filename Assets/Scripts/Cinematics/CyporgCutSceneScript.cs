using MostyProUI;
using MostyProUI.PrefsControl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyporgCutSceneScript : MonoBehaviour
{
    
    [SerializeField] GameObject dialogueToWait, dialogueToActivate;
    [SerializeField] float timeUntilNextDialogue, explodingStrength;
    [SerializeField] AudioClip secondFireClip;

    Vector3 positionToFly = new Vector3(-1.22f, 1.99f);            //Used for cinematic, sorry for crude coding

    bool pigletGunActivated,dying;
    Animator myAnimator;
    AudioSource myAudioSource;


    private void Start()
    {
        myAnimator = GetComponent<Animator>();
        myAudioSource = GetComponent<AudioSource>();
        myAudioSource.volume = PrefsController.CharactersVolume;
    }
    private void Update()
    {
        if (transform.position == positionToFly)
            SceneSwitcher.Instance.LoadNextSceneImmediate();
        else if (dying)
            transform.position = Vector2.MoveTowards(transform.position, positionToFly, explodingStrength*Time.deltaTime);
        else
            Firing();
    }

    private void Firing()
    {
        if (!myAudioSource.isPlaying && pigletGunActivated)
        {
            myAudioSource.clip = secondFireClip;
            myAudioSource.Play();
        }
        else if (!dialogueToWait)
        {
            Fire();

        }
    }

    private void Fire()
    {  
        if(!myAudioSource.isPlaying)
            myAudioSource.Play();
        myAnimator.SetTrigger("Fire");
    }
    private void SetBombAvoid()
    {
        FindObjectOfType<BombCinematic>().gameObject.GetComponent<Animator>().SetTrigger("Avoid");
    }

    private IEnumerator NextPart() //using in animatior
    {

        if (!pigletGunActivated)
        {
            
            pigletGunActivated = true;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
            yield return new WaitForSeconds(timeUntilNextDialogue);
            dialogueToActivate.SetActive(true);
        }    
    }
    public void Explode()
    {
        dying = true;
        myAudioSource.Stop();
        for (int i =0; i<transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }    
        myAnimator.SetTrigger("Explode");

    }
}
