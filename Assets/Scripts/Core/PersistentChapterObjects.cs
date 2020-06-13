using EXPMetal.Control;
using EXPMetal.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentChapterObjects : MonoBehaviour
{
    [SerializeField] string chapterName= null;
    
    private void Start()
    {
        SceneManager.sceneLoaded += DestroyIfWrongScene;
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= DestroyIfWrongScene;
    }
    private void DestroyIfWrongScene(Scene scene, LoadSceneMode mode)
    {
        PersistentChapterObjects[] players = FindObjectsOfType<PersistentChapterObjects>();
        if (players.Length == 1)
        {
            Destroy(gameObject);
            return;
        }
        foreach (PersistentChapterObjects player in players)
        {
            if (player == this) continue;
            if (player.GetID() == chapterName)
            {
                Destroy(player.gameObject);
                continue;
            }

            Destroy(gameObject);
        }
    }
    public string GetID()
    {
        return chapterName;
    }

}
