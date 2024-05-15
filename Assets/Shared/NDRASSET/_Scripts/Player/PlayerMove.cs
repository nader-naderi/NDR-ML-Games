using UnityEngine;
using System.Collections;

namespace NDRLiteFPS
{
    public class PlayerMove : MonoBehaviour
    {
        [SerializeField] private Animator headBobAnim;
        [SerializeField] private Animator anim;

        [SerializeField] private string horizontalInputName;
        [SerializeField] private string verticalInputName;

        [SerializeField] private float walkSpeed;
        [SerializeField] private float runSpeed;
        [SerializeField] private float runBuildUpSpeed;
        [SerializeField] private KeyCode runKey = KeyCode.LeftShift;
        [SerializeField] private float slopForce;
        [SerializeField] private float slopeForceRayLength;

        [SerializeField] private AnimationCurve jumpFallOff;
        [SerializeField] private float jumpMultiplier;
        [SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
        [SerializeField] private float m_StepInterval;
        [SerializeField] private AudioSource footSource;
        [SerializeField] private Footstep[] footsteps;
        [SerializeField] private AudioClip jumpStart;
        [SerializeField] private AudioClip landSound;


        private float movementSpeed;
        private CharacterController charController;

        private bool isJumping;
        private Footstep currentFootstep;
        private float playerForwardAnim;
        private float playerSidewayAnim;
        private bool isRunning = false;
        private bool m_IsWalking = false;
        private bool isCrouching = false;
        private float m_StepCycle;
        private float m_NextStep;
        private Vector3 lastPosition = Vector3.zero;
        private void Awake()
        {
            charController = GetComponent<CharacterController>();
        }

        private void Start()
        {
            currentFootstep = footsteps[0];
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle / 2f;
        }


        private void Update()
        {
            //anim.SetBool("isGrounded", charController.isGrounded);
            HandleMovement();
            HandleCrouch();
        }

        private void HandleMovement()
        {
            float verticalInput = Input.GetAxis(verticalInputName);
            float horizontalInput = Input.GetAxis(horizontalInputName);

            Vector3 forwardMovemnt = transform.forward * verticalInput;
            Vector3 rightMovement = transform.right * horizontalInput;

            charController.SimpleMove(Vector3.ClampMagnitude(forwardMovemnt + rightMovement, 1.0f) * movementSpeed);

            //if(charController.isGrounded)
            //    charController.Move(Vector3.ClampMagnitude(forwardMovemnt + rightMovement, 1.0f) * movementSpeed * Time.deltaTime);
            //else
            //    charController.Move(new Vector3(0, -5, 0) + forwardMovemnt + rightMovement * Time.deltaTime);


            if (verticalInput != 0 || horizontalInput != 0 && OnSlope())
                charController.Move(Vector3.down * charController.height / 2 * slopForce * Time.deltaTime);


            if (isRunning)
            {
                if (verticalInput < 0)
                    playerForwardAnim = -1f;
                else
                    playerForwardAnim = 1f;
                if (horizontalInput < 0)
                    playerSidewayAnim = -1f;
                else
                    playerSidewayAnim = 1f;

                m_IsWalking = false;
            }
            else
            {
                if (verticalInput < 0)
                    playerForwardAnim = -0.5f;
                else
                    playerForwardAnim = 0.5f;
                if (horizontalInput < 0)
                    playerSidewayAnim = -0.5f;
                else
                    playerSidewayAnim = 0.5f;

                m_IsWalking = true;
            }

            if (verticalInput == 0)
                playerForwardAnim = 0;
            if (horizontalInput == 0)
                playerSidewayAnim = 0;

            //anim.SetFloat("forward", playerForwardAnim, 0.1f, Time.deltaTime);
            //anim.SetFloat("sideways", playerSidewayAnim, 0.1f, Time.deltaTime);
            headBobAnim.SetFloat("Vertical", playerForwardAnim, 0.1f, Time.deltaTime);

            //if(InputManager.Horizontal == 0 && InputManager.Vertical == 0)
            //    anim.SetFloat("sideways", InputManager.MouseX, 0.5f, Time.deltaTime * 2);

            SetMovementSpeed();
            ProgressStepCycle(movementSpeed);
            HandleJump();
        }

        private void HandleJump()
        {
            if (Input.GetKeyDown(InputManager.jumpKey) && !isJumping)
            {
                //anim.CrossFade("JumpStart", Time.deltaTime * 0.25f);
                isJumping = true;
                StartCoroutine(jumpEvent());
            }
        }

        private bool OnSlope()
        {
            if (isJumping)
                return false;

            RaycastHit hit;

            if (Physics.Raycast(transform.position, Vector3.down, out hit, charController.height / 2 * slopeForceRayLength))
                if (hit.normal != Vector3.up)
                    return true;

            return false;
        }

        private IEnumerator jumpEvent()
        {
            charController.slopeLimit = 90.0f;

            float timeInAir = 0.0f;

            do
            {
                float jumpForce = jumpFallOff.Evaluate(timeInAir);
                charController.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime);
                timeInAir += Time.deltaTime;

                yield return null;
            } while (!charController.isGrounded && charController.collisionFlags != CollisionFlags.Above);

            charController.slopeLimit = 45.0f;
            PlayLandingSound();
            isJumping = false;
        }

        private void SetMovementSpeed()
        {
            if (Input.GetKey(runKey))
            {
                movementSpeed = Mathf.Lerp(movementSpeed, runSpeed, runBuildUpSpeed * Time.deltaTime);
                isRunning = true;
            }
            else
            {
                isRunning = false;
                movementSpeed = Mathf.Lerp(movementSpeed, walkSpeed, runBuildUpSpeed * Time.deltaTime);
                playerForwardAnim = 0.5f;
                playerSidewayAnim = 0.5f;
            }

        }

        private void HandleCrouch()
        {
            if (!isCrouching && Input.GetKeyDown(InputManager.crouchKey))
            {
                //anim.SetBool("isCrouching", true);
                //StartCoroutine(NDRUtility.LerpLinear(charController.height, 1, 5));
                //anim.transform.localPosition = new Vector3(-0.1f, -0.3f, -0.462f);
                charController.height = 1;
                isCrouching = true;
            }
            else if (isCrouching && Input.GetKeyDown(InputManager.crouchKey))
            {
                charController.height = 2;
                //anim.transform.localPosition = new Vector3(-0.1f, -1f, -0.462f);
                //anim.SetBool("isCrouching", false);
                //StartCoroutine(NDRUtility.LerpLinear(charController.height, 2, 5));
                isCrouching = false;
            }

            //if (isCrouching)
            //    charController.height = Mathf.Lerp(charController.height, 1f, 1f);
            //else
            //    charController.height = Mathf.Lerp(charController.height, 2f, 1.5f);
        }

        private void ProgressStepCycle(float speed)
        {
            if ((InputManager.Horizontal != 0 || InputManager.Vertical != 0))
            {

                float multiplier = ((speed * (m_IsWalking ? 1f : m_RunstepLenghten))) *
                                 Time.deltaTime;

                m_StepCycle += multiplier;
            }

            if (!(m_StepCycle > m_NextStep))
            {
                return;
            }

            m_NextStep = m_StepCycle + m_StepInterval;

            PlayFootstepSFX();
        }

        private void PlayFootstepSFX()
        {
            if (!charController.isGrounded) return;
            if (GroundType())
                if (currentFootstep.SurfaceType != GroundType().GetSurfaceType)
                    FindTypeOfSurface();

            int n = Random.Range(1, currentFootstep.StepClips.Length);
            footSource.clip = currentFootstep.StepClips[n];
            footSource.PlayOneShot(footSource.clip);

            currentFootstep.StepClips[n] = currentFootstep.StepClips[0];
            currentFootstep.StepClips[0] = footSource.clip;
        }

        private void FindTypeOfSurface()
        {
            SurfaceType wantedType = GroundType().GetSurfaceType;
            for (int i = 0; i < footsteps.Length; i++)
                if (footsteps[i].SurfaceType == wantedType)
                    currentFootstep = footsteps[i];

            return;
        }

        private void PlayLandingSound()
        {
            footSource.clip = landSound;
            footSource.Play();
            m_NextStep = m_StepCycle + .5f;
        }

        private SurfaceBehaviour GroundType()
        {
            Ray ray = new Ray(transform.position, Vector3.down);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, charController.bounds.extents.y + 1f))
            {
                if (hit.transform.GetComponent<SurfaceBehaviour>() && charController.isGrounded)
                    return hit.transform.GetComponent<SurfaceBehaviour>();
            }

            return null;
        }

        public bool IsMoving()
        {
            if (lastPosition != gameObject.transform.position || Input.GetAxis("Horizontal") != 0 ||
                    Input.GetAxis("Vertical") != 0 ||
                    Input.GetAxis("Mouse X") != 0 ||
                    Input.GetAxis("Mouse Y") != 0)
            {
                return true;
            }
            lastPosition = gameObject.transform.position;
            return false;
        }
    }
}