using EXPMetal.Saving;
using MostyProUI;
using MostyProUI.Audio;
using System;
using System.Collections;
using UnityEngine;


namespace EXPMetal.Combat
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] bool haveSelfDeath = false;
        [SerializeField] float fullHealth = 100f, timeUntilDeath = 3f;
        [SerializeField] float invicibilityTime = .3f;
        [SerializeField] AudioClip deathSound;

         public delegate void damageShield(float damage);
        public event damageShield shieldDamageEvent;
        public Action onDamageTaken;

        public Action onDeath;

        float health;
        bool isAlive = true;
        bool invicible = false;
        
        Animator anim;
        AudioSource mainAudio;
        SpriteRenderer sprite;


        private void Awake()
        {
            anim = GetComponent<Animator>();
            health = fullHealth;
            sprite = GetComponentInChildren<SpriteRenderer>();
        }
        private void Start()
        {
            mainAudio = MainAudioSource.Instance.value;
        }
        public void TakeContiniousDamage(float damage)
        {
            if (!isAlive) return;

            if (invicible)
            {
                if (shieldDamageEvent != null)
                    shieldDamageEvent(damage);
                return;
            }
            health -= damage;
            if (onDamageTaken != null)
                onDamageTaken();
            TryDeath();

        }
        public void TakeDamage(float damage)
        {
            if (!isAlive) return;

            if (invicible)
            {
                if (shieldDamageEvent != null)
                    shieldDamageEvent(damage);
                return;
            }
            HitEvent(damage);
            TryDeath();
        }

        private void TryDeath()
        {
            if (health > 0) return;
            if (onDeath != null)
                onDeath();
            if (haveSelfDeath) return;
            Death();
        }

        private void HitEvent(float damage)
        {
            anim.SetTrigger("Hitted");
            StartInvicibility();
            health -= damage;
            if (onDamageTaken != null)
                onDamageTaken();
        }

        private void StartInvicibility()
        {
            if (CompareTag("Player"))
                StartCoroutine(MakePlayerInvicible());
            else
                StartCoroutine(MakeInvicible());
        }

        public void Death()
        {
            if (!isAlive) return;

            isAlive = false;
            
            if (deathSound)
                mainAudio.PlayOneShot(deathSound);
            anim.SetBool("Death", true);
            if (CompareTag("Player"))
            {
                UIMenu.Instance.ActivateDeadMenu();
                return;
            }
            StartCoroutine(DieDelayed());
        }
        private IEnumerator DieDelayed()
        {
            yield return new WaitForSeconds(timeUntilDeath);
            gameObject.SetActive(false);
        }

        private IEnumerator MakeInvicible()
        {
            invicible = true;
            yield return new WaitForSeconds(invicibilityTime);
            invicible = false;
        }

        private IEnumerator MakePlayerInvicible()
        {
            gameObject.layer = 18;
            invicible = true;
            float timeSinceInvicible = 0f;
            while(timeSinceInvicible < invicibilityTime)
            {
                sprite.enabled = !sprite.enabled;
                timeSinceInvicible += Time.deltaTime;
                yield return .05f;
            }
            sprite.enabled = true;
            invicible = false;
            gameObject.layer = 8;
        }
        public float GetMaxHealth()
        {
            return fullHealth;
        }
        public float GetCurrentHealth()
        {
            return health;
        }
        public void SetInvicibility(bool value)
        {
            invicible = value;
        }

        public object CaptureState()
        {
            return health;
        }
        public void RestoreState(object state)
        {
            health = (float)state;  
            if (health <= 0)
            {
                isAlive = false;
                gameObject.SetActive(false);
            }
            if (onDamageTaken != null)
                onDamageTaken();
        }
        public void SetCurrentHealth(float health)
        {
            this.health = health;
            if(onDamageTaken!=null)
                onDamageTaken();
        }
    }
}
