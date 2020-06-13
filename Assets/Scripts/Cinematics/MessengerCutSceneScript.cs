using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessengerCutSceneScript : MonoBehaviour
{
    [SerializeField] GameObject dialogueToTrigger, dialogueToWait;
    [SerializeField] Transform pointToReach;
    [SerializeField] float movementSpeed;
    private void Update()
    {
        if (transform.position == pointToReach.position)
        {
            ActivateLastDialogue();

        }
        else if (!dialogueToWait)
        {
            MoveToTrigger();
        }
    }

    private void MoveToTrigger()
    {
        GetComponent<Animator>().SetBool("Walking", true);
        transform.position = Vector3.MoveTowards(transform.position, pointToReach.position, movementSpeed*Time.deltaTime);
    }

    private void ActivateLastDialogue()
    {
        dialogueToTrigger.SetActive(true);
        GetComponent<Animator>().SetBool("Walking", false);
        FindObjectOfType<BaronCutSceneScript>().ChangeDirection();
        enabled = false;
    }
}
