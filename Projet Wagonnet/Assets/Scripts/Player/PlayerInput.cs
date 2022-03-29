using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    
    public class PlayerInput : MonoBehaviour
    {
        private InputActions farmerInputActions;
        private float _jumpBuffer;
        private float _horizontalSpeed;
        private float _jumpFrameCount;
        private bool _wantToEndJump;
        private float _maxSpeed;
        
        public bool isFalling;
        public static PlayerInput instance; // singleton
        public InputAction movement;
        public bool isAirborn;
        public bool coyoteFloat;
        public bool canSpinJump;
        public Vector2 direction;
        public float apexThreshold;
        public float defaultGravityScale;


        public bool isSurfing;
        private float waitTime = 0.0001f;
        private GameObject TheChild;
        

        public Animator animator;
        //private string currentState;
        public SpriteRenderer spriteRenderer;
        public Rigidbody2D rbCharacter;
        
        //Animation States
        private const string PLAYER_IDLE = "Player_Idle";
        private const string PLAYER_RUN = "Player_Run";
        private const string PLAYER_SAUTRISE = "Player_SautRise";
        private const string PLAYER_SAUTFALL = "Player_SautFall";
        private const string PLAYER_SPINJUMP = "Player_SpinJump";
        private const string PLAYER_SURF = "Player_Surf";
        

        [SerializeField] private float walkSpeed;
        [SerializeField] private float airStopSpeed;
        [SerializeField] private float jumpForce;
        [SerializeField] private float spinJumpForce;
        [SerializeField] private float fastFallSpeed;
        [SerializeField] private int jumpBufferTime;
        [SerializeField] private float coyoteTime;
        [SerializeField] private float apexEndJump;
        [SerializeField] private float minJumpEndFrame;
        [SerializeField] private float maxJumpEndFrame;

        void Awake()
        {
            farmerInputActions = new InputActions();
       
            #region singleton
            if (instance != null)
            {
                Debug.LogError("Il y a plusieurs instance de PlayerInput");
                return;
            }
            instance = this;
            #endregion

            animator = GetComponent<Animator>();

            TheChild = transform.GetChild(0).gameObject;
            
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

        private void DoJump(InputAction.CallbackContext obj)
        {
            _jumpBuffer = jumpBufferTime;
            
            gameObject.GetComponent<CinemachineDollyCart>().enabled=false;
            rbCharacter.AddForce(new Vector2(0,jumpForce),ForceMode2D.Impulse);
            StartCoroutine(LeJump(waitTime));
        }

        private void DoSpin(InputAction.CallbackContext obj)
        {
            if (canSpinJump)
            {
                SpinJump();
            }
        }

        private void EndJump(InputAction.CallbackContext obj)
        {
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

        private void FixedUpdate()
        {
            direction = movement.ReadValue<Vector2>();
            float characterVelocity = Mathf.Abs(rbCharacter.velocity.x);    //prendre la valeur positive de vitesse
            animator.SetFloat("Speed", characterVelocity); 
        
            // Jump Buffer
            if (_jumpBuffer > 0)               //Si la touche de saut a été enfoncée, on décompte les frames de jump buffer
            {
                _jumpBuffer -= 1*Time.deltaTime;
                if (isAirborn == false)
                {
                    Jump();                     //Si la touche de saut a été enfoncée dans les temps et que le personnage n'est pas en l'air, il saute
                } 
            }

            // Gestion de la vitesse de chute et du nuancier de saut
            if (!isFalling)
            {
                if (isAirborn)
                {
                    _jumpFrameCount += 1*Time.deltaTime;
                    Debug.Log(_jumpFrameCount);
                    
                    // if (rbCharacter.velocity.y < startFallSpeedThreshold) // Acceleration de la chute
                    // {
                    //     Fall();
                    // }
                    
                    if (_jumpFrameCount > maxJumpEndFrame)           // Fin du saut max
                    {
                        Fall();
                    }

                    if (_wantToEndJump)                                      // Fin du saut en cours de montée
                    {
                        if (rbCharacter.velocity.y > apexThreshold)
                        {
                            if (_jumpFrameCount > minJumpEndFrame)
                            {
                                Fall(); 
                            }
                        }  
                    }
                    
                }
            }
            
            
            // Coyote Time
            if (!isAirborn)
            {
                if (rbCharacter.velocity.y < -1) //Si le personnage commence à tomber, on lance la coroutine CoyoteTime
                {
                    if (!coyoteFloat)
                    {
                        coyoteFloat = true;
                        StartCoroutine(CoyoteTime());
                    }
                }
            }

            if (direction.y < -0.9f)            //Lorsque le joystick est orienté vers le bas, on lance la FastFall
            {
                FastFall();
            }

            if ((direction.x < -0.1f)||(0.1f<direction.x))
            {
                _maxSpeed = walkSpeed;
                Move();
                //ChangeAnimationState(PLAYER_RUN);// Tentative animator


            }
            else
            {
                if (isAirborn)
                {
                    
                    _maxSpeed = airStopSpeed;
                    Move();
                    //ChangeAnimationState(PLAYER_RUN);// Tentative animator


                }

                //Test Animator
                // if (direction.x == 0)
                // {
                //     ChangeAnimationState(PLAYER_IDLE);
                // }
                
            }
        }

        #region FonctionsDéplacements

       // void ChangeAnimationState(string newState)
       //  {
       //      if (currentState == newState) return;
       //      
       //      animator.Play(newState);
       //      
       //      currentState = newState;
       //
       //  }
        
        private void Move()                             //Lorsque le personnage se déplace, on lui applique une vitesse dans le sens de son joystick
        {
            rbCharacter.drag = 0;
            rbCharacter.AddForce(new Vector2(_maxSpeed*direction.x*10,0f));
            _horizontalSpeed = Mathf.Clamp(rbCharacter.velocity.x, -_maxSpeed, _maxSpeed);
            rbCharacter.velocity = new Vector2(_horizontalSpeed, rbCharacter.velocity.y);
            
            Flip(rbCharacter.velocity.x);                                   //Flip le joueur en fonction de sa vitesse
        }
        
        private void Jump()                     //Lorsque le personnage saute, on lui applique une force vers le haut
        {
            rbCharacter.gravityScale = defaultGravityScale;
            rbCharacter.drag = 0;
            isFalling = false;
            isAirborn = true;
            canSpinJump = true;
            _jumpBuffer = 0;
            _jumpFrameCount = 0;
            rbCharacter.AddForce(new Vector2(0,jumpForce),ForceMode2D.Impulse);
            animator.SetBool("isJumping", true);
        }

        private void SpinJump()
        {
            rbCharacter.gravityScale = defaultGravityScale;
            isFalling = false;
            isAirborn = true;
            coyoteFloat = false;
            canSpinJump = false;
            rbCharacter.velocity = new Vector2(rbCharacter.velocity.x,0);
            rbCharacter.AddForce(new Vector2(0,spinJumpForce),ForceMode2D.Impulse);
            animator.SetBool("IsSpinJump", true);

        }

        public void Fall()
        {
            isFalling = true;
            _wantToEndJump = false;
            rbCharacter.gravityScale = defaultGravityScale;
            
            rbCharacter.velocity = new Vector2(rbCharacter.velocity.x,0f);
            rbCharacter.AddForce(new Vector2(0,apexEndJump),ForceMode2D.Impulse);
            animator.SetBool("IsFalling", true);
            animator.SetBool("isJumping", false);

        }

        private void FastFall()                         //Lorsque le personnage est en FastFall, on lui applique une vitesse vers le bas
        {
            if (isAirborn)
            {
                rbCharacter.velocity = new Vector2(rbCharacter.velocity.x, -fastFallSpeed);
            }
        }

        #endregion

        #region FonctionsAnimator

        void Flip(float velocity)
        {
            if (velocity > 0.1f)
            {
                spriteRenderer.flipX = false;
            }
            else if (velocity < -0.1f)
            {
                spriteRenderer.flipX = true;
            }
        }

        #endregion

        #region Coroutine

        private IEnumerator CoyoteTime()                        //Coroutine du coyote time
        {                                                       //On attend X secondes avant de considérer le joueur comme en l'air
            canSpinJump = true;
            rbCharacter.drag = 0;
            yield return new WaitForSeconds(coyoteTime);
            isAirborn = true;
            coyoteFloat = false;
            isFalling = true;
            rbCharacter.gravityScale = defaultGravityScale;
            StopCoroutine(CoyoteTime());
        }

        #endregion
    }
}

