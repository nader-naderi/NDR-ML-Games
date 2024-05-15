using UnityEngine;
using System.Collections;

namespace NDRLiteFPS
{
    public class Interactable : MonoBehaviour
    {
        [SerializeField] protected string interactionMessage = "Press 'E' to Interact";
        [SerializeField] protected AudioClip interactionClip;

        public string InteractionMessage { get => interactionMessage; }

        public virtual void Interact()
        {
            if (interactionClip)
                OnEvent_PlaySFX();
        }

        protected void OnEvent_PlaySFX() => AudioManager.instance.PlayInteractSound(interactionClip);

    }
}