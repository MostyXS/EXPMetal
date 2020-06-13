using EXPMetal.Combat;
using EXPMetal.UI;
using EXPMetal.Utils;
using MostyProUI;
using MostyProUI.PrefsControl;
using MostyProUI.Utils;
using System.Collections;
using UnityEngine;

namespace EXPMetal.Control.AI
{
    public class KharnController : EnemyEntity
    {
        [SerializeField] GameObject[] objectsToActivateOnDeath = null;
        

        [Header("Second Stage Attributes")]
        [SerializeField] RuntimeAnimatorController secondStageAnimatorController = null;
        [SerializeField] Sprite secondStageSprite = null;
        [SerializeField] GameObject middleDialogue = null;
        [SerializeField] AudioClip secondStageMusic = null;

        [Header("Delays and attack ranges")]
        [SerializeField] float baseAttackRange = .5f;
        [SerializeField] float tentacleAttackRange = 1f;
        [SerializeField] float tentacleAttackDelay = 10f;
        [SerializeField] float yAttackRange = .2f;

        [Header("Damages and speeds")]
        [SerializeField] float jumpSpeed = 5f;
        [SerializeField] float tentacleAttackDamage = 20f;
        [Range(1f, 5f)] [SerializeField] float jumpAttackKnockbackPower = 2.5f, tentacleKnockbackPower = 2f;

        [Header("SFX")]
        [SerializeField] AudioClip jumpSFX = null;
        [SerializeField] AudioClip tentacleSFX = null;



        [Header("Jump Attack Params")]
        [SerializeField] Transform jumpHighPoint;
        [SerializeField] float animRotationDifference = 155, jumpAttackDamage = 30f, jumpAttackRange = 2f;
        [SerializeField] float jumpAttackDelay = 10f;
        [SerializeField] float jumpAttackAvoidDelay = .2f;

        float timeSinceLastJumpAttack = Mathf.Infinity, timeSinceLastTentacleAttack = Mathf.Infinity;
        bool stageChanged = false;

        Health myHealth;
        WeaponDamager fist;
        Vector2 defaultPosition;

        protected override void Awake()
        {
            base.Awake();
            myHealth = GetComponent<Health>();
            fist = GetComponentInChildren<WeaponDamager>();
            defaultPosition = transform.position;
        }

        protected override void Start()
        {
            base.Start();
            myHealth.onDeath += ChangeStage;
            UIMenu.Instance.onPause += OnPause;
        }
        private void OnEnable()
        {
            targetTransform = GameObject.FindWithTag("Player").transform;
        }

        private void OnPause()
        {
            animator.SetBool("Walking", false);
        }

        private void Update()
        {
            if (UIMenu.Paused) return;
            ProcessControl();
            Timer();
        }

        private void Timer()
        {
            timeSinceLastJumpAttack += Time.deltaTime;
            timeSinceLastTentacleAttack += Time.deltaTime;
        }

        public void ChangeStage()
        {
            StartCoroutine(StageTransition());
        }
        public void  OnDeath()
        {
            StartCoroutine(Die());
        }
        public IEnumerator Die()
        {
            while (isAttacking)
            {
                yield return null;
            }
            
            GetComponent<Collider2D>().enabled = false;
            SetAnimationBool("Death", true);
            myHealth.onDeath -= OnDeath;
            VolumeController.MusicSource.Stop();
            for(int i =0; i<objectsToActivateOnDeath.Length;i++)
            {
                objectsToActivateOnDeath[i].SetActive(true);
            }
            enabled = false;
        }
        private void ProcessControl()
        {
            if (isAttacking) return;
            EngageTarget();
            CheckDirection();
            AttackBehaviour();
        }

        private void AttackBehaviour()
        {
            if(stageChanged && timeSinceLastTentacleAttack > tentacleAttackDelay && !IsFarFromPlayer(tentacleAttackRange) && AtRightHeightWithPlayer(yAttackRange))
            {
                PerformAttackWithDelay(TentacleAttack());
            }
            if (timeSinceLastJumpAttack > jumpAttackDelay && IsFarFromPlayer(jumpAttackRange))
            {
                PerformAttackWithDelay(JumpAttack());
            }
            else if (!IsFarFromPlayer(baseAttackRange) && AtRightHeightWithPlayer(yAttackRange))
            {
                PerformAttackWithDelay(BaseAttack());
            }
          
        }

        private IEnumerator TentacleAttack()
        {
            fist.SetKnockbackPower(tentacleKnockbackPower);
            fist.SetDamage(tentacleAttackDamage);
            mainAudio.PlayOneShot(tentacleSFX, charactersVolume);
            SetAnimationTrigger("TentacleAttack");
            yield return WaitForCurrentAnimation();
            fist.ReturnDefaults();
            timeSinceLastTentacleAttack = 0;
        }

        private IEnumerator StageTransition()
        {
            while (isAttacking)
            {
                yield return null;
            }
            VolumeController.MusicSource.Stop();
            animator.SetBool("Walking", false);
            middleDialogue.SetActive(true);
            while (middleDialogue)
            {
                yield return null;
            }

            UIMenu.Instance.Pause(false);
            animator.SetTrigger("ChangeStage");

            yield return WaitForCurrentAnimation();
            UpdateStageValues();
            UIMenu.Instance.Resume();
            VolumeController.MusicSource.Play();
            stageChanged = true;
        }

        private void UpdateStageValues()
        {
            VolumeController.MusicSource.clip = secondStageMusic;
            myHealth.SetCurrentHealth(myHealth.GetMaxHealth());
            GetComponentInChildren<SpriteRenderer>().sprite = secondStageSprite;
            animator.runtimeAnimatorController = secondStageAnimatorController;
            myHealth.onDeath -= ChangeStage;
            myHealth.onDeath += OnDeath;
        }

        private IEnumerator BaseAttack()
        {
            SetAnimationTrigger("BaseAttack");
            yield return WaitForCurrentAnimation();
        }
        private void EngageTarget()
        {
            animator.SetBool("Walking", true);
            Vector2 moveToPosition = new Vector2(movementSpeed * Time.deltaTime, 0);
            transform.position += (Vector3)moveToPosition * Direction;
        }
        private IEnumerator JumpAttack()
        {
            yield return StartCoroutine(JumpAttackStart());
            yield return StartCoroutine(JumpAttackMain());
            yield return StartCoroutine(JumpAttackEnd());
        }

        private IEnumerator JumpAttackStart()
        {
            SetAnimationBool("Jumping", true);
            yield return WaitForCurrentAnimation();
            mainAudio.PlayOneShot(jumpSFX, charactersVolume);
            bodyDamager.SetDamage(jumpAttackDamage);
            bodyDamager.SetKnockbackPower(jumpAttackKnockbackPower);
        }
        private IEnumerator JumpAttackMain()
        {
            
            float rotationZ;
            ComparerCalculator.CalculateRotation(transform.position, jumpHighPoint.position, animRotationDifference, out rotationZ, Direction);
            transform.rotation = Quaternion.Euler(0, 0, rotationZ);
            while ((Vector2)transform.position != (Vector2)jumpHighPoint.position)
            {
                transform.position = Vector2.MoveTowards(transform.position, jumpHighPoint.position, jumpSpeed * Time.deltaTime);
                yield return null;
            }

            CheckDirection();
            Vector2 targetPoint = new Vector2(targetTransform.position.x, defaultPosition.y);
            ComparerCalculator.CalculateRotation(transform.position, targetPoint, animRotationDifference, out rotationZ, Direction);
            transform.rotation = Quaternion.Euler(0, 0, rotationZ);

            yield return new WaitForSeconds(jumpAttackAvoidDelay);
            mainAudio.PlayOneShot(jumpSFX, charactersVolume);
            while ((Vector2)transform.position != targetPoint)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPoint, jumpSpeed * Time.deltaTime);
                yield return null;
            }
        }
        private IEnumerator JumpAttackEnd()
        {

            transform.rotation = Quaternion.identity;
            animator.SetTrigger("NextJumpStage");
            yield return WaitForCurrentAnimation();
            SetAnimationBool("Jumping", false);
            bodyDamager.ReturnDefaults();
            timeSinceLastJumpAttack = 0f;
        }
        protected override int LookDirection()
        {
            return transform.position.x > targetTransform.transform.position.x ? -1 : 1; // 1 looking to left/ -1 looking to right
        }
    }
}