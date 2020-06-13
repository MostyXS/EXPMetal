using EXPMetal.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EXPMetal.Combat
{
    public class Tentacle : MonoBehaviour
    {
        [SerializeField] float damage = 30f;

        Collider2D myCollider;
        private void Awake()
        {
            myCollider = GetComponent<Collider2D>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Player player = collision.GetComponent<Player>();
            if (!player) return;
            StartCoroutine(CatchAndKick(player));
        }
        private IEnumerator CatchAndKick(MonoBehaviour target)
        {
            target.enabled = false;
            target.transform.parent = transform;
            while(myCollider.enabled)
            {
                yield return null;
            }
            target.transform.parent = null;
            target.enabled = true;
        }



    }
}