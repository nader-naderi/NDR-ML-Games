﻿using UnityEngine;
using System.Collections;

namespace NDRLiteFPS
{
    public class GunBob : MonoBehaviour
    {
        [SerializeField] private float idleBobSpeed = 0.02f;
        [SerializeField] private float walkBobSpeed = 0.05f;
        [SerializeField] private float runBobSpeed = 0.075f;


        [SerializeField]
        private float bobSpeed = 1f;
        [SerializeField]
        private float bobDistance = 1f;
        [SerializeField]
        private Transform Gun;

        private float horizontal, vertical, timer, waveSlice;
        private Vector3 midPoint;


        void Start()
        {
            midPoint = Gun.localPosition;
        }

        void Update()
        {
            if (!Gun) return;

            horizontal = InputManager.Horizontal;
            vertical = InputManager.Vertical;

            Vector3 localPosition = Gun.localPosition;

            waveSlice = Mathf.Sin(timer);
            timer = timer + bobSpeed;
            if (timer > Mathf.PI * 2)
            {
                timer = timer - (Mathf.PI * 2);
            }

            if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
            {
                bobSpeed = Mathf.Lerp(bobSpeed, idleBobSpeed, Time.deltaTime * 5f);

                if (waveSlice != 0)
                {
                    float translateChange = waveSlice * bobDistance;
                    float totalAxes = 0.1f + 0.1f;
                    totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
                    translateChange = totalAxes * translateChange;
                    localPosition.y = midPoint.y + translateChange;
                    localPosition.x = midPoint.x + translateChange * 2;
                }
                else
                {
                    localPosition.y = midPoint.y;
                    localPosition.x = midPoint.x;
                }

            }
            else
            {
                if (InputManager.IsRunning)
                    bobSpeed = Mathf.Lerp(bobSpeed, runBobSpeed, Time.deltaTime * 5f);
                else
                    bobSpeed = Mathf.Lerp(bobSpeed, walkBobSpeed, Time.deltaTime * 5f);

                if (waveSlice != 0)
                {
                    float translateChange = waveSlice * bobDistance;
                    float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
                    totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
                    translateChange = totalAxes * translateChange;
                    localPosition.y = midPoint.y + translateChange;
                    localPosition.x = midPoint.x + translateChange * 2;
                }
                else
                {
                    localPosition.y = midPoint.y;
                    localPosition.x = midPoint.x;
                }
            }

            Gun.localPosition = localPosition;
        }

    }
}