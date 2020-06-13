using EXPMetal.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace EXPMetal.UI
{
    public class EnemyHealthBar : MonoBehaviour
    {
        [SerializeField] Image healthBar;
        float maxHealth;
        Health health;

        private void Awake()
        {
            health = GetComponent<Health>();
        }

        private void Start()
        {
            maxHealth = health.GetMaxHealth();
            health.onDamageTaken += UpdateHealth;
            UpdateHealth();
        }
        private void UpdateHealth()
        {
            healthBar.fillAmount = health.GetCurrentHealth() / maxHealth;
        }
    }
}