using EXPMetal.Saving;
using GameDevTV.Utils;
using MostyProUI.DialgoueSystem;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;


namespace MostyProUI
{
    [RequireComponent(typeof(SavingSystem))]
    public class SceneSwitcher : MonoBehaviour
    {
        [SerializeField] int menuScenesRange = 2;
        
        SavingSystem savingSystem;
        const string expMetalSave = "SaveFile";

        public void ExitToMenu()
        {
            StartCoroutine(LoadMenu());
        }
        private IEnumerator LoadMenu()
        {
            UIMenu.Instance.Resume();
            yield return SceneManager.LoadSceneAsync(0);
        }

        public static bool CanSave { get; set; }
        public static SceneSwitcher Instance
        {
            get; private set;
        }
        private void Awake()
        {
            Instance = this;
            savingSystem = GetComponent<SavingSystem>();
            CheckForMenuSceneRange();
            SceneManager.sceneLoaded += OnSceneLoad;
        }
        public static bool FileExists()
        {
            return SavingSystem.FileExists(expMetalSave);
        }
        public void LoadLastScene()
        {
            StartCoroutine(savingSystem.LoadLastScene(expMetalSave));
        }

        void OnSceneLoad(Scene scene, LoadSceneMode mode)
        {
            CheckForMenuSceneRange();
        }

        private void CheckForMenuSceneRange()
        {
            if (SceneManager.GetActiveScene().buildIndex <= menuScenesRange)
            {
                enabled = false;
            }
            else
            {
                enabled = true;
            }
        }

        private void Update()
        {
            if (UIMenu.Paused) return;
            if(CrossPlatformInputManager.GetButtonDown("Load"))
            {
                Load();
            }
            if(CrossPlatformInputManager.GetButtonDown("Save"))
            {
                Save();
            }
            if (CrossPlatformInputManager.GetButtonDown("Delete"))
            {
                DeleteFile();
            }
        }

        
        public static void DeleteFile()
        {
            SavingSystem.Delete(expMetalSave);
        }

        public void Save()
        {
            savingSystem.Save(expMetalSave);
        }

        public void Load()
        {
            
            StartCoroutine(savingSystem.LoadLastScene(expMetalSave));
        }

        public void LoadStartMenu()
        {
            SceneManager.LoadScene("00 Menu");
        }
        public void LoadNextSceneImmediate()
        {
            if (!UIMenu.CanInteractWithGame) return;
            StartCoroutine(LoadSceneByIndex(SceneManager.GetActiveScene().buildIndex + 1,0f));
        }
        public void RestartScene()
        {
            StartCoroutine(Restart());
        }
        IEnumerator Restart()
        {
            UIMenu.Instance.Resume();
            yield return SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
        public void LoadNextScene()
        {
            if (!UIMenu.CanInteractWithGame) return;
            StartCoroutine(LoadSceneByIndex(SceneManager.GetActiveScene().buildIndex + 1));
        }
        public void LoadNextScene(float fadeTime)
        {
            if (!UIMenu.CanInteractWithGame) return;
            StartCoroutine(LoadSceneByIndex(SceneManager.GetActiveScene().buildIndex + 1,fadeTime));
        }
        public IEnumerator LoadSceneByIndex(int sceneToLoad)
        {
            UIMenu.Instance.Pause(false);
            yield return Fader.Instance.FadeOut(3f);
            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            yield return Fader.Instance.FadeIn(3f);
            UIMenu.Instance.Resume();
        }
        public IEnumerator LoadSceneByIndex(int sceneToLoad, float time)
        {
            UIMenu.Instance.Pause(false);
            yield return Fader.Instance.FadeOut(time);
            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            yield return Fader.Instance.FadeIn(time);
            UIMenu.Instance.Resume();
        }
        public IEnumerator LoadSceneByName(string name)
        {
            UIMenu.Instance.Pause(false);
            yield return Fader.Instance.FadeOut(2f);
            yield return SceneManager.LoadSceneAsync(name);
            yield return Fader.Instance.FadeIn(2f);
            UIMenu.Instance.Resume();
        }
        public IEnumerator LoadSceneByName(string name,float time)
        {
            UIMenu.Instance.Pause(false);
            yield return Fader.Instance.FadeOut(time);
            yield return SceneManager.LoadSceneAsync(name);
            yield return Fader.Instance.FadeIn(time);
            UIMenu.Instance.Resume();
        }
        
    }
}