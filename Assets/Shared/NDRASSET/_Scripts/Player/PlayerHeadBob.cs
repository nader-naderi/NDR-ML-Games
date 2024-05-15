using UnityEngine;

namespace NDRLiteFPS
{
    public class PlayerHeadBob : MonoBehaviour
    {
        [Header("Bobbing Parameters")]
        [SerializeField] private float bobSpeed = 10f;
        [SerializeField] private float bobAmount = 0.25f;

        private float timer;
        private Quaternion originalRotation;

        void Start()
        {
            originalRotation = transform.localRotation;
        }

        void Update()
        {
            // Simulate head bobbing when moving
            if (InputManager.IsMoving)
            {
                float waveSlice = Mathf.Sin(timer);
                timer += bobSpeed * Time.deltaTime;

                if (timer > Mathf.PI * 2)
                    timer -= Mathf.PI * 2;

                float bobValue = waveSlice * bobAmount;

                // U-shaped pattern
                if (timer <= Mathf.PI)
                    bobValue = Mathf.Abs(bobValue);
                else
                    bobValue = -Mathf.Abs(bobValue);

                Quaternion bobbingRotation = originalRotation;
                bobbingRotation *= Quaternion.Euler(new Vector3(bobValue, 0f, 0f));

                transform.localRotation = bobbingRotation;
            }
            else
            {
                // Reset rotation when not moving
                transform.localRotation = originalRotation;
                timer = 0f;
            }
        }
    }
}
