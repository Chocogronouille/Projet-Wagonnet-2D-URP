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
        private float _maxSpeed;
        private float _maxFallSpeed;
        private int _canSpinJump;

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

        public Animator animator;
        private float delaySpinJump = 0.35f;

        //private string currentState;
        public SpriteRenderer spriteRenderer;
        public SpriteRenderer screenRenderer;
        public Rigidbody2D rbCharacter;

        [HideInInspector] public Tween currentTween;


        [SerializeField] private float walkSpeed;
        [SerializeField] private float jumpForce;
        [SerializeField] private float spinJumpForce;
        [SerializeField] private float fallSpeed;
        [SerializeField] private float fastFallSpeed;
        [SerializeField] private int jumpBufferTime;
        [SerializeField] private float coyoteTime;
        [SerializeField] private float apexEndJump;
        [SerializeField] private float minJumpDuration;
        [SerializeField] private float maxJumpDuration;
        [SerializeField] private float spinJumpDuration;

        // Variable des Rails
        public bool isSurfing;
        private float waitTime = 0.0001f;
        private GameObject TheChild;
        
        #endregion

        void Awake()
        {
            TheChild = GameObject.Find("PlayerCollider");
            farmerInputActions = new InputActions();
            _maxFallSpeed = fallSpeed;

            #region singleton

            if (instance != null)
            {
                Debug.LogError("Il y a plusieurs instance de PlayerInput");
                return;
            }

            instance = this;

            #endregion

            animator = GetComponent<Animator>();
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
            gameObject.GetComponent<CinemachineDollyCart>().enabled = false;
            ApplyJumpForce(25);
        }

        // TODO SEE IF EQUILIBRAGE DE RAIL DIRECTION OR VOUS DEBUGUEZ LE DEPLACEMENT DEPUIS LA FRAME PRECEDENTE
        public void ApplyJumpForce(int jumpBonus = 0)
        {
            rbCharacter.AddForce(new Vector2(railDirection.x * railJump, railDirection.y * railJump + jumpBonus), ForceMode2D.Impulse);
        }

        private void DoSpin(InputAction.CallbackContext obj)
        {
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
            //         gameObject.transform.rotation = new Quaternion(0,0,0,0);
            if (isSurfing)
            {
                animator.SetBool("isSurfing", true);
                //        gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
                //        gameObject.GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Discrete;
                //        gameObject.GetComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.None;
            }
            else
            {
                // gameObject.transform.rotation = new Quaternion(0,0,0,0);
                animator.SetBool("isSurfing", false);
                // gameObject.GetComponent<Rigidbody2D>().gravityScale = defaultGravityScale;
                // gameObject.GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Continuous;
                // gameObject.GetComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.Interpolate;
            }

            //    gameObject.transform.rotation = new Quaternion(gameObject.transform.rotation.x,90,gameObject.transform.rotation.z,0);
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
            Jump();
            
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
                _maxSpeed = walkSpeed;
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
        

        // void ChangeAnimationState(string newState)                                    //N'EST PAS UNE DE MES FONCTIONS
        //  {                                                                            
        //      if (currentState == newState) return;                                    //N'EST PAS UNE DE MES FONCTIONS
        //      animator.Play(newState);                                                 //N'EST PAS UNE DE MES FONCTIONS
        //      currentState = newState;                                                 //N'EST PAS UNE DE MES FONCTIONS
        //  }

        private void Move()
        {
            rbCharacter.drag = 0;
            rbCharacter.AddForce(new Vector2(_maxSpeed * direction.x * 10, 0f));

            Flip(rbCharacter.velocity.x); //Flip le joueur en fonction de sa vitesse  //N'EST PAS UNE DE MES FONCTIONS
        }

        private void ClampMove()
        {
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

        private void Jump()
        {
            rbCharacter.gravityScale = defaultGravityScale;
            rbCharacter.drag = 0;
            isFalling = false;
            isAirborn = true;
            _jumpBuffer = 0;
            _jumpDuration = 0;

            rbCharacter.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            animator.SetBool("isJumping", true); //N'EST PAS UNE DE MES FONCTIONS

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

        private void FastFall()
        {
            if (!isAirborn) return;

            _maxFallSpeed = fastFallSpeed;
        }

        public void ResetSpinJump()
        {
            _canSpinJump = numberOfSpinJump;
        }

        #endregion
        

        #region FonctionsAnimator

        void Flip(float velocity) //Fonction utilisée pour changer le sens du sprite du personnage
        {
            if (velocity > 0.1f) //Si le joueur va vers la droite
            {
                spriteRenderer.flipX = false; //On garde le sprite dans son orientation de base (vers la droite)
                screenRenderer.flipX = false;
            }
            else if (velocity < -0.1f) //Si le joueur va vers la gauche
            {
                spriteRenderer.flipX = true; //On oriente le sprite vers la gauche
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
    }
}