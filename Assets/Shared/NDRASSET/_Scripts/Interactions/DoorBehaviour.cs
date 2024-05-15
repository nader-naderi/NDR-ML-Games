using UnityEngine;
using System.Collections;
using NDR;

namespace NDRLiteFPS
{
    public class DoorBehaviour : Interactable
    {
        [SerializeField] Vector3 openPos;
        [SerializeField] Vector3 closePos;
        [SerializeField] float interactionSpeed = 2f;
        [SerializeField] AudioClip closeClip;
        [SerializeField] Animator anim;

        AudioClip openClip;

        bool isOpen = false;
        private void Start()
        {
            openClip = interactionClip;
        }
        public override void Interact()
        {
           

            isOpen = !isOpen;
            //anim.SetBool("isOpen", isOpen);
            //anim.SetTrigger("interact");
            if (isOpen)
            {
                interactionClip = openClip;

                StartCoroutine(NDRUtility.LerpLocalEulerAngles(transform, openPos, interactionSpeed * Time.deltaTime));
            }
            else
            {
                openClip = interactionClip;
                interactionClip = closeClip;

                StartCoroutine(NDRUtility.LerpLocalEulerAngles(transform, closePos, interactionSpeed * Time.deltaTime));
            }


            base.Interact();
        }

    }
}