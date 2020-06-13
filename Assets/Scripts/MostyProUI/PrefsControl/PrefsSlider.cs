using UnityEngine;
using UnityEngine.UI;


namespace MostyProUI.PrefsControl
{
    [System.Serializable]
    public class PrefsSlider 
    {
        public PrefKey prefKey; 
        public Slider slider;
        [Range(0,1f)] public float defaultValue = .5f;
        public PrefsSlider(Slider slider)
        {
            this.slider = slider;
        }

        public void SetPrefValue(float value)
        {
            PrefsController.SetRawVolumeByKey(prefKey, value); 
            
        }





    }
}