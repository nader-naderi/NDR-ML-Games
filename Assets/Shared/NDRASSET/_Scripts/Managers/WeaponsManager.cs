using UnityEngine;
using System.Collections;

namespace NDRLiteFPS
{
    public class WeaponsManager : MonoBehaviour
    {
        private static WeaponsManager _instance;

        public static WeaponsManager instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<WeaponsManager>();
                }

                return _instance;
            }
        }

        [SerializeField] PlayerWeaponBob weaponBob;
        [SerializeField] PlayerLook mouseLook;
        [SerializeField] Camera weaponCam;
        [SerializeField] Transform weaponsParent;

        private WeaponPickup currentWeapon;
        public PlayerLook MouseLook { get => mouseLook; }
        public PlayerWeaponBob WeaponBob { get => weaponBob; }

        public void EquipWeapon(WeaponPickup pickup)
        {
            pickup.enabled = false;
            pickup.transform.SetParent(weaponsParent);
            pickup.transform.position = weaponsParent.position + Vector3.down;
            pickup.transform.rotation = weaponsParent.rotation;
            weaponCam.enabled = true;

            if (currentWeapon)
            {
                currentWeapon.Weapon.enabled = false;
                currentWeapon.transform.parent = null;
                currentWeapon.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, 1f + transform.position.z) * 10f, ForceMode.Force);
                //weaponCam.enabled = false;

            }

            currentWeapon = pickup;
        }
    }
}