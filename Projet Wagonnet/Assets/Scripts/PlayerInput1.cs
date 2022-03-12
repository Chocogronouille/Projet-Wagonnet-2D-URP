using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput1 : MonoBehaviour
{
    public static PlayerInput1 instance; // singleton
    private InputActions farmerInputActions;
    private InputAction movement;
    public Vector2 Direction;

    public float walkSpeed;
    private int _jumpBuffer;

    public Rigidbody2D rbCharacter;
    [SerializeField] private float jumpForce;
    [SerializeField] private float fastFallSpeed;
    [SerializeField] private bool isAirborn;
    [SerializeField] private bool coyoteFloat;
    [SerializeField] private int jumpBufferTime;
    [SerializeField] private float coyoteTime;

    //public Vector2 newPosition= new Vector2(0.3f, 2.6f);
    
    // Start is called before the first frame update
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
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("DeathZoneActivé");
        transform.position = new Vector3(0,3, 0);
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
   //   Jump();
      _jumpBuffer = jumpBufferTime;
    }

    private void Jump()
    {
      Debug.Log("Jump!!");
      isAirborn = true;
      rbCharacter.AddForce(new Vector2(0,jumpForce),ForceMode2D.Impulse);

    /*  
    _jumpBuffer = jumpBufferTime;
    */
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
                    Debug.Log("Tombe");
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
        rbCharacter.velocity = new Vector2(rbCharacter.velocity.x, -fastFallSpeed);
    }

     void OnCollisionEnter2D(Collision2D other)
    {
        StopCoroutine(CoyoteTime());
        isAirborn = false;
        coyoteFloat = false;
        Debug.Log("Landed");
    }

    IEnumerator CoyoteTime()                //Coroutine du coyote time
    {
        Debug.Log("CoyoteTime");
        yield return new WaitForSeconds(coyoteTime);
        isAirborn = true;
        Debug.Log(isAirborn);
        StopCoroutine(CoyoteTime());
    }
    }

