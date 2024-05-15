using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NDRLiteFPS
{

    public enum WeaponStates
    {
        Idle = 0,
        Aim = 1,
        Walk = 2,
        Run = 3,
        Attack = 4,
    }

    public enum WeaponFireType
    {
        Single,
        Shotgun
    }

    public class Weapon : MonoBehaviour
    {
        [SerializeField] WeaponStates currentWeaponStates;
        [SerializeField] WeaponFireType fireType;
        [SerializeField] GameObject[] childern;
        [SerializeField] new Rigidbody rigidbody;
        [SerializeField] new Collider collider;

        [SerializeField] LayerMask layerMask;


        [SerializeField] WeaponOffset kickbackOffset;

        [SerializeField] Vector3 currentOffset;
        [SerializeField] WeaponOffset[] weaponOffsets;

        [SerializeField] SequencedWeaponOffset[] reloadOffsets;
        private int currentReloadOffsetIndex;

        [Space(10), Header("Stats")]
        [SerializeField] private bool isAuto = false;
        [SerializeField] float rateOfFire = 0.1f;
        [Space(10), Header(" ------ Spread :")]
        [SerializeField] float baseSpread = 4; // Base spread.
        [SerializeField] float maxSpread = 6; // Maximum spread.
        [SerializeField] float baseSpreadCrouch = 3; // Base spread when crouched.
        [SerializeField] float spreadAIM = 0.4f; // Spread when aiming.
        [SerializeField] float spreadWhenMove = 2;  // Increase spread amount when moving.
        [SerializeField] float spreadPerShot = 2; // Increase spread amount when firing.

        private float spread; // The current value of spread.
        private int currentOffsetIndex = 0;

        [Space(10)]
        [Header("----------Gun Ammunations----------")]
        [SerializeField] int magSize = 30;
        [SerializeField] int currentBullets = 30;
        [SerializeField] private int bulletsLeft = 300;
        [SerializeField] int ammoAlert = 3;
        [SerializeField] int maxBullets;
        [SerializeField] float reloadDuration = 2f;

        [Space(10)]
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
        [Space(10)]
        [Header("----------Shake Stats----------")]
        [SerializeField] private float shakeMagnitude = 4f;
        [SerializeField] private float shakeRoughness = 4f;
        [SerializeField] private float shakeFadeIn = 0.1f;
        [SerializeField] private float shakeFadeOut = 0.1f;


        [Space(10), Header("GFX")]
        [SerializeField] ParticleSystem muzzleFlash;
        [SerializeField] Light muzzleLight;
        [Space(10), Header("SFX")]
        [SerializeField] AudioClip[] fireClips;
        [SerializeField] AudioClip reloadClip;
        [SerializeField] AudioClip dryFire;

        private bool isEquiped = false;
        private float shotTimer;
        private bool isInReload;

        public Vector3 CurrentKickbackPosition { get; set; }

        public Vector3 CurrentKickbackRotation { get; set; }
        private void OnDisable()
        {
            isEquiped = false;
            foreach (GameObject item in childern)
            {
                item.layer = 0;
                if (item.GetComponent<SkinnedMeshRenderer>())
                    item.SetActive(isEquiped);
            }
            collider.tag = "Interactable";
            rigidbody.isKinematic = isEquiped;
            rigidbody.useGravity = !isEquiped;
            collider.enabled = !isEquiped;

        }

        private void OnEnable()
        {
            isEquiped = true;
            foreach (GameObject item in childern)
            {
                item.layer = 7;
                if (item.GetComponent<SkinnedMeshRenderer>())
                    item.SetActive(isEquiped);
            }
            collider.tag = "Untagged";
            rigidbody.isKinematic = isEquiped;
            rigidbody.useGravity = !isEquiped;
            collider.enabled = !isEquiped;
        }

        private void Update()
        {
            CurrentKickbackPosition = Vector3.Lerp(CurrentKickbackPosition, Vector3.zero, Time.deltaTime * kickbackOffset.PositionSpeed);
            CurrentKickbackRotation = Vector3.Lerp(CurrentKickbackRotation, Vector3.zero, Time.deltaTime * kickbackOffset.RotationSpeed);
            UpdateSpread();
            UpdateWeaponStates();

            if (isAuto)
            {
                shotTimer += Time.deltaTime;
                if (Input.GetMouseButton(0) && shotTimer > rateOfFire)
                {
                    HandleFire();
                    shotTimer = 0;
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                    HandleFire();
            }

            if(InputManager.IsReload)
            {
                isInReload = true;
            }

            if (!isInReload)
                weaponOffsets[currentOffsetIndex].Update(this);
            else
            {
                reloadOffsets[currentReloadOffsetIndex].Update(this);

                if (reloadOffsets[currentReloadOffsetIndex].IsCompleted())
                    currentReloadOffsetIndex++;

                if (currentReloadOffsetIndex >= reloadOffsets.Length)
                {
                    currentReloadOffsetIndex = 0;
                    isInReload = false; ReloadCalculation();
                    //ReloadWithDelay();
                }
            }
        }

        private void HandleFire()
        {
            CurrentKickbackPosition = kickbackOffset.WantedPosition;
            CurrentKickbackRotation = kickbackOffset.WantedRotation;
            AudioManager.instance.PlayGunShot(fireClips[Random.Range(0, fireClips.Length)]);

            int palletCount = 1;

            switch (fireType)
            {
                case WeaponFireType.Single:
                    palletCount = 1;
                    break;
                case WeaponFireType.Shotgun:
                    palletCount = 8;
                    break;
                default:
                    break;
            }

            for (int i = 0; i < palletCount; i++)
            {
                FireWithRay();
            }

            muzzleFlash.Play();

        }

        private void FireWithRay()
        {
            RaycastHit hit;

            Vector3 direction = WeaponsManager.instance.MouseLook.transform.TransformDirection(ShotDirection());

            Vector3 origin = WeaponsManager.instance.MouseLook.transform.position;

            Ray ray = new Ray(origin, direction);
            //Instantiate(projectile, origin + direction, cameraTarget.transform.rotation);
            if (Physics.Raycast(ray, out hit, 100, layerMask))
            {
                var rb = hit.collider.GetComponent<Rigidbody>();
                if (rb) rb.AddForceAtPosition(ray.direction * Random.Range(5, 20f), hit.point, ForceMode.Impulse);

                SurfaceBehaviour surfaceBehaviour;
                if (hit.collider.GetComponent<SurfaceBehaviour>())
                {
                    surfaceBehaviour = hit.collider.GetComponent<SurfaceBehaviour>();
                    SurfaceImpact bh = Instantiate(WeaponsGFX.instance.GetImpact(surfaceBehaviour.GetSurfaceType).GetSurfaceImpact,
                        hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));

                    bh.transform.parent = hit.transform;

                    AudioManager.instance.BulletImpactSoft(bh.GetComponent<AudioSource>());
                    Destroy(bh, 12f);
                }
                else if (!hit.collider.isTrigger && !hit.collider.CompareTag("Player"))
                {
                    surfaceBehaviour = hit.transform.gameObject.AddComponent<SurfaceBehaviour>();
                    SurfaceImpact bh = Instantiate(WeaponsGFX.instance.GetImpact(surfaceBehaviour.GetSurfaceType).GetSurfaceImpact,
                        hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));

                    bh.transform.parent = hit.transform;

                    AudioManager.instance.BulletImpactSoft(bh.GetComponent<AudioSource>());

                    Destroy(bh, 12f);
                }
            }
        }

        public void ChangeWeaponState(WeaponStates state)
        {
            currentWeaponStates = state;
            currentOffsetIndex = (int)currentWeaponStates;
            WeaponsManager.instance.WeaponBob.enabled = true;

            switch (currentWeaponStates)
            {
                case WeaponStates.Idle:
                    break;
                case WeaponStates.Aim:
                    WeaponsManager.instance.WeaponBob.enabled = false;
                    break;
                case WeaponStates.Walk:
                    break;
                case WeaponStates.Run:
                    break;
                case WeaponStates.Attack:
                    break;
                default:
                    break;
            }
        }

        public void UpdateWeaponStates()
        {
            switch (currentWeaponStates)
            {
                case WeaponStates.Idle:
                    if (Input.GetMouseButtonDown(1)) ChangeWeaponState(WeaponStates.Aim);
                    else if (InputManager.Horizontal != 0 || InputManager.Vertical != 0) ChangeWeaponState(WeaponStates.Walk);
                    break;
                case WeaponStates.Aim:
                    if (Input.GetMouseButtonDown(1)) ChangeWeaponState(WeaponStates.Idle);
                    break;
                case WeaponStates.Walk:
                    if (InputManager.IsRunning) ChangeWeaponState(WeaponStates.Run);
                    else if (InputManager.Horizontal == 0 || InputManager.Vertical == 0) ChangeWeaponState(WeaponStates.Idle);
                    break;
                case WeaponStates.Run:
                    if (!InputManager.IsRunning) ChangeWeaponState(WeaponStates.Walk);
                    break;
                case WeaponStates.Attack:

                    break;
                default:
                    break;
            }

            if (currentWeaponStates != WeaponStates.Aim)
            {

            }
        }

        private Vector3 ShotDirection()
        {
            float x = Random.Range(-0.01f, 0.01f) * spread / 1.5f; // Horizontal variation
            float y = Random.Range(-0.01f, 0.01f) * spread / 1.5f; // Vertical variation
            return new Vector3(x, y, 1); // Returns a vector with random values based on the current spread.
        }

        private void UpdateSpread()
        {
            spread = Mathf.Clamp(spread, spreadAIM, maxSpread); // Limits the spread value to never be higher than maxSpread and lower than spreadAIM.

            if (InputManager.IsMoving || shotTimer > Time.time) // The player is not idle or are shooting?
            {
                if (currentWeaponStates == WeaponStates.Aim) // Is aiming?
                {
                    if (InputManager.IsCrouching) // The player is crouched?
                    {
                        // check for spread crouch value.
                        if (spread != baseSpreadCrouch)
                            spread = Mathf.Lerp(spread, baseSpreadCrouch,
                                (shotTimer > Time.time ? spreadPerShot : spreadWhenMove) * Time.deltaTime); // Changes the spread value to baseSpreadCrouch.
                    }
                    else
                    {
                        // check for neccesity of Updates.
                        if (spread != baseSpread)
                            spread = Mathf.Lerp(spread, baseSpread,
                                (shotTimer > Time.time ? spreadPerShot : spreadWhenMove) * Time.deltaTime); // Changes the spread value to baseSpread.
                    }
                }
                else
                {
                    if (spread < maxSpread)
                        spread += (shotTimer > Time.time ? spreadPerShot : spreadWhenMove) * Time.deltaTime; // Changes the spread value to maxSpread.
                }
            }
            else
            {
                if (currentWeaponStates == WeaponStates.Aim)
                {
                    if (spread != spreadAIM)
                        spread = Mathf.Lerp(spread, spreadAIM, spreadWhenMove * Time.deltaTime); // Changes the spread value to spreadAIM.
                }
                else
                {
                    if (InputManager.IsCrouching && baseSpreadCrouch != baseSpread)
                    {
                        if (spread != baseSpreadCrouch)
                            spread = Mathf.Lerp(spread, baseSpreadCrouch, spreadWhenMove * Time.deltaTime); // Changes the spread value to baseSpreadCrouch.
                    }
                    else
                    {
                        if (spread != baseSpread)
                            spread = Mathf.Lerp(spread, baseSpread, spreadWhenMove * Time.deltaTime); // Returns the spread value to the default.
                    }
                }
            }
        }

        void ReloadCalculation()
        {
            if (bulletsLeft <= 0) return;

            if (fireType != WeaponFireType.Shotgun)
            {
                int bulletsToLead = magSize - currentBullets;
                int bulletsToDeduct = (bulletsLeft >= bulletsToLead) ? bulletsToLead : bulletsLeft;

                bulletsLeft -= bulletsToDeduct;
                currentBullets += bulletsToDeduct;
            }

        }

        IEnumerator ReloadWithDelay(float duration = 0.2f)
        {
            if (bulletsLeft <= 0) yield return null;

            currentWeaponStates = WeaponStates.Idle;

            AudioManager.instance.PlayGenericSound(reloadClip);
            yield return new WaitForSeconds(duration);
            ReloadCalculation();
            //UpdateUI();
            //UIManager.instance.DisableAmmoNotify();
            isInReload = false;
        }
    }

    [System.Serializable]
    public class WeaponOffset
    {
        [SerializeField] protected string name;
        [SerializeField] protected Vector3 wantedPosition;
        [SerializeField] protected Vector3 wantedRotation;
        [SerializeField] protected float positionSpeed = 6f;
        [SerializeField] protected float rotationSpeed = 12f;

        public Vector3 WantedPosition { get => wantedPosition; }
        public Vector3 WantedRotation { get => wantedRotation; }
        public float PositionSpeed { get => positionSpeed; }
        public float RotationSpeed { get => rotationSpeed; }

        public virtual void Update(Weapon weapon)
        {
            weapon.transform.localPosition = Vector3.Lerp(weapon.transform.localPosition, wantedPosition + weapon.CurrentKickbackPosition,
                Time.deltaTime * positionSpeed);
            weapon.transform.localEulerAngles = Vector3.Lerp(weapon.transform.localEulerAngles, wantedRotation + weapon.CurrentKickbackRotation,
                Time.deltaTime * rotationSpeed);
        }
    }

    [System.Serializable]
    public class SequencedWeaponOffset : WeaponOffset
    {
        [SerializeField] protected float duration = 1f;
        [SerializeField] protected AudioClip clip;
        protected float timer;

        public override void Update(Weapon weapon)
        {
            base.Update(weapon);

            if (timer == 0)
                AudioManager.instance.PlayGenericSound(clip);

            timer += Time.deltaTime;
        }

        public bool IsCompleted()
        {
            if (timer >= duration)
            {
                timer = 0;
                return true;
            }
            else return false;
        }
    }
}