using EXPMetal.Combat;
using MostyProUI;
using UnityEngine;
using UnityEngine.UI;

namespace EXPMetal.UI
{
    public class PlayerHealthDisplay : MonoBehaviour
    {
        [Header("Disable script if you don't need it")]
        [SerializeField] GameObject healthBar;
        float maximumhealth;
        Image tempHealthBar;

        Health playerHealth;

        private void Awake()
        {
            if (!enabled) return;
            if (healthBar == null)
            {
                Debug.LogError("health bar is null");
                return;
            }
            
            
        }

        private void Start()
        {
            InstantiateHealthBar();
            playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
            maximumhealth = playerHealth.GetMaxHealth();
            playerHealth.onDamageTaken += UpdateHealth;
            UpdateHealth();

        }

        private void InstantiateHealthBar()
        {
            Transform tempHPImage = Instantiate(healthBar, MainCanvas.Transform).transform;
            tempHealthBar = tempHPImage.GetChild(tempHPImage.childCount - 1).GetComponent<Image>();
          
        }
        private void UpdateHealth()
        {
            tempHealthBar.fillAmount = playerHealth.GetCurrentHealth() / maximumhealth ;
        }

        
    }
}
