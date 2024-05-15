using UnityEngine;
using System.Collections;

namespace NDRLiteFPS
{
    public class InputManager : MonoBehaviour
    {
        public static float Horizontal;
        public static float Vertical;
        public static float HorizontalRaw;
        public static float VerticalRaw;
        public static float MouseX;
        public static float MouseY;

        public static bool IsAiming;
        public static bool IsFireClickDown;
        public static bool IsFireHolding;
        public static bool IsFireTypeClicked;
        public static bool IsSprinting;
        public static bool IsCrouching;
        public static bool IsUsedFlashLight;
        public static bool IsReload;
        public static bool IsInteract;

        public static KeyCode jumpKey = KeyCode.Space;
        public static KeyCode crouchKey = KeyCode.C;
        
        public static bool IsRunning { get => IsSprinting; }
        public static bool IsMoving => Horizontal != 0 || Vertical != 0;

        private void Update()
        {
            Horizontal = Input.GetAxis("Horizontal");
            Vertical = Input.GetAxis("Vertical");
            HorizontalRaw = Input.GetAxisRaw("Horizontal");
            VerticalRaw = Input.GetAxisRaw("Vertical");
            MouseX = Input.GetAxis("Mouse X");
            MouseY = Input.GetAxis("Mouse Y");

            IsInteract = Input.GetKeyDown(KeyCode.F);
            IsAiming = Input.GetKeyDown(KeyCode.Mouse1);
            IsFireClickDown = Input.GetKeyDown(KeyCode.Mouse0);
            IsFireHolding = Input.GetKey(KeyCode.Mouse0);
            IsFireTypeClicked = Input.GetKeyDown(KeyCode.X);
            IsUsedFlashLight = Input.GetKeyDown(KeyCode.V);
            IsSprinting = Input.GetKey(KeyCode.LeftShift);
            IsReload = Input.GetKeyDown(KeyCode.R);
        }
    }
}