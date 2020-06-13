using MostyProUI.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MostyProUI.PrefsControl
{
    [ExecuteInEditMode]
    public class DefaultsController : MonoBehaviour
    {
        [Tooltip("Finds sliders in order of childs")]
        [SerializeField] List<PrefsSlider> optionSliders = new List<PrefsSlider>();
        VolumeController[] volumeChangers;

        private void Start()
        {
            CheckForVolumeChangers();
        }

        //Used for performance optimization to prevent calling on Update
        private void CheckForVolumeChangers()
        {
            volumeChangers = FindObjectsOfType<VolumeController>();
            for (int i = 0; i < optionSliders.Count; i++)
            {
                for (int j = 0; j < volumeChangers.Length; j++)
                {
                    optionSliders[i].slider.onValueChanged.AddListener(volumeChangers[j].ChangeVolumeEvent);
                }
            }
        }

        private void Awake()
        {
            SetSliders();
        }

        private void SetSliders()
        {
            for (int i = 0; i < optionSliders.Count; i++)
            {
                optionSliders[i].slider.onValueChanged.AddListener(optionSliders[i].SetPrefValue);
                optionSliders[i].slider.value = PrefsController.GetDefaultVolumeByKey(optionSliders[i].prefKey);
            }
        }
#if UNITY_EDITOR
        private void Update()
        {
            if (Application.isPlaying) return;
            
            Slider[] sliders = GetComponentsInChildren<Slider>();
            WasteCheck(sliders);
            
            if (optionSliders.Count >= sliders.Length) return;
            CreateSliderLists(sliders);
        }


        private void WasteCheck(Slider[] sliders)
        {
            if (optionSliders.Count <= sliders.Length) return;
            
                optionSliders.Remove(optionSliders[optionSliders.Count - 1]);
            
        }

        private void CreateSliderLists(Slider[] sliders)
        {

            for (int i = 0; i < sliders.Length; i++)
            {
                if (!optionSliders.Contains(sliders[i]))
                {
                    optionSliders.Add(new PrefsSlider(sliders[i]));
                }
            }
        }
#endif
        //Button Action
        public void SetDefaultSlidersValue()
        {
            for (int i = 0; i < optionSliders.Count; i++)
            {
                optionSliders[i].slider.value = optionSliders[i].defaultValue;
            }
        }
        public void SavePrefs()
        {
            PlayerPrefs.Save();
        }

      
    }
}
