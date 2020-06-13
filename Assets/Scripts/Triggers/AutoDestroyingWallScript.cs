using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EXPMetal.Triggers
{
    public class AutoDestroyingWallScript : MonoBehaviour
    {
        [SerializeField] List<GameObject> enemiesToAwait = new List<GameObject>();

        void Update()
        {
            DestroyCheck();
            ObjectsExistChecking();

        }

        private void DestroyCheck()
        {
            if (enemiesToAwait.Count == 0)
                Destroy(gameObject);
        }

        private void ObjectsExistChecking()
        {

            for (int i = 0; i < enemiesToAwait.Count; i++)
            {
                if ((enemiesToAwait[i].activeInHierarchy)) continue;

                enemiesToAwait.Remove(enemiesToAwait[i]);

            }
        }
    }
}
