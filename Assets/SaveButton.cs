using MostyProUI;
using MostyProUI.DialgoueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveButton : MonoBehaviour
{
    private void Awake()
    {
         UIMenu.Instance.onPause+=ChangeActive;
    }
    private void ChangeActive()
    {
        GetComponent<Button>().enabled = !DialogueSystemPlayer.IsActive;

    }

}
