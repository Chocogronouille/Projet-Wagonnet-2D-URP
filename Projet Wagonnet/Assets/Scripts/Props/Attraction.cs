using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using PlayerInput = Cinemachine.PlayerInput;

//using UnityEditor.Experimental.GraphView;


public class Attraction : MonoBehaviour
{
    
    private InputActions farmerInputActions;
    public Animator animator;


    [SerializeField] private GameObject player;
    public int currentAttractionCount;
   // public InteractBar interactBar;
    public bool isColliding;
    
    public CinemachineVirtualCamera CameraAttraction; //GroupCamera

    
    
    
//    private InputActions farmerInputActions;
    
     private void Awake()
     
    {
       // interactBar = GameObject.FindGameObjectWithTag("InteractBar").GetComponent<InteractBar>();
        farmerInputActions = new InputActions();
    }
    
     private void OnEnable()
     {
         farmerInputActions.Player.PressB.performed += DoPressB;
         farmerInputActions.Player.PressB.Enable();
     }
    private void Start()
    {
        currentAttractionCount = 0;
        //interactBar.SetCount(currentAttractionCount);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isColliding = true;

        }
    }
    private void DoPressB(InputAction.CallbackContext obj)
    {
        PressB();
    }
    
    private void PressB()                     
    {
        if (isColliding == true)
        {
            GameManage.instance.CountAnim.SetBool("isAttraCount", true);
            StartCoroutine(Activation());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isColliding = false;
    }

    IEnumerator Activation()
    {
        animator.SetFloat("Speed", 0);
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.GetComponent<PlayerInput>().enabled = false;
        CameraAttraction.Priority = 5;
        yield return new WaitForSeconds(1f);
        GameManage.instance.CountAnim.SetBool("isAttraCount", false);
        yield return new WaitForSeconds(0.4f);
        CounterAttraction.instance.AddCounterAttraction(1);
        currentAttractionCount = currentAttractionCount + 1;
        // interactBar.SetCount(currentAttractionCount);
        GetComponent<BoxCollider2D>().enabled = false;
        CameraAttraction.Priority = 0;
        player.GetComponent<PlayerInput>().enabled = true;


    }
}
