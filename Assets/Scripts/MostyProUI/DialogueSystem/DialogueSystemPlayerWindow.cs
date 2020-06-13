using MostyProUI.PrefsControl;
using MostyProUI.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

namespace MostyProUI.DialgoueSystem
{
    [RequireComponent(typeof(AudioSource))]
    public class DialogueSystemPlayerWindow : MonoBehaviour
    {
        Queue<string> sentences = new Queue<string>();

        bool created = false;
        Dictionary<Param, GameObject> paramsLookupTable = new Dictionary<Param, GameObject>();

        public DialogueWindow Window { get; set; }
        public bool IsEnded { get; private set; } = false;
         
        bool typing = false;
        bool audioPlayed = false;


        AudioSource myAudio;


        private void Awake()
        {
            AudioSource audio = GetComponent<AudioSource>();
            if (!audio)
                gameObject.AddComponent<AudioSource>();
            myAudio = GetComponent<AudioSource>();
            UpdateParamLookupTable();
        }
        private void Start()
        {
            ClearSentence();     
        }
        public void QueueSentences()
        {
            for (int i = 0; i < Window.sentences.Length; i++)
            {
                sentences.Enqueue(Window.sentences[i]);
            }
        }
        public void PlayAudio()
        {
            if (audioPlayed) return;
            audioPlayed = true;
            if (!Window.voiceActing) return;
            myAudio.PlayOneShot(Window.voiceActing, PrefsController.CharactersVolume);
        }

        public void DisplaySentence()
        {
            PlayAudio();
            if (typing) return;
            if (sentences.Count == 0)
            {
                IsEnded = true;
                return;
            }
            string sentence = sentences.Dequeue();
            StartCoroutine(TypeSentence(sentence));
            
            
        }
        private void ClearSentence()
        {
            paramsLookupTable[Param.Sentence].GetComponent<Text>().text = "";
        }
        public void Skip()
        {
            StopAllCoroutines();
        }
        

        private IEnumerator TypeSentence(string sentence)
        {
            typing = true;
            Text dialogue = paramsLookupTable[Param.Sentence].GetComponent<Text>();
            dialogue.text = "";
            char[] sentenceAsCharArray = sentence.ToCharArray();
            for (int i = 0; i<sentenceAsCharArray.Length; i++)
            {
                char letter = sentenceAsCharArray[i];
                dialogue.text += letter;
                yield return new WaitForSeconds(.03f);
            }
            typing = false;
        }

        private void UpdateParamLookupTable()
        {
            
            paramsLookupTable.Clear();
            for (int i = 0; i < transform.childCount; i++)
            {
                var windowParam = transform.GetChild(i).GetComponent<DialogueParam>();
                paramsLookupTable.Add(windowParam.param, windowParam.gameObject);
            }
        }

        public void AddAnimatorComponent()
        {
            UpdateParamLookupTable();
        }

        public void SetActiveSentence(int sentenceNumber)
        {
            if (Window == null || Window.sentences == null || sentenceNumber >= Window.sentences.Length) return;
            UpdateParamLookupTable();
            if (!paramsLookupTable.ContainsKey(Param.Sentence)) return;
            paramsLookupTable[Param.Sentence].GetComponent<Text>().text = Window.sentences[sentenceNumber];
        }

        public void UpdateParams()
        {
            UpdateParamLookupTable();
            UpdateFonts();
            paramsLookupTable[Param.Avatar].GetComponent<Image>().sprite = Window.avatar;
            paramsLookupTable[Param.CharacterName].GetComponent<Text>().text = Window.characterName;
        }

        public void FillFromEditor()
        {
            if (created)
            {
                UpdateParams();
                return;
            }
            created = true;
            for (int i = 0; i < 3; i++)
            {
                GameObject child;
                DialogueParam windowParam;
                RectTransform rectTransform;
                DefineObjectAndAddParam(out child, out windowParam, out rectTransform);
                switch (i)
                {
                    case (0):
                        SetupAvatarObject(child, windowParam, rectTransform);
                        continue;
                    case (1):
                        SetupNameObject(child, windowParam, rectTransform);
                        continue;
                    case (2):
                        SetupSentenceObject(child, windowParam, rectTransform);
                        continue;
                }
            }
            UpdateParams();
        }

        public void UpdateFonts()
        {
            UpdateParamLookupTable();
            var nameText = paramsLookupTable[Param.CharacterName].GetComponent<Text>();
            var sentenceText = paramsLookupTable[Param.Sentence].GetComponent<Text>();
            nameText.fontSize = Window.nameFontSize;
            sentenceText.fontSize = Window.sentenceFontSize;
            nameText.font = Window.defaultFont;
            sentenceText.font = Window.defaultFont;
        }

        private void DefineObjectAndAddParam(out GameObject child, out DialogueParam dialogue, out RectTransform rectTransform)
        {
            child = new GameObject("", typeof(RectTransform));
            child.transform.SetParent(transform, false);
            child.AddComponent(typeof(DialogueParam));
            dialogue = child.GetComponent<DialogueParam>();
            rectTransform = child.GetComponent<RectTransform>();
        }

        private void SetupAvatarObject(GameObject child, DialogueParam dialogue, RectTransform rectTransform)
        {
            rectTransform.SetAnchor(new Vector2(.91f, 1));
            rectTransform.SetSize(new Vector2(128, 128));
            child.AddComponent(typeof(Image));
            if (Window.avatar != null)
                child.GetComponent<Image>().sprite = Window.avatar;
            dialogue.param = Param.Avatar;
            child.name = "Avatar";
        }

        private void SetupSentenceObject(GameObject child, DialogueParam dialogue, RectTransform rectTransform)
        {
            rectTransform.SetAnchorAndSize(new Vector2(0, 0), new Vector2(1, .7f), new Vector2(0, 0));
            rectTransform.ResetSize();
            child.AddComponent<Text>();
            dialogue.param = Param.Sentence;
            child.name = "Sentence";
        }
        private void SetupNameObject(GameObject child, DialogueParam windowParam, RectTransform rectTransform)
        {
            rectTransform.SetAnchor(new Vector2(0, .7f), new Vector2(0.8f, 1));
            rectTransform.ResetSize();
            child.AddComponent(typeof(Text));
            Text childText = child.GetComponent<Text>();
            childText.text = Window.characterName;
            windowParam.param = Param.CharacterName;
            child.name = "Name";
        }
        public void FillManually()
        {
            paramsLookupTable.Clear();
            if (Window == null)
                Window = new DialogueWindow();
            for (int i=0; i <transform.childCount;i++)
            {
                if (i >= 3) break;
                var child = transform.GetChild(i).gameObject;
                var childDialogueParam = child.GetComponent<DialogueParam>();
                if (!childDialogueParam) continue;
                Param windowParam = childDialogueParam.param;
                if (paramsLookupTable.ContainsKey(windowParam))
                {
                    Debug.LogError("There is already such param");
                    continue;
                }
                paramsLookupTable.Add(windowParam, childDialogueParam.gameObject);
                switch (windowParam)
                {
                    case (Param.Avatar):
                        SetupAvatarManually(child, childDialogueParam);
                        continue;
                    case (Param.Sentence):
                        SetupSentenceManually(child, childDialogueParam);
                        continue;
                    case (Param.CharacterName):
                        SetupNameManually(child, childDialogueParam);
                        continue;
                    default:
                        print("There is no such case for that param");
                        continue;
                }
            }
            
            Window.characterName = paramsLookupTable[Param.CharacterName].GetComponent<Text>().text;
        }

        private void SetupNameManually(GameObject child, DialogueParam childDialogueParam)
        {
            var name = childDialogueParam.GetComponent<Text>();
            if (!name)
                name = child.AddComponent<Text>();
            Window.nameFontSize = name.fontSize;
            childDialogueParam.name = "Name";
        }

        private void SetupAvatarManually(GameObject child, DialogueParam childDialogueParam)
        {
            var avatar = childDialogueParam.GetComponent<Image>();
            if (!avatar)
                child.AddComponent<Image>();
            Window.avatar = avatar.sprite;
            childDialogueParam.name = "Avatar";
        }

        private void SetupSentenceManually(GameObject child, DialogueParam childDialogueParam)
        {
            childDialogueParam.name = "Sentence";
            var sentence = childDialogueParam.GetComponent<Text>();
            if (sentence)
            {
                Window.defaultFont = sentence.font;
                Window.sentenceFontSize = sentence.fontSize;
            }
            else
            {
                child.AddComponent<Text>();
            }
        }
    }
}
