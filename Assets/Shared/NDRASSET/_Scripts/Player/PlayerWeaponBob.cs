using UnityEngine;

namespace NDRLiteFPS
{
    public class PlayerWeaponBob : MonoBehaviour
    {
        [Header("Bobbing Speeds")]
        [SerializeField] private float idleBobSpeed = 2f;
        [SerializeField] private float walkBobSpeed = 4f;
        [SerializeField] private float runBobSpeed = 6f;

        [Header("Bobbing Parameters")]
        [SerializeField] private float bobSpeed = 1f;
        [SerializeField] private float bobDistance = 0.005f;
        [SerializeField] private float maxRandomOffset = 0.02f;

        [Header("Gun Transform")]
        [SerializeField] private Transform gunTransform;

        private float horizontal, vertical, timer, waveSlice;
        private Vector3 midPoint;
        private Vector3 targetPosition;

        private void Start()
        {
            InitializeGunBob();
        }

        private void Update()
        {
            if (gunTransform == null)
                return;

            UpdateInputValues();
            UpdateBobbing();
        }

        private void InitializeGunBob()
        {
            if (gunTransform == null)
            {
                Debug.LogError("Gun Transform not assigned in GunBob script!");
                enabled = false;
                return;
            }

            midPoint = gunTransform.localPosition;
        }

        private void UpdateInputValues()
        {
            horizontal = InputManager.Horizontal;
            vertical = InputManager.Vertical;
        }

        private void UpdateBobbing()
        {
            targetPosition = gunTransform.localPosition;

            CalculateWaveSlice();

            if (IsIdle())
                targetPosition = UpdateBobPosition(targetPosition, idleBobSpeed);
            else
            {
                float currentBobSpeed = InputManager.IsRunning ? runBobSpeed : walkBobSpeed;
                targetPosition = UpdateBobPosition(targetPosition, currentBobSpeed);
            }

            gunTransform.localPosition = targetPosition;
        }

        private void CalculateWaveSlice()
        {
            waveSlice = Mathf.Sin(timer);
            timer += bobSpeed * Time.deltaTime;

            if (timer > Mathf.PI * 2)
                timer -= (Mathf.PI * 2);
        }

        private bool IsIdle() => Mathf.Approximately(horizontal, 0f) && Mathf.Approximately(vertical, 0f);

        private Vector3 UpdateBobPosition(Vector3 localPosition, float currentBobSpeed)
        {
            if (waveSlice != 0)
            {
                float translateChange = waveSlice * bobDistance;
                float totalAxes = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
                translateChange *= totalAxes * 2;

                // Introduce smoother randomness using Perlin noise
                float randomOffsetX = Mathf.PerlinNoise(Time.time * maxRandomOffset, 0) * 2 - 1; // Map the Perlin noise to the range [-1, 1]
                float randomOffsetY = Mathf.PerlinNoise(0, Time.time * maxRandomOffset) * 2 - 1;

                localPosition.y = midPoint.y + translateChange + randomOffsetY * maxRandomOffset;
                localPosition.x = midPoint.x + translateChange * 2 + randomOffsetX * maxRandomOffset;
            }
            else
            {
                localPosition.y = midPoint.y;
                localPosition.x = midPoint.x;
            }

            bobSpeed = Mathf.Lerp(bobSpeed, currentBobSpeed, Time.deltaTime * 5f);
            return localPosition;
        }
    }
}
