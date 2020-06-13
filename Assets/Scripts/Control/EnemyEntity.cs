using EXPMetal.Combat;
using EXPMetal.Utils;
using MostyProUI.PrefsControl;
using MostyProUI.Utils;
using System.Collections;
using UnityEngine;

namespace EXPMetal.Control.AI
{
    public class EnemyEntity : CharacterEntity
    {
        [SerializeField] protected float movementSpeed = 1f;
        protected bool isAttacking = false;
        protected Transform targetTransform;
        
        protected BodyDamager bodyDamager;
        protected int Direction
        {
            get
            {
                return LookDirection();
            }
        }
        protected override void Awake()
        {
            base.Awake();
            bodyDamager = GetComponent<BodyDamager>();
            if (!GameObject.FindWithTag("Player")) return;
            targetTransform = GameObject.FindWithTag("Player").transform;
        }

        protected void PerformAttackWithDelay(IEnumerator attack)
        {
            StartCoroutine(Attack(attack));
        }
        protected void SetAnimationTrigger(string animationName)
        {
            animator.SetTrigger(animationName);
        }
        protected void SetAnimationBool(string animationName, bool value)
        {
            animator.SetBool(animationName, value);
        }
        protected IEnumerator Attack(IEnumerator attack)
        {
            isAttacking = true;
            yield return (StartCoroutine(attack));
            isAttacking = false;

        }
        protected bool IsFarFromPlayer(float range)
        {
            return ComparerCalculator.IsFarFrom(transform, targetTransform, range);
        }

        protected bool AtRightHeightWithPlayer(float range)
        {
            return ComparerCalculator.AtRightHeight(transform, targetTransform, range);
        }
        protected virtual void CheckDirection() // 1 looking to left/ -1 looking to right
        {
            transform.localScale = new Vector2(defaultXScale * Direction, transform.localScale.y);
        }
        protected virtual int LookDirection()
        {
            return transform.position.x > targetTransform.transform.position.x ? 1 : -1; // 1 looking to left/ -1 looking to right
        }
        protected Coroutine WaitForCurrentAnimation()
        {
             return StartCoroutine(animator.WaitForCurrentAnimation());
        }
        
    }
}