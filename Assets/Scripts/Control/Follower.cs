using UnityEditor;
using UnityEngine;

namespace EXPMetal.Control.AI
{
    public class Follower : CharacterEntity
    {
        [Range(1f, 3f)] [SerializeField] float followRange = 1f;
        [SerializeField] float scareDistance = 2f;
        [Tooltip("Minimum distance between player and follower")] [SerializeField] float stopDistance = 1f;

        float movementSpeed;
        
        GameObject targetToFollow;
        Animator anim;
        private void Awake()
        {
            targetToFollow = GameObject.FindWithTag("Player");
            anim = GetComponent<Animator>();
        }
        void Start()
        {
            movementSpeed = targetToFollow.GetComponent<Player>().GetSpeed() - .1f;
        }
        
        private void Update()
        { 
            if (IsScared())
            {
                anim.SetBool("Walking", false);
                return;
            }
            ProcessControl();
        }

        private void ProcessControl()
        {

            if (CloseToPlayer())
            {
                anim.SetBool("Walking", false);
                return;
            }
            FollowPlayer();
        }

        

        private bool CloseToPlayer()
        {

            if (targetToFollow.transform.position.x > transform.position.x)
            {
                transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
                return targetToFollow.transform.position.x - stopDistance < transform.position.x;
            }
            else
            {
                transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
                return targetToFollow.transform.position.x + stopDistance > transform.position.x;
            }
        }

        private bool IsEnemiesNearby()
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, scareDistance, Vector2.zero);
            for (int i = 0; i < hits.Length; i++)
            {
                var enemy = hits[i].transform.GetComponent<EnemyAI>();
                if (!enemy) continue;
                return true;
            }
            return false;
        }
        private bool IsScared()
        {
            if (NoPlayerInFollowRange() || IsEnemiesNearby())
            {
                anim.SetBool("Scared", true);
                return true;
            }
            anim.SetBool("Scared", false);
            return false;


        }
        private bool NoPlayerInFollowRange()
        {
            return (targetToFollow.transform.position - transform.position).sqrMagnitude > followRange*followRange;
        }

        private void FollowPlayer()
        {
            anim.SetBool("Walking", true);
            transform.position = Vector2.MoveTowards(transform.position, targetToFollow.transform.position,
                movementSpeed * Time.deltaTime);
        }
    }
}