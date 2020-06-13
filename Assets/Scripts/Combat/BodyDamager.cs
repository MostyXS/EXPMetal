using EXPMetal.Control;
using EXPMetal.Utils;
using MostyProUI.Audio;
using System.Collections;
using UnityEngine;


namespace EXPMetal.Combat
{
    public class BodyDamager : TouchDamager
    {
        [SerializeField] float stepBackSpeed = .5f;
        [SerializeField] float stepBackDistance = 1f;
        bool canStepBack = true;
        private void Start()
        {
            if (!GetComponent<Health>()) return;
            GetComponent<Health>().onDeath += Disable;
        }
        private void OnCollisionEnter2D(Collision2D other)
        {
            DamageTarget(other.collider);
        }
        public void Disable()
        {
            enabled = false;
        }
        protected override void Knock(Transform target, Vector2 direction)
        {
            base.Knock(target,direction);
            if (!canStepBack) return;
            StartCoroutine(StepBack(direction));
            
        }
        public void SetStepBack(bool value)
        {
            canStepBack = value;
        }

        private IEnumerator StepBack(Vector2 rawDirection)
        {
            int direction = rawDirection.x > 0 ? 1 : -1; 
            Vector2 stepBackPosition = new Vector2(transform.position.x + stepBackDistance * direction, transform.position.y);
            float timeSinceBacking = 0f;
            while((Vector2)transform.position !=stepBackPosition)
            {
                if (timeSinceBacking >= .5f) break;
                timeSinceBacking += Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, stepBackPosition, stepBackSpeed * Time.deltaTime);
                yield return null;
            }
        }

    }

}