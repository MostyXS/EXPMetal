using EXPMetal.Combat;
using MostyProUI;
using MostyProUI.PrefsControl;
using MostyProUI.Utils;
using System.Collections;
using UnityEngine;

namespace EXPMetal.Control.AI
{
    public class CyporgController : EnemyEntity
    { 
        [Range(0.01f, 10f)] [SerializeField] float baseAttackRange, flamethrowerRange, yAttackRange, dashStopRange;
        [Header("Attack durations and delays")]
        [SerializeField] float dashSpeed;

        [SerializeField] float flamethrowerReloadTime, groundAttackReloadTime, dashReloadTime;

        [Header("Ground attack params")]
        [SerializeField] float attackDamage;
        [SerializeField] float knockbackStrength;
        [Tooltip("Difference between player and boss positions")]
        [SerializeField] float yPositionDifference, xPositionDifference;
        
        [Tooltip("shaking - how strong boss shakes while charging. Avoid time - tme for player to react")]

        [SerializeField] float shaking = 0.01f, chargingTime=2f, avoidTime;
        [SerializeField]float positionError = .5f;
        [Header("Sounds")]
        [SerializeField] AudioClip flamethrowerSFX = null;
        
        float timeSinceLastFlamethrowerAttack = Mathf.Infinity, timeSinceLastGroundAttack = 0 , timeSinceLastDash = Mathf.Infinity;
        

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
            GetComponent<Health>().onDeath += TriggerNextScene;
        }
       
        private void Update()
        {
            if (UIMenu.Paused) return;
            
            Timer();
            ProcessControl();
        }
        private void TriggerNextScene()
        {
            SceneSwitcher.Instance.LoadNextSceneImmediate();
        }
        private void Timer()
        {
            timeSinceLastDash += Time.deltaTime;
            timeSinceLastFlamethrowerAttack += Time.deltaTime;
            timeSinceLastGroundAttack+= Time.deltaTime;
        }
        private void ProcessControl()
        {

            if (isAttacking) return;
            CheckDirection();
            EngageTarget();
            AttackBehaviour();
        }

        private void AttackBehaviour()
        {
            if (timeSinceLastGroundAttack >= groundAttackReloadTime)
            {
                PerformAttackWithDelay(GroundAttack());
            }
            else if (timeSinceLastFlamethrowerAttack >= flamethrowerReloadTime && AtRightHeightWithPlayer(yAttackRange) && !IsFarFromPlayer(flamethrowerRange))
            {
                PerformAttackWithDelay(FlamethrowerAttack());
            }
            else if (AtRightHeightWithPlayer(yAttackRange) && !IsFarFromPlayer(baseAttackRange))
            {
                PerformAttackWithDelay(BaseAttack());
            }
            else if (timeSinceLastDash >= dashReloadTime && IsFarFromPlayer(dashStopRange))
            {
                PerformAttackWithDelay(Dash());
            }
        }

        private IEnumerator BaseAttack()
        {
            animator.SetTrigger("BaseAttack");
            yield return WaitForCurrentAnimation();
        }
        private IEnumerator Dash()
        {
            animator.SetBool("Dashing", true);  
            bodyDamager.SetStepBack(false);
            Vector2 targetPosition;
            DeclareDashPositions(out targetPosition);
            while ((Vector2)transform.position != targetPosition)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, dashSpeed * Time.deltaTime);
                yield return null;  
            }
           
            bodyDamager.SetStepBack(true);
            animator.SetBool("Dashing", false);
            timeSinceLastDash = 0;
        }
        private void DeclareDashPositions(out Vector2 targetPosition)
        {
            float error = transform.position.y >= targetTransform.position.y + positionError ? positionError : 0;
            targetPosition = new Vector2(transform.position.x + (dashStopRange - positionError) * -Direction, targetTransform.transform.position.y + error);
        }
            
        private IEnumerator GroundAttack()
        {
            yield return StartCoroutine(GroundAttackStart());
            SetGroundAttackParams();
            yield return new WaitForSeconds(avoidTime);
            yield return StartCoroutine(GroundAttackMain());
            EndGroundAttack();
        }

        private void SetGroundAttackParams()
        {
            bodyDamager.SetDamage(attackDamage);
            bodyDamager.SetKnockbackPower(knockbackStrength);
            bodyDamager.SetStepBack(false);
        }

        private IEnumerator GroundAttackStart()
        {
            Vector2 startPosition;
            DeclareStart(out startPosition);
            while ((Vector2)transform.position != startPosition)
            {
                transform.position = Vector2.MoveTowards(transform.position, startPosition, dashSpeed * Time.deltaTime);
                yield return null;
            }
            float timeSinceCharging = 0;
            while (timeSinceCharging < chargingTime)
            {
                DeclareStart(out startPosition);
                transform.position = new Vector2(Random.Range(startPosition.x - shaking, startPosition.x + shaking),
                    Random.Range(transform.position.y - shaking, transform.position.y + shaking));
                timeSinceCharging += Time.deltaTime;
                yield return null;
            }
           
        }
        private void DeclareStart(out Vector2 start)
        {
            int position = (transform.position.x > targetTransform.transform.position.x ? 1 : -1); // 1 right from player  // -1 left from player
            start = new Vector2(targetTransform.transform.position.x + xPositionDifference * position, targetTransform.transform.position.y + yPositionDifference);
        }
        
        private IEnumerator GroundAttackMain()
        {
            Vector2 targetPosition, endingPosition;

            DeclareGroundAttackTargets(out targetPosition, out endingPosition);
            DeclareDirection();
            animator.SetBool("Dashing", true);
            while ((Vector2)transform.position != targetPosition)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, dashSpeed * Time.deltaTime);
                yield return null;
            }
            transform.eulerAngles = new Vector3(0, 0, 0);
            while ((Vector2)transform.position != endingPosition)
            {
                transform.position = Vector2.MoveTowards(transform.position, endingPosition, dashSpeed * Time.deltaTime);
                yield return null;
            }
        }
        private void DeclareGroundAttackTargets(out Vector2 targetPosition, out Vector2 endingPosition)
        {
            targetPosition = new Vector2(targetTransform.transform.position.x, targetTransform.transform.position.y - 0.2f); //0.2f - deviation from real ground position
            
            endingPosition = new Vector2(targetTransform.transform.position.x - xPositionDifference* Direction, targetTransform.transform.position.y + yPositionDifference);
        }
        private void DeclareDirection()
        { 
            transform.eulerAngles = new Vector3(0, 0, 60f * Direction);
        }
        private void EndGroundAttack()
        {
            bodyDamager.ReturnDefaults();
            bodyDamager.SetStepBack(true);
            timeSinceLastGroundAttack = Random.Range(-5,5);
            animator.SetBool("Dashing", false);
            
        }
        private IEnumerator FlamethrowerAttack()
        {
            
            mainAudio.PlayOneShot(flamethrowerSFX, charactersVolume);
            SetAnimationTrigger("Flamethrower");
            yield return WaitForCurrentAnimation();
            timeSinceLastFlamethrowerAttack = Random.Range(-3, 3);
        }
        private void EngageTarget()
        {
            animator.SetBool("Flying", true);
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, targetTransform.transform.position.y+.1f), movementSpeed * Time.deltaTime);
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(targetTransform.transform.position.x, transform.position.y), movementSpeed * Time.deltaTime);
        }
    }
}
