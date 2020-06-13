using EXPMetal.Saving;
using MostyProUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EXPMetal.UI
{
    public class ContinueButton : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Button>().interactable = SceneSwitcher.FileExists(); 
        }
    }
}