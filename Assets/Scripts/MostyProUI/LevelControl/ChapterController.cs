
using MostyProUI.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MostyProUI.LevelController
{
    [ExecuteInEditMode]
    public class ChapterController : MonoBehaviour
    {
        [SerializeField] List<LevelButton> levelButtons = new List<LevelButton>();

        private void Start()
        {
            for (int i =0; i<levelButtons.Count;i++)
            {
                levelButtons[i].button.interactable = LastLevelKeeper.MaxReachedScene >= levelButtons[i].levelToOpen;
            }
        }
#if UNITY_EDITOR
        private void Update()
        {

            if (Application.isPlaying) return;
            Button[] buttons = GetComponentsInChildren<Button>();
            WasteCheck(buttons);

            if (levelButtons.Count >= buttons.Length) return;
            CreateButtonList(buttons);

        }

        private void CreateButtonList(Button[] buttons)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                if (levelButtons.Contains(buttons[i])) continue;
                levelButtons.Add(new LevelButton(buttons[i]));
            }
        }

        private void WasteCheck(Button[] buttons)
        {
            if (levelButtons.Count <= buttons.Length) return;
            levelButtons.Remove(levelButtons[levelButtons.Count - 1]);

        }
#endif
    }
}