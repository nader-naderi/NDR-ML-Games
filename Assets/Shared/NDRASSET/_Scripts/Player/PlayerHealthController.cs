using UnityEngine;
using System.Collections;

namespace NDRLiteFPS
{
    public class PlayerHealthController : MonoBehaviour
    {
        [SerializeField] float maxHealth = 100;
        float currentHealth = 0;

        private void Start()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
        }

        public void AddHealth(float amount)
        {
            currentHealth += amount;
            
        }
    }
}