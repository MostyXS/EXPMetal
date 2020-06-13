using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EXPMetal.Triggers
{
    public class DestroyOnStart : MonoBehaviour
    {
        [SerializeField] float destroyingTime = 0.1f;

        void Start()
        {
            StartCoroutine(Destroy());
        }

        private IEnumerator Destroy()
        {
            yield return new WaitForSeconds(destroyingTime);
            Destroy(gameObject);
        }
    }
}