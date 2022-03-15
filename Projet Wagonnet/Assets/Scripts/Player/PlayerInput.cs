using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInput : MonoBehaviour
    {
        private InputActions farmerInputActions;
        private int _jumpBuffer;
    
        public static PlayerInput instance; // singleton
        public InputAction movement;
        public float walkSpeed;
        public bool isAirborn;
        public bool coyoteFloat;
        public bool canSpinJump;
        public Vector2 direction;

        [SerializeField] private Rigidbody2D rbCharacter;
        [SerializeField] private float jumpForce;
        [SerializeField] private float spinJumpForce;
        [SerializeField] private float fastFallSpeed;
        [SerializeField] private int jumpBufferTime;
        [SerializeField] private float coyoteTime;

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
            farmerInputActions.Player.Jump.Enable();
            
            farmerInputActions.Player.SpinMove.performed += DoSpin;
            farmerInputActions.Player.SpinMove.Enable();
        }

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
    
        void Update()
        {
            direction = movement.ReadValue<Vector2>();
            Move();
        
            // Jump Buffer
            if (_jumpBuffer != 0)               //Si la touche de saut a été enfoncée, on décompte les frames de jump buffer
            {
                _jumpBuffer -= 1;
                if (isAirborn == false)
                {
                    Jump();                     //Si la touche de saut a été enfoncée dans les temps et que le personnage n'est pas en l'air, il saute
                } 
            } 

            // Coyote Time
            if (isAirborn == false)
            {
                if (rbCharacter.velocity.y < 0) //Si le personnage commence à tomber, on lance la coroutine CoyoteTime
                {
                    if (coyoteFloat == false)
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

        void Move()                             //Lorsque le personnage se déplace, on lui applique une vitesse dans le sens de son joystick
        {
            rbCharacter.velocity = new Vector2(walkSpeed * direction.x, rbCharacter.velocity.y);
        }
    
        private void Jump()                     //Lorsque le personnage saute, on lui applique une force vers le haut
        {
            isAirborn = true;
            canSpinJump = true;
            _jumpBuffer = 0; 
            rbCharacter.AddForce(new Vector2(0,jumpForce),ForceMode2D.Impulse);
        }

        private void SpinJump()
        {
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

        public IEnumerator CoyoteTime()                         //Coroutine du coyote time
        {                                                       //On attend X secondes avant de considérer le joueur comme en l'air
            yield return new WaitForSeconds(coyoteTime);
            isAirborn = true;
            coyoteFloat = false;
            canSpinJump = true;
            StopCoroutine(CoyoteTime());
        }
    }
}

