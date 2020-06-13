using EXPMetal.Combat;
using UnityEngine;

namespace EXPMetal
{
    public class ContiniousDamage : MonoBehaviour
    {
        [SerializeField] float damage = 1f;
        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<Health>().TakeContiniousDamage(damage * Time.deltaTime);
            }
        }
    }
}
