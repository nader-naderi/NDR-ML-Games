using UnityEngine;
using System.Collections;

namespace NDRLiteFPS
{
    public class Pickupable : Interactable
    {

        [SerializeField] protected bool destroyAfterPickup = true;

        protected Collider target;

        public override void Interact()
        {
            base.Interact();
            HandleDestroy();
            if (!target) return;
        }

        private void HandleDestroy()
        {
            if (!destroyAfterPickup) return;

            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            target = other;
            Debug.Log("Trigger");
            Interact();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            target = null;
        }

        private void OnCollisionEnter(Collision collision)
        {

            if (!collision.collider.CompareTag("Player")) return;
            target = collision.collider;
            Debug.Log("Coillision");
            Interact();
        }

        private void OnCollisionExit(Collision collision)
        {
            if (!collision.collider.CompareTag("Player")) return;
            target = null;
        }
    }
}