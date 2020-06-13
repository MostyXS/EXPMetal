using UnityEngine;
using UnityEngine.Events;
using UnityStandardAssets.CrossPlatformInput;

namespace EXPMetal.Triggers
{
    public class UseInTriggerZone : MonoBehaviour
    {
        public UnityEvent onUse;

        private void Update()
        {
            if(CrossPlatformInputManager.GetButton("Use"))
            {
                onUse.Invoke();
            }
        }
    }
}

