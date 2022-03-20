using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInput : MonoBehaviour
    {
        private InputActions farmerInputActions;
        private int _jumpBuffer;
        
        public bool isFalling;
        public static PlayerInput instance; // singleton
        public InputAction movement;
        public float walkSpeed;
        public bool isAirborn;
        public bool coyoteFloat;
        public bool canSpinJump;
        public Vector2 direction;
        public float apexThreshold;

        public Animator animator;
        public SpriteRenderer spriteRenderer;
        public Rigidbody2D rbCharacter;
        
        [SerializeField] private float jumpForce;
        [SerializeField] private float spinJumpForce;
        [SerializeField] private float fastFallSpeed;
        [SerializeField] private int jumpBufferTime;
        [SerializeField] private float coyoteTime;
        [SerializeField] private float apexEndJump;
        [SerializeField] private float fallGravityScale;
        [SerializeField] private float startFallSpeedThreshold;
        [SerializeField] private float defaultGravityScale;

        private float m_lastDirectionX;
        private float lastDirectionX      //Si la position du joystick est différente de la dernière position enregistrée, on actualise la vitesse du personnage
        {
            get => m_lastDirectionX;
            set
            {
                m_lastDirectionX = value;
                Move();
            }
        }
        
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
                if (rbCharacter.velocity.y > apexThreshold)
                {
                    isFalling = true;
                    rbCharacter.gravityScale = fallGravityScale;
                    rbCharacter.velocity = new Vector2(rbCharacter.velocity.x,0f);
                    rbCharacter.AddForce(new Vector2(0,apexEndJump),ForceMode2D.Impulse); 
                }
            }
            else
            {
                _jumpBuffer = 0;
            }
        }

        #endregion
        
        void Update()
        {
            direction = movement.ReadValue<Vector2>();
            lastDirectionX = direction.x;       //On enregistre la position du joystick en x
        
            // Jump Buffer
            if (_jumpBuffer != 0)               //Si la touche de saut a été enfoncée, on décompte les frames de jump buffer
            {
                _jumpBuffer -= 1;
                if (isAirborn == false)
                {
                    Jump();                     //Si la touche de saut a été enfoncée dans les temps et que le personnage n'est pas en l'air, il saute
                } 
            }

            // Gestion de la vitesse de chute
            if (!isFalling)
            {
                if (isAirborn)
                {
                    if (rbCharacter.velocity.y < startFallSpeedThreshold)
                    {
                        isFalling = true;
                        rbCharacter.gravityScale = fallGravityScale;
                    }
                }
            }
            
            // Coyote Time
            if (!isAirborn)
            {
                if (rbCharacter.velocity.y < 0) //Si le personnage commence à tomber, on lance la coroutine CoyoteTime
                {
                    if (!coyoteFloat)
                    {
                        coyoteFloat = true;
                        StartCoroutine(CoyoteTime());
                    }
                }
            }

            if (direction.y < -0.7f)            //Lorsque le joystick est orienté vers le bas, on lance la FastFall
            {
                FastFall();
            } 
        }

        #region FonctionsDéplacements

        void Move()                             //Lorsque le personnage se déplace, on lui applique une vitesse dans le sens de son joystick
        {
            rbCharacter.velocity = new Vector2(walkSpeed*lastDirectionX, rbCharacter.velocity.y);
            
            Flip(rbCharacter.velocity.x);                                   //Flip le joueur en fonction de sa vitesse
            float characterVelocity = Mathf.Abs(rbCharacter.velocity.x);    //prendre la valeur positive de vitesse
            animator.SetFloat("Speed", characterVelocity);              // animator
        }
        
        private void Jump()                     //Lorsque le personnage saute, on lui applique une force vers le haut
        {
            rbCharacter.gravityScale = defaultGravityScale;
            isFalling = false;
            isAirborn = true;
            canSpinJump = true;
            _jumpBuffer = 0; 
            rbCharacter.AddForce(new Vector2(0,jumpForce),ForceMode2D.Impulse);
        }

        private void SpinJump()
        {
            rbCharacter.gravityScale = defaultGravityScale;
            isFalling = false;
            isAirborn = true;
            coyoteFloat = false;
            canSpinJump = false;
            rbCharacter.velocity = new Vector2(0, 0);
            rbCharacter.AddForce(new Vector2(0,spinJumpForce),ForceMode2D.Impulse);
        }
       
        void FastFall()                         //Lorsque le personnage est en FastFall, on lui applique une vitesse vers le bas
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
            yield return new WaitForSeconds(coyoteTime);
            isAirborn = true;
            coyoteFloat = false;
            StopCoroutine(CoyoteTime());
        }

        #endregion
    }
}

