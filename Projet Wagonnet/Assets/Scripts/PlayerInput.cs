using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private InputActions farmerInputActions;
    private InputAction movement;
    private int _jumpBuffer;

    public static PlayerInput instance; // singleton
    public float walkSpeed;
    public bool isAirborn;
    public bool coyoteFloat;

    [SerializeField] private Rigidbody2D rbCharacter;
    [SerializeField] private float jumpForce;
    [SerializeField] private float fastFallSpeed;
    [SerializeField] private int jumpBufferTime;
    [SerializeField] private float coyoteTime;
    public Vector2 Direction;
    
    void Awake()
    {
       farmerInputActions = new InputActions();
       
       #region singleton
       if (instance != null)
       {
           Debug.LogError("Il y a plusieurs instance de PlayerInput1");
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
    }

    private void DoJump(InputAction.CallbackContext obj)
    {
        _jumpBuffer = jumpBufferTime;
    }

    private void Jump()                     //Lorsque le personnage saute, on lui applique une force vers le haut
    {
        isAirborn = true;
        _jumpBuffer = 0; 
        rbCharacter.AddForce(new Vector2(0,jumpForce),ForceMode2D.Impulse);
    }
    
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
            if (rbCharacter.velocity.y < 0) //Si le personnage commence à tomber, on lance la coroutine CoyoteTime
            {
                if (isAirborn == false)
                {
                    coyoteFloat = true;
                    StartCoroutine(CoyoteTime());
                }
            }
        }

        if (Direction.y < -0.7f)            //Lorsque le joystick est orienté vers le bas, on lance la FastFall
        {
            FastFall();
        } 
    }

    void Move()                             //Lorsque le personnage se déplace, on lui applique une vitesse dans le sens de son joystick
    {
        rbCharacter.velocity = new Vector2(walkSpeed * Direction.x, rbCharacter.velocity.y);
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
        StopCoroutine(CoyoteTime());
    }
}

