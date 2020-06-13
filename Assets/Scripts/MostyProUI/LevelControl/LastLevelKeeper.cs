using EXPMetal.Control;
using EXPMetal.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MostyProUI.LevelController
{
    public class LastLevelKeeper : MonoBehaviour
    {
        const string MAX_SCENE_KEY = "maxReachedScene";

        public static int MaxReachedScene
        {
            get
            {
                if(!PlayerPrefs.HasKey(MAX_SCENE_KEY))
                {
                    return 0;
                }
                return PlayerPrefs.GetInt(MAX_SCENE_KEY);
            }

        }
        private void Awake()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            if (PlayerPrefs.GetInt(MAX_SCENE_KEY) < currentSceneIndex)
            {
                PlayerPrefs.SetInt(MAX_SCENE_KEY, currentSceneIndex);
            }
        }

        public static void ResetPlayedChapters()
        {
            PlayerPrefs.SetInt(MAX_SCENE_KEY, 0);
        }
    }
}