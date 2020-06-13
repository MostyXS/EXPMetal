using EXPMetal.Control.AI;
using EXPMetal.Saving;
using EXPMetal.UI;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace EXPMetal.Utils
{
    public class BombScriptOnSecondBoss : MonoBehaviour, ISaveable
    {
        [SerializeField] KharnController kharnController;
        [SerializeField] PlayerHealthDisplay healthBar;
        [SerializeField] GameObject activePlayer, sword, buttonTip, bossHealthBar;
        [SerializeField] AudioSource musicSource;

        bool activated = false;
        private void Update()
        {
            if (CrossPlatformInputManager.GetButton("Use"))
            {
                GetComponent<Animator>().SetTrigger("TakeSword");
            }
        }
        private void StartGame()
        {
            musicSource.Play();
            activePlayer.SetActive(true);
            
            bossHealthBar.SetActive(true);
            healthBar.enabled = true;
            Destroy(sword);
            Destroy(buttonTip);
            Destroy(gameObject);
            kharnController.enabled = true;
            activated = true;
        }

        public object CaptureState()
        {
            return activated;
        }

        public void RestoreState(object state)
        {
            if (!(bool)state || activated) return;
            StartGame();
        }
    }
}