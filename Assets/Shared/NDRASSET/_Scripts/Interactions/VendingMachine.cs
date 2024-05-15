using UnityEngine;
using System.Collections;

namespace NDRLiteFPS
{
    public class VendingMachine : Interactable
    {
        [SerializeField] Transform spawnPoint;
        [SerializeField] float delayOnOrder = 2f;
        [SerializeField] AudioClip interactClip;
        [SerializeField] GameObject[] products;

        public override void Interact()
        {
            base.Interact();
            StartCoroutine(DelayTheInteraction());
        }

        IEnumerator DelayTheInteraction()
        {
            AudioManager.instance.PlayGenericSound(interactClip);
            yield return new WaitForSeconds(delayOnOrder);
            Instantiate(products[Random.Range(0, products.Length)], spawnPoint.position, Quaternion.identity);
        }
    }
}