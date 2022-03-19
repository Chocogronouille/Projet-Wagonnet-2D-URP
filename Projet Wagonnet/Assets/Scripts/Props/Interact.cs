using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Interact : MonoBehaviour
{
    
    private InputActions farmerInputActions;



    public int currentCount;
    public InteractBar interactBar;
    public bool isColliding;
    
    
    
//    private InputActions farmerInputActions;
    
     private void Awake()
     
    {
        interactBar = GameObject.FindGameObjectWithTag("InteractBar").GetComponent<InteractBar>();
        farmerInputActions = new InputActions();
    }
    
     private void OnEnable()
     {
         farmerInputActions.Player.PressB.performed += DoPressB;
         farmerInputActions.Player.PressB.Enable();
     }
    private void Start()
    {
        currentCount = 0;
        interactBar.SetCount(currentCount);
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
            InteractCounter.instance.AddCounter(1);
            currentCount = currentCount + 1;
            interactBar.SetCount(currentCount);
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<Interact>().enabled = false;

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isColliding = false;
    }
}
