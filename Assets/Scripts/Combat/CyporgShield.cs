using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EXPMetal.Combat
{
    public class CyporgShield : MonoBehaviour
    {
        [SerializeField] float maxShieldHealth;
        [SerializeField] GameObject shield = null;
        [SerializeField] Image shieldBar = null;
        [SerializeField] float rechargeTime = 30f;
        [SerializeField] Vector2 shieldSize = new Vector2(0.2424471f, 0.3133706f);

        float currentHelath;
        Health health;

        CapsuleCollider2D myCollider;

        Vector2 defaultColliderSize;
        

        private void Awake()
        {
            health = GetComponent<Health>();
            currentHelath = maxShieldHealth;
            myCollider = GetComponent<CapsuleCollider2D>();
            defaultColliderSize = myCollider.size;
            myCollider.size = shieldSize;

        }

        private void Start()
        {
            health.SetInvicibility(true);
            health.shieldDamageEvent += TakeDamage;
        }

        public void TakeDamage(float damageValue)
        {
            currentHelath -= damageValue;
            shieldBar.fillAmount = currentHelath / maxShieldHealth;
            if (currentHelath > 0) return;
            StartCoroutine(ShieldRecharge());
        }

        private IEnumerator ShieldRecharge()
        {
            DeactivateShield();
            yield return new WaitForSeconds(rechargeTime);
            ActivateShield();
        }

        private void ActivateShield()
        {
            health.shieldDamageEvent += TakeDamage;
            health.SetInvicibility(true);
            currentHelath = maxShieldHealth;
            shield.SetActive(true);
            shieldBar.fillAmount = currentHelath / maxShieldHealth;
            myCollider.size = shieldSize;
        }

        private void DeactivateShield()
        {
            health.shieldDamageEvent -= TakeDamage;
            myCollider.size = defaultColliderSize;
            shield.SetActive(false);
            health.SetInvicibility(false);
        }
    }
}