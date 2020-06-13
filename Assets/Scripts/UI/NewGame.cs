using EXPMetal.Saving;
using MostyProUI;
using MostyProUI.LevelController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace EXPMetal.UI
{
    public class NewGame : MonoBehaviour
    {
        [SerializeField] Text tipText = null;
        [SerializeField] float delayUntilClick = 2f;
        [SerializeField] int firstScene = 3;

        float timeSinceLastClicked = Mathf.Infinity;

        public void StartNewGame()
        {
            if (timeSinceLastClicked > delayUntilClick && SceneSwitcher.FileExists())
            {
                tipText.color = new Color(tipText.color.r, tipText.color.g, tipText.color.b, 1f);
                timeSinceLastClicked = 0;
                return;
            }
            SceneSwitcher.DeleteFile();
            LastLevelKeeper.ResetPlayedChapters();
            SceneManager.LoadSceneAsync(firstScene);
        }
        private void Update()
        {
            if (Mathf.Approximately(tipText.color.a,0f)) return;
            timeSinceLastClicked += Time.deltaTime;
            tipText.color = new Color(tipText.color.r, tipText.color.g, tipText.color.b, tipText.color.a-Time.deltaTime/delayUntilClick);
        }



    }
}