using EXPMetal.Saving;
using UnityEngine;

namespace EXPMetal.Control.AI
{
    public class EnemyTrigger : MonoBehaviour,ISaveable
    {
        [SerializeField] float timeToUntrigger = 5f;
        [SerializeField] float triggerRange = 2f;
        float defaultUntriggerTime;
        
        GameObject target;
        
        public bool Triggered { get; private set; }


        private void Awake()
        {
            defaultUntriggerTime = timeToUntrigger;
            target = GameObject.FindGameObjectWithTag("Player");
        }
        private void Update()
        {
            TriggerCheck();
        }
        private void Untriggering()
        {
            if (!Triggered) return;

            timeToUntrigger -= Time.deltaTime;
            if (timeToUntrigger <= 0)
                Triggered = false;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, triggerRange);
        }
        private void TriggerCheck()
        {
            if (IsPlayerInDistance())
            {
                Untriggering();
                return;
            }
            else
            {
                Triggering();
            }
        }
        private void Triggering()
        {
            RaycastHit2D[] enemies = Physics2D.CircleCastAll(transform.position, triggerRange, Vector2.zero);
            for (int i = 0; i < enemies.Length; i++)
            {
                var nearestEnemy = enemies[i].transform.GetComponent<EnemyTrigger>();
                if (!nearestEnemy) continue;
                nearestEnemy.Triggered = true;
            }
            Triggered = true;
            timeToUntrigger = defaultUntriggerTime;
        }

        private bool IsPlayerInDistance()
        {
            return (transform.position - target.transform.position).sqrMagnitude > triggerRange*triggerRange;
        }

        public object CaptureState()
        {
            return Triggered;
        }

        public void RestoreState(object state)
        {
            timeToUntrigger = defaultUntriggerTime;
            if(!(bool)state) // state is Triggered bool
            {
                timeToUntrigger = 0f;
            }
            Triggered = (bool)state;
        }
    }
}