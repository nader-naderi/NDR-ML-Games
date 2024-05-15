using UnityEngine;
using System.Collections;

namespace NDRLiteFPS
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(Weapon))]
    public class WeaponPickup : Pickupable
    {
        [SerializeField] Weapon weapon;

        public Weapon Weapon { get => weapon; }

        public override void Interact()
        {
            base.Interact();
            weapon.enabled = true;
            WeaponsManager.instance.EquipWeapon(this);
        }
    }
}