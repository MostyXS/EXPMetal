using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StellaScaringOnBoss : MonoBehaviour
{
    [SerializeField] GameObject dialogueToDestroy;


    private void Update()
    {
        if (!dialogueToDestroy)
        {
            GetComponent<Animator>().SetBool("Scared", true);
            enabled = false;
        }
    }

}
