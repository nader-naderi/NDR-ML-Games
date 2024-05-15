using UnityEngine;
namespace NDRLiteFPS
{
    public class PlayerInteraction : MonoBehaviour
    {
        [SerializeField] private float rayLength = 10f;
        [SerializeField] LayerMask layerMask;
        private GameObject raycastedObject;
        private UIManager uiManager;
        bool isCrosshairActivated = false;

        private void Start()
        {
            // Create UIManager.
            if (!UIManager.instance)
                Instantiate(Resources.Load("InGameUI"));

            uiManager = UIManager.instance;
        }

        private void LateUpdate()
        {
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, rayLength, layerMask.value) && 
                hit.collider.CompareTag("Interactable"))
                ShowInteraction(hit);
            else
                HideInteraction();
        }

        private void ShowInteraction(RaycastHit hit)
        {
            raycastedObject = hit.collider.gameObject;
            Interactable interactable = raycastedObject.GetComponent<Interactable>();
            isCrosshairActivated = true;

            // Activate UI
            uiManager.SetInteraction(interactable.InteractionMessage, true);

            if (Input.GetKeyDown(KeyCode.E))
                InteractWithObject(interactable);
        }

        private void HideInteraction()
        {
            isCrosshairActivated = false;
            uiManager.SetInteraction("", false);
            raycastedObject = null;
        }

        private void InteractWithObject(Interactable interactable)
        {
            interactable.Interact();

            if (interactable.GetComponent<Pickupable>())
                uiManager.SetInteraction("", false);
        }
    }
}