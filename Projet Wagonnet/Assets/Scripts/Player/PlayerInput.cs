using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

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
        public float _maxSpeed;
        private float _maxFallSpeed;
        private int _canSpinJump;
        private bool useRailSpeed;
        private bool _falledFromBallon;

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
            ballon = 1, plateforme = 2, ground = 3
        }

        public Animator animator;
        private float delaySpinJump = 0.35f;

        //private string currentState;
        public SpriteRenderer spriteRenderer;
        public SpriteRenderer screenRenderer;
        public Rigidbody2D rbCharacter;

        [HideInInspector] public Tween currentTween;


        [SerializeField] public float walkSpeed;
        [SerializeField] private float jumpForce;
        [SerializeField] private float spinJumpForce;
        [SerializeField] private float fallSpeed;
        [SerializeField] private float fastFallSpeed;
        [SerializeField] private float railSpeed;
        [SerializeField] private int jumpBufferTime;
        [SerializeField] private float coyoteTime;
        [SerializeField] private float apexEndJump;
        [SerializeField] private float minJumpDuration;
        [SerializeField] private float maxJumpDuration;
        [SerializeField] private float spinJumpDuration;
        [SerializeField] private float fallBallonDelay;

        // Variable des Rails
        public bool isSurfing;
        private float waitTime = 0.0001f;
        private GameObject TheChild;

        // La Cam
        private GameObject laCamParent;
        private GameObject laCam;

        // Ejection
        public bool isEject;
        
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
              //Destroy(gameObject);
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

            farmerInputActions.Player.SpinMove.performed += DoSpin;
            farmerInputActions.Player.SpinMove.Enable();
        }

        #region InputAction

        public Vector2 railDirection;
        public float railJump = 25;

        private void DoJump(InputAction.CallbackContext obj)
        {
            gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);
            currentTween?.Kill();

            _jumpBuffer = jumpBufferTime;

            if (!isSurfing) return;
            isSurfing = false;
            AJF(25);
        }

        // TODO SEE IF EQUILIBRAGE DE RAIL DIRECTION OR VOUS DEBUGUEZ LE DEPLACEMENT DEPUIS LA FRAME PRECEDENTE
        public void ApplyJumpForce(int jumpBonus = 0)
        {
            useRailSpeed = true;
            _maxSpeed = railSpeed;
            rbCharacter.AddForce(new Vector2(railDirection.x * railJump + 10, railDirection.y * railJump + 5), ForceMode2D.Impulse);
        }
        public void AJF(int jumpBonus = 0)
        {
            rbCharacter.AddForce(new Vector2(0, railDirection.y * railJump + 0),ForceMode2D.Impulse);
            Debug.Log("Jump_Rail");
            StartCoroutine(ChangeGravity());
        }
        private IEnumerator ChangeGravity()
        {
            Debug.Log("couroutine");
            yield return new WaitForSeconds(1f);
        }

        private void DoSpin(InputAction.CallbackContext obj)
        {
            if (coyoteFloat)
            {
                SpinJump();
                return;
            }
            
            if (!isAirborn) return;
            if (_canSpinJump != 0)
            {
                SpinJump();
            }
        }

        private void EndJump(InputAction.CallbackContext obj)
        {
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
            Movement();
            CalculateDeplacement();
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
            float characterVelocity = Mathf.Abs(rbCharacter.velocity.x); //N'EST PAS UNE DE MES FONCTIONS
            animator.SetFloat("Speed", characterVelocity); //N'EST PAS UNE DE MES FONCTIONS
        }
        
        private void JumpBuffer()
        {
            // Jump Buffer
            if (_jumpBuffer <= 0) return;
            
            _jumpBuffer -= 1 * Time.deltaTime;

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
                FastFall();
            }
        }
        
        private void CheckMove()
        {
            if ((direction.x < -0.1f) ||  (0.1f < direction.x))
            {
                Move();
                
                //ChangeAnimationState(PLAYER_RUN);// Tentative animator                //N'EST PAS UNE DE MES FONCTIONS
            }
            else
            {
                //Test Animator                                                         //N'EST PAS UNE DE MES FONCTIONS
                // if (direction.x == 0)                                                //N'EST PAS UNE DE MES FONCTIONS
                // {                                                                    //N'EST PAS UNE DE MES FONCTIONS
                //     ChangeAnimationState(PLAYER_IDLE);                               //N'EST PAS UNE DE MES FONCTIONS
                // }                                                                    //N'EST PAS UNE DE MES FONCTIONS

                AirSlowDown();
                
                //ChangeAnimationState(PLAYER_RUN);// Tentative animator            //N'EST PAS UNE DE MES FONCTIONS
            }
        }
        
        private void Move()
        {
            if(isEject) return;
            
            rbCharacter.drag = 0;
            rbCharacter.AddForce(new Vector2(_maxSpeed * direction.x * 10, 0f));

            Flip(rbCharacter.velocity.x); //Flip le joueur en fonction de sa vitesse  //N'EST PAS UNE DE MES FONCTIONS
        }

        private void ClampMove()
        {
            if(isEject) return;
            
            if (useRailSpeed)
            {
                if (rbCharacter.velocity.x < walkSpeed)
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
            _jumpDuration = 0;

            switch (jumpState)
            {
                case JumpState.ballon:
                    Debug.Log(jumpState);
                    GetComponentInChildren<Ballon>()?.JumpFromBallon();
                    if (direction.y < -0.9)
                    {
                        FallFromBallon();
                    }
                    else
                    {
                        Jump();
                    }
                    break;
                
                // case JumpState.plateforme:
                //     Debug.Log(jumpState);
                //     if (direction.y < -0.9)
                //     {
                //         StartCoroutine(FallFromPlateform());
                //     }
                //     else
                //     {
                //         Jump();
                //     }
                //     break;
                
                default:
                    Debug.Log(jumpState);
                    Jump();
                    break;
            }
        }

        private void Jump()
        {
            rbCharacter.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            animator.SetBool("isJumping", true);
            groundCheck.SetActive(false);
        }

        private void SpinJump()
        {
            rbCharacter.gravityScale = defaultGravityScale;
            isFalling = false;
            isAirborn = true;
            coyoteFloat = false;
            _jumpDuration = maxJumpDuration - spinJumpDuration;

            rbCharacter.velocity = new Vector2(rbCharacter.velocity.x, 0); //Arrêt de la montée et lissage de l'apex
            rbCharacter.AddForce(new Vector2(0, spinJumpForce * _canSpinJump), ForceMode2D.Impulse);

            animator.SetBool("IsSpinJumping", true); //N'EST PAS UNE DE MES FONCTIONS
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

        private IEnumerator FallFromPlateform()
        {
            GetComponent<BoxCollider2D>().enabled = false;
            yield return new WaitForSeconds(0.3f);
            GetComponent<BoxCollider2D>().enabled = true;
            StopCoroutine(FallFromPlateform());
        }

        private void FastFall()
        {
            if (!isAirborn) return;
            if (_falledFromBallon) return;

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
        #endregion

        private void OnDestroy()
        {
           Debug.Log("Disable");
          movement.Disable();
          farmerInputActions.Player.Jump.Disable();
          farmerInputActions.Player.SpinMove.Disable();
        }
    }
}