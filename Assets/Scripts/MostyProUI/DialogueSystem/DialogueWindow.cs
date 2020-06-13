using System;
using UnityEngine;

namespace MostyProUI.DialgoueSystem
{
    [System.Serializable]
    public class DialogueWindow
    {
        [TextArea(3,10)] public string[] sentences;
        public Sprite avatar;
        public string characterName;
        public AudioClip voiceActing;
        public int nameFontSize = 70;
        public int sentenceFontSize = 60;
        public Font defaultFont;
    }



}