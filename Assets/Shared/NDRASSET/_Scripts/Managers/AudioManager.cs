using UnityEngine;
using System.Collections;

namespace NDRLiteFPS
{
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager _instance;

        public static AudioManager instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<AudioManager>();
                }

                return _instance;
            }
        }

        [SerializeField] AudioSource playerSource;
        [SerializeField] AudioSource interactSource;
        [SerializeField] AudioSource genericSource;
        [SerializeField] AudioSource weaponSource;
        [SerializeField] AudioSource impactSource;
        [SerializeField] AudioSource phoneSource;

        [SerializeField] AudioClip healthPickupClip;
        [SerializeField] AudioClip armorPickupClip;
        [SerializeField] AudioClip ammoPickupClip;
        [SerializeField] AudioClip genericPickupClip;
        [SerializeField] AudioClip drinkingClip;
        [SerializeField] AudioClip[] bulletImpactSoft;
        [SerializeField] AudioClip[] bulletImpactHard;
        [SerializeField] AudioClip[] bulletImpactGround;
        [SerializeField] AudioClip[] bulletImpactMetal;
        [SerializeField] AudioClip[] bulletImpactWood;

        [SerializeField] AudioClip weaponPickup;

        [SerializeField]
        AudioClip phoneRingTone;
        [SerializeField]
        AudioClip phoneDialTone;

        public void PlayHealthPickup() => interactSource.PlayOneShot(healthPickupClip);
        public void PlayArmorPickup() => interactSource.PlayOneShot(armorPickupClip);


        public void PlayWeaponEquip() => interactSource.PlayOneShot(weaponPickup);
        public void PlayGunShot(AudioClip fireShot) => weaponSource.PlayOneShot(fireShot);
        public void PlayGenericSound(AudioClip clip) => genericSource.PlayOneShot(clip);

        public void PlayPhoneRing() => phoneSource.PlayOneShot(phoneRingTone);
        public void PlayPhoneDialTone() => phoneSource.PlayOneShot(phoneDialTone);

        public void BulletImpactSoft(AudioSource source) => source.PlayOneShot(bulletImpactSoft[Random.Range(0, bulletImpactSoft.Length)]);
        public void BulletImpactHard(AudioSource source) => source.PlayOneShot(bulletImpactHard[Random.Range(0, bulletImpactHard.Length)]);
        public void BulletImpactGround(AudioSource source) => source.PlayOneShot(bulletImpactGround[Random.Range(0, bulletImpactGround.Length)]);
        public void BulletImpactMetal(AudioSource source) => source.PlayOneShot(bulletImpactMetal[Random.Range(0, bulletImpactMetal.Length)]);
        public void BulletImpactWood(AudioSource source) => source.PlayOneShot(bulletImpactWood[Random.Range(0, bulletImpactWood.Length)]);

        public void PlayDrinkingSound() => genericSource.PlayOneShot(drinkingClip);
        public void PlayInteractSound(AudioClip clip) => interactSource.PlayOneShot(clip);

        public void StopPhoneSound()
        {
            if (phoneSource.isPlaying)
                phoneSource.Stop();
        }
    }
}