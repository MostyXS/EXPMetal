using EXPMetal.Saving;
using MostyProUI;
using UnityEngine;
using UnityEngine.Events;

namespace EXPMetal.Triggers
{
    public class UniversalTrigger : MonoBehaviour,ISaveable
    {
        [SerializeField] UnityEvent triggerEnterActions = null;
        [SerializeField] UnityEvent triggerExitActions = null;
        [SerializeField] bool oneOff = true;
        [Header("These objects would destroy at Checkpoint loading if trigger worked")]
        [SerializeField] GameObject[] objectsToTrigger = null;

        Collider2D myCollider;

        private void Awake()
        {
            myCollider = GetComponent<Collider2D>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player")) return;

            if(triggerEnterActions!=null)
                triggerEnterActions.Invoke();
            ActivateObjects();
        }
        

        

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (!collision.CompareTag("Player")) return;
            if (UIMenu.Paused)
            {
                DeactivateObjects();
                return;
            }
            
            ActivateObjects();
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (oneOff) return;
            if (!collision.CompareTag("Player")) return;
            if (triggerExitActions != null)
                triggerExitActions.Invoke();
            DeactivateObjects();

        }

        

        private void DeactivateObjects()
        {
            for (int i = 0; i < objectsToTrigger.Length; i++)
            {
                objectsToTrigger[i].SetActive(false);
            }
        }
        private void ActivateObjects()
        {
            for (int i = 0; i < objectsToTrigger.Length; i++)
            {
                if (!objectsToTrigger[i]) continue;
                objectsToTrigger[i].SetActive(true);
            }
            if (oneOff)
                myCollider.enabled = false;
        }

        public object CaptureState()
        {
            return myCollider.enabled;
        }

        public void RestoreState(object state) //returns colliderEnabled
        {
            if (!oneOff) return;
            myCollider.enabled = (bool)state;
            bool triggerActivated = !myCollider.enabled;
            for (int i = 0; i < objectsToTrigger.Length; i++)
            {
                if (triggerActivated)
                    Destroy(objectsToTrigger[i]);
            }
            if (!triggerActivated) return;
            if (triggerEnterActions != null)
                triggerEnterActions.Invoke();

        }
    }
}