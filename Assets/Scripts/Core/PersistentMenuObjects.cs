using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EXPMetal.UI
{
    
    public class PersistentMenuObjects : MonoBehaviour
    {
        [SerializeField] int menuScenesRange = 3;

        private void Awake()
        {
            CheckDestroy();
            SceneManager.sceneLoaded += DestroyIfWrongScene;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= DestroyIfWrongScene;
        }
        private void DestroyIfWrongScene(Scene scene, LoadSceneMode mode)
        {
            if (SceneManager.GetActiveScene().buildIndex > menuScenesRange)
            {
                Destroy(gameObject);
            }
        }

        private void CheckDestroy()
        {
            if (FindObjectsOfType<PersistentMenuObjects>().Length > 1)
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}