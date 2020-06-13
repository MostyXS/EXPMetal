using MostyProUI.Audio;
using MostyProUI.PrefsControl;
using UnityEngine;

namespace EXPMetal.Combat
{
    public class WeaponDamager : TouchDamager
    {
        [SerializeField] AudioClip hitSFX;

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag(tag)) return;

            DamageTarget(collider);
        }
        protected override void DamageTarget(Collider2D collision)
        {
            base.DamageTarget(collision);
            if (!hitSFX) return;
            MainAudioSource.Instance.value.PlayOneShot(hitSFX, PrefsController.CharactersVolume );
        }
    }
}