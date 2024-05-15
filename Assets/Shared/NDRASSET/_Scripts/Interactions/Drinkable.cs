using UnityEngine;
using System.Collections;

namespace NDRLiteFPS
{
    public class Drinkable : Pickupable
    {
        [SerializeField] float amount;
        public override void Interact()
        {
            base.Interact();
            AudioManager.instance.PlayDrinkingSound();
        }
    }
}