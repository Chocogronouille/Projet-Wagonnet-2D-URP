using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.InputSystem.Haptics;
using UnityEngine.InputSystem.XR.Haptics;

namespace Cinemachine
{
    #region OldTrackScriptRemains
    
    /// <summary>
    /// This is a very simple behaviour that constrains its transform to a CinemachinePath.
    /// It can be used to animate any objects along a path, or as a Follow target for 
    /// Cinemachine Virtual Cameras.
    /// </summary>
    [DocumentationSorting(DocumentationSortingAttribute.Level.UserRef)]
#if UNITY_2018_3_OR_NEWER
    [ExecuteAlways]
#else
    [ExecuteInEditMode]
#endif
    [DisallowMultipleComponent]
    //   [HelpURL(Documentation.BaseURL + "manual/CinemachineDollyCart.html")]
    
    #endregion
    public class PlayerInput : MonoBehaviour
    {
        #region Variables
        
        private InputActions farmerInputActions;
        private float _jumpBuffer;
        private float _horizontalSpeed;
        private float _verticalSpeed;
        private float _jumpDuration;
        private bool _wantToEndJump;
        [SerializeField] private float _maxSpeed;
        private float _maxFallSpeed;
        private int _canSpinJump;
        private bool useRailSpeed;
        private bool _falledFromBallon;
        private bool _falledFromPlatform;

        public bool isFalling;
        public static PlayerInput instance; // singleton
        public InputAction movement;
        public bool isAirborn;
        public bool coyoteFloat;
        public int numberOfSpinJump;
        public Vector2 direction;
        public float apexThreshold;
        public float defaultGravityScale;
        public float airDrag; //set between 0 and 1
        public float groundDrag;
        public GameObject groundCheck;
        public JumpState jumpState;
        public enum JumpState
        {
            Ballon = 1, Platform = 2, Ground = 3
        }

        public Animator animator;
        private float delaySpinJump = 0.35f;
        public float characterVelocity;

        //private string currentState;
        public SpriteRenderer spriteRenderer;
        public SpriteRenderer screenRenderer;
        public Rigidbody2D rbCharacter;

        [HideInInspector] public Tween currentTween;


        [SerializeField] public float walkSpeed;
        [SerializeField] public float afterSpinAirSpeed;
        [SerializeField] private float jumpForce;
        [SerializeField] private float spinJumpForce;
        [SerializeField] private float fallSpeed;
        [SerializeField] private float fastFallSpeed;
        [SerializeField] private float railSpeed;
        [SerializeField] private float jumpBufferTime;
        [SerializeField] private float coyoteTime;
        [SerializeField] private float apexEndJump;
        [SerializeField] private float minJumpDuration;
        [SerializeField] private float maxJumpDuration;
        [SerializeField] private float spinJumpDuration;
        [SerializeField] private float fallBallonDelay;
        [SerializeField] private float fallPlatformDelay;
        [SerializeField] private float facteurAccel;
        [SerializeField] private float facteurDecelSpinJump; //set between 0 and 1
        [SerializeField] private float ecartDepartDecelSpinJump; //set between 0 and 1
        [SerializeField] private float facteurDecelAfterRail;
        
        // Variable des Rails
        public bool isSurfing;
        private float waitTime = 0.0001f;
        private GameObject TheChild;

        // La Cam
        private GameObject laCamParent;
        private GameObject laCam;
        
        // Ejection
        public bool isEject;

        // Stop Jump during Interaction
        public bool isInteract;
        
        //Liste Plateforme A faire Apparaitre Après Descente
        [HideInInspector] public Collider2D currentPlatform;
       
        // Particule System
        public ParticleSystem Effects;
        public ParticleSystem FallEffects;
        
        #endregion

        void Awake()
        {
            // La cam
            laCamParent = GameObject.Find("VirtualCam");
            laCam = laCamParent.transform.GetChild(0).gameObject;
            laCam.SetActive(false);
            StartCoroutine(DesactiveCamera());
            TheChild = GameObject.Find("PlayerCollider");
            farmerInputActions = new InputActions();
            _maxFallSpeed = fallSpeed;
            _maxSpeed = walkSpeed;

            #region singleton

            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;

            #endregion

            animator = GetComponent<Animator>();
        }
        
        private IEnumerator DesactiveCamera()
        {
            yield return new WaitForSeconds(0.000001f);
            laCam.SetActive(true);
        } 

        private void OnEnable()
        {
            movement = farmerInputActions.Player.Movement;
            movement.Enable();

            farmerInputActions.Player.Jump.performed += DoJump;
            farmerInputActions.Player.Jump.canceled += EndJump;
            farmerInputActions.Player.Jump.Enable();
        }
        
        #region InputAction

        public Vector2 railDirection;
        public float railJump = 25;

        private void DoJump(InputAction.CallbackContext obj)
        {
            _jumpBuffer = jumpBufferTime;
            
            if (!isAirborn) return;
            if(isEject) return;
            if (isSurfing) return;

            if (_canSpinJump != 0)
            {
                _jumpBuffer = 0;
                SpinJump();
            }
        }
        
        private void EndJump(InputAction.CallbackContext obj)
        {
            if(isEject) return;
            if (isSurfing) return;
            
            groundCheck.SetActive(true);

            if (!isFalling)
            {
                if (isAirborn)
                {
                    _wantToEndJump = true;
                }
            }
            else
            {
                _jumpBuffer = 0;
            }
        }

        #endregion

        private Vector2 lastPos = Vector2.zero;
        public Vector2 deplacement;

        private void FixedUpdate()
        {
            if(isFalling)
            {
                animator.SetBool("isOnFloor", false);
            }
              else
            {
                animator.SetBool("isOnFloor", true);
            }
            if(isAirborn)
            {
                animator.SetBool("isFlying", true);
            }
              else
            {
                animator.SetBool("isFlying", false);
            }
            Movement();
            CalculateDeplacement();
            Flip(rbCharacter.velocity.x); //Flip le joueur en fonction de sa vitesse  //N'EST PAS UNE DE MES FONCTIONS
        }

        // TODO FIND THE GOOD MOVEMENT VALUE
        private void CalculateDeplacement()
        {
            deplacement = transform.position - new Vector3(lastPos.x, lastPos.y, 0);
            lastPos = transform.position;
        }

        private void Movement()
        {
            Surf();
            JumpBuffer();
            if(isEject) return;
            if (isSurfing) return;
            FallManagement();
            CheckCoyoteTime();
            CheckFastFall();
            CheckMove();
            ClampMove();
        }

        
        #region FonctionsDéplacements
        
        private void Surf()
        {
            if (isSurfing)
            {
                animator.SetBool("isSurfing", true);
                isAirborn =  false;
                isFalling = false;
            }
            else
            {
                animator.SetBool("isSurfing", false);
            }

            direction = movement.ReadValue<Vector2>();
            characterVelocity = Mathf.Abs(rbCharacter.velocity.x); //N'EST PAS UNE DE MES FONCTIONS
            animator.SetFloat("Speed", characterVelocity); //N'EST PAS UNE DE MES FONCTIONS
        }
        
        private void JumpBuffer()
        {
            // Jump Buffer
            if (_jumpBuffer <= 0) return;
            
            _jumpBuffer -= 1 * Time.deltaTime;

            if(isEject) return;
            if (isSurfing) return;
            if (isAirborn) return;
            CheckJump();
            
        }
        
        private void FallManagement()
        {
            if (isFalling) return;
            if (!isAirborn) return;

            _jumpDuration += 1 * Time.deltaTime;

            if (_jumpDuration > maxJumpDuration)
            {
                Fall();
            }

            if (!_wantToEndJump) return;
            if (rbCharacter.velocity.y <= apexThreshold) return;

            if (_jumpDuration > minJumpDuration)
            {
                Fall();
            }
        }
        
        private void CheckCoyoteTime()
        {
            if (isAirborn) return;
            if (rbCharacter.velocity.y > -1) return;
            if (coyoteFloat) return;
                
            coyoteFloat = true;
            StartCoroutine(CoyoteTime());
                
        }
        
        private void CheckFastFall()
        {
            if (direction.y < -0.9f)
            {
                if (!isAirborn) Crouch();
                FastFall();
            }
            else if(direction.y > -0.9f)
            {
            animator.SetBool("isCrouching",false);
            }
        }
        
        private void Crouch()
        {
            animator.SetBool("isCrouching",true);
            return;
        }
        
        private void CheckMove()
        {
            if ((direction.x < -0.1f) ||  (0.1f < direction.x))
            {
                Move();
            }
            else
            {
                AirSlowDown();
            }
        }

        public void SetAirSpeedAfterRail(float ejectionSpeedX)
        {
            if (ejectionSpeedX < 0) ejectionSpeedX = -ejectionSpeedX;
            //On prend la valeur absolu de la vitesse
            if(ejectionSpeedX > walkSpeed)
            {
                _maxSpeed = ejectionSpeedX;
            }
            else
            {
                _maxSpeed = walkSpeed;
            }
        }
        
        public void ResetMaxSpeed()
        {
            if (_maxSpeed<walkSpeed) _maxSpeed = walkSpeed;
        }

        private void Move()
        {
            if (isInteract) return;
            
            rbCharacter.drag = 0;
            rbCharacter.AddForce(new Vector2(walkSpeed * direction.x*facteurAccel, 0f));
        }

        private void ClampMove()
        {
            if (_maxSpeed > walkSpeed)
            {
                if(isAirborn) _maxSpeed -= facteurDecelAfterRail * Time.deltaTime;
                else _maxSpeed -= facteurDecelAfterRail * 3 * Time.deltaTime;
                
                if (Mathf.Abs(rbCharacter.velocity.x)<walkSpeed)
                {
                    _maxSpeed = walkSpeed;
                }
            }

            _horizontalSpeed = Mathf.Clamp(rbCharacter.velocity.x, -_maxSpeed, _maxSpeed);
            _verticalSpeed = Mathf.Clamp(rbCharacter.velocity.y, -_maxFallSpeed, Single.PositiveInfinity); 
            rbCharacter.velocity = new Vector2(_horizontalSpeed, _verticalSpeed);
        }

        private void AirSlowDown()
        {
            if (!isFalling) return;
            var vel = rbCharacter.velocity;
            vel.x *= 1f-airDrag;
            rbCharacter.velocity = vel;
        }

        private void CheckJump()
        {
            rbCharacter.gravityScale = defaultGravityScale;
            rbCharacter.drag = 0;
            isFalling = false;
            isAirborn = true;
            _jumpBuffer = 0;

            if (isEject) return;
            
            switch (jumpState)
            {
                case JumpState.Ballon:
                    GetComponentInChildren<Ballon>()?.JumpFromBallon();
                    if (direction.y < -0.9)
                    {
                        FallFromBallon();
                    }
                    else
                    {
                        Jump();
                    }
                    jumpState = JumpState.Ground;
                    break;
                
                case JumpState.Platform:
                    if (direction.y < -0.9)
                    {
                        FallFromPlatform();
                    }
                    else
                    {
                        Jump();
                    }
                    jumpState = JumpState.Ground;
                    break;
                
                default:
                    Jump();
                    break;
            }
        }

        private void Jump()
        {
            if (currentPlatform != null)
            {
                currentPlatform.enabled = true;
                currentPlatform = null;
            }

            if (isInteract) return;
            
            ResetMaxSpeed();
            _jumpDuration = 0;
            transform.localEulerAngles = new Vector3(0,0,0);
            rbCharacter.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            animator.SetBool("isJumping", true);
            groundCheck.SetActive(false);
            
            StartCoroutine(JumpRumble());
        }

        private void SpinJump()
        {
            Effects.Play();
            GetComponentInChildren<Ballon>()?.JumpFromBallon();
            if (currentPlatform != null)
            {
                currentPlatform.enabled = true;
                currentPlatform = null;
            }

            //On réactive la dernière maison au cas où le joueur en est descendu

            jumpState = JumpState.Ground;
            rbCharacter.gravityScale = defaultGravityScale;
            rbCharacter.drag = 0;
            isFalling = false;
            isAirborn = true;
            coyoteFloat = false;
            groundCheck.SetActive(false);
            transform.localEulerAngles = new Vector3(0,0,0);
            _jumpDuration = maxJumpDuration - spinJumpDuration;

            // ATTENTION : FORMULE DE RALENTISSEMENT PROGRESSIVE DE LA VITESSE APRES UN SPIN JUMP ECRITE EN DUR
            _maxSpeed = afterSpinAirSpeed + (_maxSpeed-afterSpinAirSpeed)*(_canSpinJump-1)*facteurDecelSpinJump*ecartDepartDecelSpinJump;
            
            rbCharacter.velocity = new Vector2(rbCharacter.velocity.x, 0); //Arrêt de la montée et lissage de l'apex
            rbCharacter.AddForce(new Vector2(0, spinJumpForce*_canSpinJump), ForceMode2D.Impulse);

            animator.SetBool("IsSpinJumping", true); //N'EST PAS UNE DE MES FONCTIONS
            
            StopCoroutine(JumpRumble());
            StartCoroutine(JumpRumble());
            StartCoroutine(TimerSpinJump(delaySpinJump));

            _canSpinJump -= 1;
        }
        
        public void Fall()
        {
            isFalling = true;
            _wantToEndJump = false;
            rbCharacter.gravityScale = defaultGravityScale;
            _maxFallSpeed = fallSpeed;

            rbCharacter.velocity = new Vector2(rbCharacter.velocity.x, 0f); //Arrêt de la montée et lissage de l'apex
            rbCharacter.AddForce(new Vector2(0, apexEndJump), ForceMode2D.Impulse);

            animator.SetBool("IsFalling", true); //N'EST PAS UNE DE MES FONCTIONS
            animator.SetBool("isJumping", false); //N'EST PAS UNE DE MES FONCTIONS
        }

        private void FallFromBallon()
        {
            Fall();
            StartCoroutine(NoFastFallFromBallon());
        }

        private IEnumerator NoFastFallFromBallon()
        {
            _falledFromBallon = true;
            yield return new WaitForSeconds(fallBallonDelay);
            _falledFromBallon = false;
            StopCoroutine(NoFastFallFromBallon());
        }

        public void StandOnPlatform(Collider2D platformCollider) //Fonction appelée lors d'une collision avec une plateforme
        {
            currentPlatform = platformCollider;
            jumpState = JumpState.Platform;
        }
        private void FallFromPlatform()
        {
            Fall();
            StartCoroutine(NoFastFallFromPlatform());
        }
        
        private IEnumerator NoFastFallFromPlatform()
        {
            _falledFromPlatform = true;
            currentPlatform.enabled = false;
            yield return new WaitForSeconds(fallPlatformDelay);
            _falledFromPlatform = false;
            
            StopCoroutine(NoFastFallFromPlatform());
        }
        private void FastFall()
        {
            if (!isAirborn) return;
            if (_falledFromBallon) return;
            if (_falledFromPlatform) return;
            FallEffects.gameObject.SetActive(true);
          //  FallEffects.Play();
            _maxFallSpeed = fastFallSpeed;
            rbCharacter.velocity = new Vector2(rbCharacter.velocity.x, -fastFallSpeed);
        }
        
        public void ResetSpinJump()
        {
            _canSpinJump = numberOfSpinJump;
        }

        #endregion
        

        #region FonctionsAnimator

        void Flip(float velocity)
        {
            if (velocity > 0.1f)
            {
                spriteRenderer.flipX = false;
                screenRenderer.flipX = false;
            }
            else if (velocity < -0.1f)
            {
                spriteRenderer.flipX = true;
                screenRenderer.flipX = true;
            }
        }

        #endregion
        
        #region Coroutine

        private IEnumerator CoyoteTime()
        {
            rbCharacter.drag = 0;

            yield return new WaitForSeconds(coyoteTime);

            isAirborn = true;
            coyoteFloat = false;
            isFalling = true;
            rbCharacter.gravityScale = defaultGravityScale;

            StopCoroutine(CoyoteTime());
        }

        private IEnumerator TimerSpinJump(float delaySpinJump)
        {
            yield return new WaitForSeconds(delaySpinJump);
            animator.SetBool("IsSpinJumping", false);
        }

        private IEnumerator JumpRumble()
        {
            Gamepad.current.SetMotorSpeeds(1f, 0.5f);
            yield return new WaitForSecondsRealtime(0.04f);
            Gamepad.current.SetMotorSpeeds(0f, 0f);
        }
        #endregion

        private void OnDestroy()
        { 
            Gamepad.current.SetMotorSpeeds(0f, 0f);
            movement.Disable();
            farmerInputActions.Player.Jump.Disable();
        }
    }
}