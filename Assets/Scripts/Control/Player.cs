using EXPMetal.Combat;
using MostyProUI;
using MostyProUI.Audio;
using MostyProUI.PrefsControl;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace EXPMetal.Control
{
    public class Player : CharacterEntity
    {
       
        [Header("Main Parametrs")]
        [SerializeField] float movementSpeed = 1f;
        [SerializeField] float jumpHeight = 5f;
        [SerializeField] float distanceToGround = 0.5f;

        [Header("Additional Parametrs")]
        [Range(0.1f, 5f)] [SerializeField] float attackDelay;
        
        [SerializeField] bool isPassive = false;
        [Header("SFX")]
        [SerializeField] AudioClip jumpSound = null;
        [SerializeField] AudioClip attackSound = null;
       
        bool isAttacking = false;
        bool canJump = true; //Used for preventing double jumps
        
        
        Rigidbody2D rigid;

        protected override void Awake()
        {
            base.Awake();
            rigid = GetComponent<Rigidbody2D>();
            
        }
        protected override void Start()
        {
            base.Start();
            GetComponent<Health>().onDeath += Stop;
            UIMenu.Instance.onPause += OnPause;
        }
        private void OnPause()
        {
            animator.SetFloat("Speed", 0f);
            if (!walkingSource) return;
            walkingSource.Stop();
        }

        void Update()
        {
            if (UIMenu.Paused) return;
            ProcessControl();
        }
        public void Stop()
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            animator.SetFloat("Speed", 0f);
        }

        private void Attack()
        {
            if (isPassive) return;
            if (CrossPlatformInputManager.GetButtonDown("Skip") && !isAttacking)
            {
                if (attackSound)
                    mainAudio.PlayOneShot(attackSound, PrefsController.CharactersVolume);
                animator.SetTrigger("Attack");
                StartCoroutine(AttackDelay());
            }
        }

        private IEnumerator AttackDelay()
        {
            isAttacking = true;
            yield return new WaitForSeconds(attackDelay);
            isAttacking = false;
        }
       

        private void ProcessControl()
        {
            MoveSoundCheck();

            Move();
            Jump();
            Attack();
        }
        private void Move()
        {
            float axisRaw = CrossPlatformInputManager.GetAxisRaw("Horizontal");
            animator.SetFloat("Speed", Mathf.Abs(axisRaw));
            float deltaX = axisRaw * Time.deltaTime * movementSpeed;
            transform.position = new Vector2(transform.position.x + deltaX, transform.position.y);
            DirectionCheck(axisRaw);
            
        }
        private void DirectionCheck(float axis)
        {
            if (Mathf.Approximately(axis, 0)) return;
            transform.localScale = new Vector2(defaultXScale * axis, transform.localScale.y);
                
        }
        private void MoveSoundCheck()
        {
            if (!walkingSource) return;

            if (CrossPlatformInputManager.GetButton("Horizontal") && IsGrounded())
            {
                if(!walkingSource.isPlaying)
                    walkingSource.Play();
            }
            else
            {
                walkingSource.Stop();
            }
        }

        private IEnumerator JumpDelay()
        {
            canJump = false;
            yield return new WaitForSeconds(.1f);
            canJump = true;
        }
        private void Jump()
        {
            if (!CrossPlatformInputManager.GetButtonDown("Jump") || !IsGrounded()) return;
            if(jumpSound)
                mainAudio.PlayOneShot(jumpSound);
            StartCoroutine(JumpDelay());
            animator.SetTrigger("Jump");
            rigid.velocity = new Vector2(0, jumpHeight); 
        }

        private bool IsGrounded()
        {
            if (!canJump) return false;
            RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.down, distanceToGround, 1 << 11);
            return (hitInfo.collider);
        }

        public float GetSpeed() //used by follower
        {
            return movementSpeed;
        }
    }
}
