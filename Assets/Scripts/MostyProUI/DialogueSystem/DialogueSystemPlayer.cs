using MostyProUI.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityStandardAssets.CrossPlatformInput;

namespace MostyProUI.DialgoueSystem
{
    [RequireComponent(typeof(Animator),typeof(DialogueSystemTextEditor))]
    public class DialogueSystemPlayer : MonoBehaviour
    {
        [Tooltip("Objects that triggers after dialogue")] [SerializeField] GameObject[] objectsToTrigger = null;
        [SerializeField] UnityEvent onDialogueEnd = null;
        [SerializeField] bool finalDialogue = false;
        public static bool IsActive { get; private set; } = false;

        Queue<DialogueSystemPlayerWindow> windowPlayers = new Queue<DialogueSystemPlayerWindow>();
        DialogueSystemTextEditor myEditor;
        
        Animator anim;
        bool changing = false;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            myEditor = GetComponent<DialogueSystemTextEditor>();
            
            
        }

        private void Disable()
        {
            enabled = false;
        }

        private void Start()
        {
            CreateDialogue();
            UIMenu.Instance.onPlayerDeath += Disable;

            UIMenu.Instance.Pause(false);
            IsActive = true;
        }
        public void CreateDialogue()
        {
            myEditor.UpdateDialogueWindows();
            for (int i = 0; i< transform.childCount;i++)
            {
                var child = transform.GetChild(i).gameObject;
                child.SetActive(i == 0);
                var childPlayer = child.GetComponent<DialogueSystemPlayerWindow>();
                if (!childPlayer) continue;
                childPlayer.QueueSentences();
                windowPlayers.Enqueue(childPlayer);
            }
        }
        

        void Update()
        {
            

            if (CrossPlatformInputManager.GetButtonDown("Skip") && !Mathf.Approximately(Time.deltaTime,0f))
            {
                PlayDialogue();
            }
        }

        public void PlayDialogue()
        {
            if (changing) return;
            DialogueSystemPlayerWindow sentence = windowPlayers.Peek();
            sentence.DisplaySentence();
            if (!sentence.IsEnded) return;
            StartCoroutine(PlayNext());

        }

        private IEnumerator PlayNext()
        {
            if (changing) yield break;
            changing = true;
            anim.SetTrigger("Close");
            yield return StartCoroutine(anim.WaitForCurrentAnimation());
            windowPlayers.Dequeue().gameObject.SetActive(false);
            if (windowPlayers.Count == 0)
            {
                EndDialogue();
                yield break;
            }
            windowPlayers.Peek().gameObject.SetActive(true);
            anim.SetTrigger("Open");
            yield return StartCoroutine(anim.WaitForCurrentAnimation());
            changing = false;
        }
        private void OnDestroy()
        {
            ForceEndDialogue();
        }

        private void EndDialogue()
        {
            ActivateObjects();
            InvokeDestroyActions();
            IsActive = false;
            if (IsFinalDialogue()) return;
            UIMenu.Instance.onPlayerDeath -= Disable;
            Destroy(gameObject);
            UIMenu.Instance.Resume();

        }

        private void ForceEndDialogue()
        {
            IsActive = false;
            UIMenu.Instance.onPlayerDeath -= Disable;
            UIMenu.Instance.Resume();
        }


        private void InvokeDestroyActions()
        {
            if (onDialogueEnd != null)
                onDialogueEnd.Invoke();
        }

        bool IsFinalDialogue()
        {
            if (finalDialogue)
            {
                FindObjectOfType<SceneSwitcher>().LoadNextScene(2f);
                Destroy(gameObject);
            }
            return finalDialogue;
        }

        private void ActivateObjects()
        {
            for (int i = 0; i < objectsToTrigger.Length; i++)
            {
                objectsToTrigger[i].SetActive(true);
            }
        }

        public void Skip()
        {
            windowPlayers.Peek().Skip();
            StopAllCoroutines();
            EndDialogue();

        }
    }
}

