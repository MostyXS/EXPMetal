using MostyProUI.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MostyProUI.DialgoueSystem
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(DialogueSystemPlayer))]
    public class DialogueSystemTextEditor : MonoBehaviour
    {
        [SerializeField] DialogueWindow[] rawDialogues = null;
        List<DialogueSystemPlayerWindow> playerWindows = new List<DialogueSystemPlayerWindow>();
        [Space][Tooltip("Double Click to use")][SerializeField] bool FillFromEditorButton = false,  FillFromChildsButton = false;

        [Range(0, 20)] [Tooltip("Write element number to activate")] [SerializeField] int activeObject = 0;
        [Range(0, 50)] [SerializeField] int activeSentence = 0;

        Coroutine activeDoubleClick = null;

        public void UpdateDialogueWindows() // Used by dialogue system player, when starting, because of nulling playerwindows;
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var childPlayer = transform.GetChild(i).GetComponent<DialogueSystemPlayerWindow>();
                childPlayer.Window = rawDialogues[i];
            }
        }
#if UNITY_EDITOR
        private void Update()
        {
            if (Application.isPlaying) return;

            if (activeDoubleClick != null) return;

            if (FillFromChildsButton)
                FillManuallyByButton();

            if (FillFromEditorButton)
                FillFromEditorByButton();

            UpdateActive();
        }
        private void UpdateActive()
        {
            for(int i = 0; i <playerWindows.Count;i++)
            {
                var windowPlayer = playerWindows[i];

                if (!windowPlayer)
                {
                    playerWindows.Remove(windowPlayer);
                    continue;
                }
                if (windowPlayer.Window == null) continue;
                windowPlayer.gameObject.SetActive(i == activeObject);
                if(i < rawDialogues.Length)
                    windowPlayer.Window.sentences = rawDialogues[i].sentences;
                windowPlayer.SetActiveSentence(activeSentence);
            }
        }
        private void FillFromEditorByButton()
        {
            activeDoubleClick = StartCoroutine(MissClickFillFromEditorCoroutine(1));
        }
        private void FillManuallyByButton()
        {
            activeDoubleClick = StartCoroutine(MissClickFillManuallyCoroutine(1));
        }
        private IEnumerator MissClickFillManuallyCoroutine(float delay)
        {
            FillFromChildsButton = false;
            float timeSinceClick = 0;
            while (timeSinceClick < delay && !FillFromChildsButton)
            {
                timeSinceClick += Time.deltaTime;
                yield return null;
            }
            activeDoubleClick = null;
            FillFromChildsButton = false;
            if (timeSinceClick >= delay)
            {
                yield break;
            }
            playerWindows.Clear();
            FillManually();
            UpdatePlayerWindows();
        }

        private void FillManually()
        {
            rawDialogues = new DialogueWindow[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                DialogueSystemPlayerWindow childDialogue = child.GetComponent<DialogueSystemPlayerWindow>();
                if (!childDialogue)
                {
                    Debug.LogError("No dialogue player on object" + child.name);
                    continue;
                }
                childDialogue.FillManually();
                playerWindows.Add(childDialogue);
                if(rawDialogues[i] != null)
                childDialogue.Window.sentences = rawDialogues[i].sentences;
                
                rawDialogues[i] = childDialogue.Window;
            }
        }

        private IEnumerator MissClickFillFromEditorCoroutine(float delay)
        {
            FillFromEditorButton = false;
            float timeSinceClick = 0;
            while (timeSinceClick < delay && !FillFromEditorButton)
            {
                timeSinceClick += Time.deltaTime;
                yield return null;
            }
            activeDoubleClick = null;
            if (timeSinceClick >= delay)
            {
                yield break;
            }
            FillFromEditorButton = false;

            UpdatePlayerWindows();
            DestroyChilds();
            playerWindows.Clear();
            CreateDialogues();
            UpdateActive();
        }
        private void UpdatePlayerWindows()
        {
            playerWindows.Clear();
            for (int i =0; i<transform.childCount; i++)
            {
               var childPlayer = transform.GetChild(i).GetComponent<DialogueSystemPlayerWindow>();
               playerWindows.Add(childPlayer);
            }
        }
        

        private void DestroyChilds()
        {
            for (int i = 0; i < playerWindows.Count; i++)
            {
                if(playerWindows[i])
                DestroyImmediate(playerWindows[i].gameObject);
            }
        }

        private void CreateDialogues()
        {
            for (int i = 0; i < rawDialogues.Length; i++)
            {
                CreateNewDialogueWindow(i);
            }
        }

        private void CreateNewDialogueWindow(int i)
        {
            GameObject window;
            CreateWindowObject(i, out window);
            UpdateRect(window);
            UpdateWindowParametrs(i, window);
        }

        private void UpdateWindowParametrs(int i, GameObject window)
        {
            playerWindows.Add(window.AddComponent<DialogueSystemPlayerWindow>());
            playerWindows[i].Window = rawDialogues[i];
            playerWindows[i].FillFromEditor();
        }

        private void CreateWindowObject(int i, out GameObject windowObject)
        {
            string name = rawDialogues[i].characterName +$" ({i})";
            windowObject = new GameObject(name, typeof(RectTransform));
            windowObject.transform.SetParent(transform, false);
        }

        private static void UpdateRect(GameObject window)
        {
            var rect = window.GetComponent<RectTransform>();
            rect.SetAnchor(new Vector2(0, 0), new Vector2(1, 1));
            rect.ResetSize();
        }
#endif
    }
}
