using EXPMetal.Utils;
using UnityEngine;


namespace EXPMetal.Combat
{
    public class TouchDamager : MonoBehaviour
    {
        [SerializeField] float damage = 10f;
        [Header("Knock parametrs")]
        [Tooltip("Can knock after damage, inheritance from touchDamager")]

        [SerializeField] bool haveKnockback = true;
        [Range(.5f, 5f)] [SerializeField] protected float knockbackPower = 1f;

        float defaultTouchDamage;
        float defaultKnockbackPower;

        protected virtual void Awake()
        {
            defaultTouchDamage = damage;
            defaultKnockbackPower = knockbackPower;
        }
        

        protected virtual void DamageTarget(Collider2D collision)
        {
            Health targetHealth = collision.transform.GetComponent<Health>();
            if (!targetHealth) return;
            PerformDamage(targetHealth);
        }
        private void PerformDamage(Health target)
        {
            target.TakeDamage(damage);
            if (!haveKnockback) return;
            Transform targetTransform = target.transform;
            Vector2 direction = (transform.position - targetTransform.position).normalized;
            Knock(targetTransform,direction);
        }
        protected virtual void Knock(Transform target, Vector2 direction)
        {
            if (ComparerCalculator.InRange(-0.5f, 0.5f, direction.x))
            {
                int newXdir = direction.x < 0 ? -1 : 1;
                direction = new Vector2(newXdir, direction.y);
            }
            if (ComparerCalculator.InRange(-.2f, .2f, direction.y))
            {
                float newYdir = Random.Range(0, 2) == 0 ? -.5f : .5f;
                direction = new Vector2(direction.x, newYdir);
            }
            target.GetComponent<Rigidbody2D>().AddForce(-direction * 100f * knockbackPower);
        }
        


        public void SetKnockbackPower(float value)
        {
            knockbackPower = value;
        }
        public void SetDamage(float damageValue)
        {
            damage = damageValue;
        }
        public void ReturnDefaults()
        {
            damage = defaultTouchDamage;
            knockbackPower = defaultKnockbackPower;
        }
    }
}