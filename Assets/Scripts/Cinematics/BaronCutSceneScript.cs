using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaronCutSceneScript : MonoBehaviour
{
    [SerializeField] GameObject objectToWait, firstDialogueToActivate, secondDialogueToActivate;
    [SerializeField] Transform startingPoint, endingPoint;
    [SerializeField] float movementSpeed;

    bool onFirstPosition;
    void Update()
    {
        if (transform.position == endingPoint.position)
        {
            TriggerSecondDialogue();
        }
        else if(!firstDialogueToActivate)
        {
            WalkToSecondPosition();
        }
        else if (transform.position == startingPoint.position && !onFirstPosition)
        {
            TriggerFirstDialogue();
        }
        else if (!objectToWait && !onFirstPosition)
        {
            WalkToFirstPosition();
        }
    }

    private void TriggerFirstDialogue()
    {
        onFirstPosition = true;
        DeactivateWalking();
        firstDialogueToActivate.SetActive(true);
    }
    private void TriggerSecondDialogue()
    {
        DeactivateWalking();
        secondDialogueToActivate.SetActive(true);
        enabled = false;
    }

    private void WalkToFirstPosition()
    {
        ActivateWalking();
        transform.position = Vector2.MoveTowards(transform.position, startingPoint.position, movementSpeed * Time.deltaTime);
    }
    private void WalkToSecondPosition()
    {
        ActivateWalking();
        transform.position = Vector2.MoveTowards(transform.position, endingPoint.position, movementSpeed * Time.deltaTime);
    }

    private void ActivateWalking()
    {
        GetComponent<Animator>().SetBool("Walking", true);
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Animator>().SetBool("Walking", true);
        }
    }
    private void DeactivateWalking()
    {
        GetComponent<Animator>().SetBool("Walking", false);
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Animator>().SetBool("Walking", false);
        }
    }
    public void ChangeDirection()
    {
        GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<SpriteRenderer>().flipX= !(transform.GetChild(i).GetComponent<SpriteRenderer>().flipX);  
        }
    }
}
