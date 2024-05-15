using NDRLiteFPS;

using UnityEngine;

namespace PerceptronExample
{

    public class MachineGun : MonoBehaviour
    {
        [SerializeField] private Projectile[] bullets;
        private int currentSelectedBullet;

        [SerializeField] private CanvasGroup[] bulletsGroup;
        [SerializeField] private Transform spawner;
        //Gun Sway
        [Header("----------GunSway----------")]
        [Space(10)]
        [SerializeField]
        Transform swayTransform;
        [SerializeField] float swaySensivityX = 20f;
        [SerializeField] float swaySensivityY = 20f;
        [SerializeField] float swaySensivityZ = 20f;
        private float swaySensivityXR;
        private float swaySensivityYR;
        private float swayvol;
        [SerializeField] float swaySmoothTime = 2;
        [SerializeField] float swaySensivityTime = 2;

        [Header("Recoil")]
        public float recoilAmount = 0.1f;
        public float recoilRecoverTime = 0.2f;
        public float rotationRecoil = 0.05f;

        float currentRecoilZPos = 0.2f;
        float currentRecoilZPosV = 0.2f;
        float currentRecoilYPos = 0.1f;
        float currentRecoilYosV = 0.1f;

        private Vector3 recoilPOs;

        [SerializeField] Perceptron perceptron;


        private void Update()
        {
            recoilPOs = Vector3.forward * currentRecoilZPos;

            GunSwayHandler();

            currentRecoilZPos = Mathf.SmoothDamp(currentRecoilZPos, 0, ref currentRecoilZPosV, recoilRecoverTime);
            currentRecoilYPos = Mathf.SmoothDamp(currentRecoilYPos, 0, ref currentRecoilYosV, recoilRecoverTime);
        
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Fire();
               
            }

            float mouseScroll = Input.GetAxis("Mouse ScrollWheel");

            if (mouseScroll < 0)
            {
                bulletsGroup[currentSelectedBullet].alpha = 0.5f;
                currentSelectedBullet--;

                if (currentSelectedBullet < 0)
                    currentSelectedBullet = bullets.Length - 1;

                bulletsGroup[currentSelectedBullet].alpha = 1;

            }
            else if (mouseScroll > 0)
            {
                bulletsGroup[currentSelectedBullet].alpha = 0.5f;

                currentSelectedBullet++;

                if (currentSelectedBullet >= bullets.Length)
                    currentSelectedBullet = 0;

                bulletsGroup[currentSelectedBullet].alpha = 1;
            }
        }

        public void GunSwayHandler()
        {
            if (!swayTransform)
                return;

            float rotationX = Input.GetAxis("Mouse X") * swaySensivityX;
            float rotationY = Input.GetAxis("Mouse Y") * swaySensivityY;
            swaySensivityYR = Mathf.SmoothDamp(swaySensivityYR, rotationY, ref swayvol, swaySmoothTime * Time.deltaTime);
            swaySensivityXR = Mathf.SmoothDamp(swaySensivityXR, rotationX, ref swayvol, swaySmoothTime * Time.deltaTime);

            var rotValue = Quaternion.Euler(swaySensivityYR, 0, swaySensivityXR);
            swayTransform.localRotation = Quaternion.Slerp(swayTransform.localRotation, rotValue, swaySensivityTime * Time.deltaTime);
        }

        void Fire()
        {
            currentRecoilZPos -= recoilAmount;
            currentRecoilYPos -= recoilAmount;

            Projectile projectile = bullets[currentSelectedBullet];

            Instantiate(projectile, spawner.position, spawner.rotation);

            perceptron.SendInput(projectile.Input.x, projectile.Input.y, projectile.Input.z);
        }
    }
}
