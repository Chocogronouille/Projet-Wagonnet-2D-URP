using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{

    private InputActions farmerInputActions;
    private InputAction movement;
    public Vector2 Direction;

    public float walkSpeed;
    private int _jumpBuffer;
    public bool isAirborn;
    public bool coyoteFloat;

    [SerializeField] private Rigidbody2D rbCharacter;
    [SerializeField] private float jumpForce;
    [SerializeField] private float fastFallSpeed;
    [SerializeField] private int jumpBufferTime;
    [SerializeField] private float coyoteTime;
    
    void Awake()
    {
       farmerInputActions = new InputActions();    
    }

    private void OnEnable()
    {
       movement = farmerInputActions.Player.Movement;
       movement.Enable();

       farmerInputActions.Player.Jump.performed += DoJump;
       farmerInputActions.Player.Jump.Enable();
    }

    private void DoJump(InputAction.CallbackContext obj)
    {
        _jumpBuffer = jumpBufferTime;
    }

    private void Jump()
    {
        isAirborn = true;
        _jumpBuffer = 0; 
        rbCharacter.AddForce(new Vector2(0,jumpForce),ForceMode2D.Impulse);
    }
    
    

    // Update is called once per frame
    void Update()
    {
       Direction = movement.ReadValue<Vector2>();
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
        if (coyoteFloat == false)
        {
            if (rbCharacter.velocity.y < 0)
            {
                if (isAirborn == false)
                {
                    coyoteFloat = true;
                    StartCoroutine(CoyoteTime());
                }
            }
        }

        if (Direction.y < -0.7f)
        {
            FastFall();
        } 
    }

         void Move()
    {
        rbCharacter.velocity = new Vector2(walkSpeed * Direction.x, rbCharacter.velocity.y);
    }
       
    void FastFall()
    {
        if (isAirborn)
        {
            rbCharacter.velocity = new Vector2(rbCharacter.velocity.x, -fastFallSpeed);
        }
    }

    public IEnumerator CoyoteTime()                //Coroutine du coyote time
    {
        yield return new WaitForSeconds(coyoteTime);
        isAirborn = true;
        coyoteFloat = false;
        StopCoroutine(CoyoteTime());
    }
}

