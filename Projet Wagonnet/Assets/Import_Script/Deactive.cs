using System.Collections;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

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


public class Deactive : MonoBehaviour
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

    public bool isSurfing;
    private float waitTime = 0.0001f;
    private GameObject TheChild;
    private float ReplaceSpeed = 1;
    public float LaRotation;
    public bool isDetect;
    // Start is called before the first frame update
    void Awake()
    {
       farmerInputActions = new InputActions();    
       TheChild = transform.GetChild(0).gameObject;
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
        Direction = movement.ReadValue<Vector2>();
        Move(); 

     /*   if(Input.GetKeyDown(KeyCode.D))
        {
            Player.GetComponent<CinemachineDollyCart>().enabled=false;
        } */

        if(isSurfing)
        {
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        }
        else
        {
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
        }
    }

    private void DoJump(InputAction.CallbackContext obj)
    {
   //   Jump();
   //   _jumpBuffer = jumpBufferTime;
   gameObject.GetComponent<CinemachineDollyCart>().enabled=false;
   rbCharacter.AddForce(new Vector2(0,jumpForce),ForceMode2D.Impulse);
   StartCoroutine(LeJump(waitTime));
 //  rbCharacter.AddForce(new Vector2(0,jumpForce),ForceMode2D.Impulse);
    }

   private void DoJump()
    {
      Debug.Log("Jump!!");
      isAirborn = true;
      rbCharacter.AddForce(new Vector2(0,jumpForce),ForceMode2D.Impulse);
      
    } 
     void Move()
    {
        rbCharacter.velocity = new Vector2(walkSpeed * Direction.x, rbCharacter.velocity.y);
    } 

            IEnumerator LeJump(float waitTime)
    {
        gameObject.transform.rotation = new Quaternion(0.0f,90,0.0f,90);
        yield return new WaitForSeconds(waitTime);
        isSurfing = false;
        yield return new WaitForSeconds(waitTime + 0.2f);
        TheChild.GetComponent<BoxCollider2D>().enabled = true;
    }
}
}
