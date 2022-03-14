using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine.Utility;

namespace Cinemachine
{
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


public class Déactive : MonoBehaviour
{

    private InputActions farmerInputActions;
    private InputAction movement;
    public Vector2 Direction;

    public float walkSpeed;
    private int _jumpBuffer;
    

     [SerializeField] private Rigidbody2D rbCharacter;
    [SerializeField] private float jumpForce;
    [SerializeField] private float fastFallSpeed;
    [SerializeField] private bool isAirborn;
    [SerializeField] private bool coyoteFloat;
    [SerializeField] private int jumpBufferTime;
    [SerializeField] private float coyoteTime;

    public GameObject Player;

    public Transform other;
    public GameObject track;
    public float dist;
   // public Vector3 trackPos;

    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {
      /*  trackPos = track.GetComponent<CinemachinePath>().Waypoint[0].position; */
        dist = Vector3.Distance(other.position, transform.position);

        Direction = movement.ReadValue<Vector2>();
        Move();

     /*   if(Input.GetKeyDown(KeyCode.D))
        {
            Player.GetComponent<CinemachineDollyCart>().enabled=false;
        } */
    }

    private void DoJump(InputAction.CallbackContext obj)
    {
   //   Jump();
   //   _jumpBuffer = jumpBufferTime;
   rbCharacter.AddForce(new Vector2(0,jumpForce),ForceMode2D.Impulse);
   Debug.Log("Hi");
   Player.GetComponent<CinemachineDollyCart>().enabled=false;
    }

 /*  private void DoJump()
    {
      Debug.Log("Jump!!");
      isAirborn = true;
      rbCharacter.AddForce(new Vector2(0,jumpForce),ForceMode2D.Impulse);
    } */
     void Move()
    {
        rbCharacter.velocity = new Vector2(walkSpeed * Direction.x, rbCharacter.velocity.y);
    }
}
}
