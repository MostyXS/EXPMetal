using System.Collections;
using UnityEngine;

namespace MostyProUI
{
    public class Fader : MonoBehaviour
    {
        //Singleton

        [Tooltip("Requires canvas with main canvas script on scene")]
        [SerializeField] float timeBetweenFading;
        Coroutine currentFade = null;

        static CanvasGroup canvasGroup;
        static Fader fader;
        public static Fader Instance
        {
            get
            {  
                if (fader == null)
                    Debug.LogError("There is no fader on scene");
                return fader;
            }
            private set
            {
                fader = value;
            }
        }
        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            Instance = this;
        }

        public void FadeOutImmediate()
        {
            canvasGroup.alpha = 1;
        }
        public void FadeInImmediate()
        {
            canvasGroup.alpha = 0;
        }
        public Coroutine FadeIn(float time)
        {
            return Fade(time, 0);
        }
        
        public Coroutine FadeOut(float time)
        {
            
            return Fade(time, 1);
        }
        private Coroutine Fade(float time, float target)
        {
            
            if (currentFade !=null)
            {
                StopCoroutine(currentFade);
            }

            currentFade = StartCoroutine(FadeRoutine(time, target));
            return currentFade;
        }
        
        IEnumerator FadeRoutine (float time, float  target)
        {
            while(!Mathf.Approximately(canvasGroup.alpha, target))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.deltaTime / time);
                yield return null;
            }

        }

    }
}
