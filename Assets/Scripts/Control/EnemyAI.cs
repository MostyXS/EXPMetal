using EXPMetal.Combat;
using EXPMetal.Utils;
using MostyProUI;
using System.Collections;
using UnityEngine;

namespace EXPMetal.Control.AI
{
    public class EnemyAI : EnemyEntity
    {
        [Header("Main parametrs")]
        [SerializeField] float attackRange = 0.6f;

        [Header("Additional Parametrs")]
        [Range(0.1f, 2f)] [SerializeField] float attackDelay = 0.1f;
        
        EnemyTrigger myEnemyTrigger;
        Collider2D myCollider;
        protected override void Awake()
        {
            base.Awake();
            myEnemyTrigger = GetComponent<EnemyTrigger>();
            myCollider = GetComponent<Collider2D>();
        }
        protected override void Start()
        {
            base.Start();
            GetComponent<Health>().onDeath += OnDeath;
            UIMenu.Instance.onPause += Pause;
        }
        void Update()
        {
            if (UIMenu.Paused) return;
            ProcessControl();
        }
        public void OnDeath()
        {
            Pause();
            myCollider.enabled = false;
            enabled = false;
        }
        private void Pause()
        {
            if (GetComponent<Rigidbody>())
                GetComponent<Rigidbody>().velocity = new Vector2(0, 0);
            animator.SetBool("Walking", false);
        }

        private void ProcessControl()
        {
            if (!myEnemyTrigger.Triggered)
            {
                animator.SetBool("Walking", false);
                return;
            }
            if (isAttacking) return;
            EngageTarget();
            if (IsFarFromPlayer(attackRange) ) return;
            PerformAttackWithDelay(BaseAttack());
        }

        private void EngageTarget()
        {
            animator.SetBool("Walking", true);
            CheckDirection(); // inherited from enemy entity
            Vector2 moveToPosition = new Vector2(movementSpeed * Time.deltaTime, 0);
            transform.position -= (Vector3)moveToPosition * Direction;
        }
        private IEnumerator BaseAttack()
        {
            animator.SetTrigger("Attack");
            yield return new WaitForSeconds(attackDelay);
        }

    }
}