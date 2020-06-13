using MostyProUI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EXPMetal.UI
{
    public class ButtonSceneLoader : MonoBehaviour
    {

        public void Save()
        {
            FindObjectOfType<SceneSwitcher>().Save();
        }

        public void RestartScene()
        {
            FindObjectOfType<SceneSwitcher>().RestartScene();
        }

        public void ExitToMenuFromFinalScene()
        {
            SceneManager.LoadScene(0);
        }

        public void ExitToMenu()
        {
            FindObjectOfType<SceneSwitcher>().ExitToMenu();
        }

        public void LoadSceneByIndex(int index)
        {
            SceneManager.LoadSceneAsync(index);
        }
        public void LoadLastScene()
        {
            FindObjectOfType<SceneSwitcher>().LoadLastScene();
        }


        public void ExitGame()
        {
            Application.Quit();
        }
        
        public void LoadNextScene(float fadeTime)
        {
            FindObjectOfType<SceneSwitcher>().LoadNextScene(fadeTime);
        }
        public void LoadNextSceneImmediate()
        {
            FindObjectOfType<SceneSwitcher>().LoadNextSceneImmediate();
        }
    }
}