using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Attraction : MonoBehaviour
{
    
    private InputActions farmerInputActions;



    public int currentAttractionCount;
   // public InteractBar interactBar;
    public bool isColliding;
    
    
    
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
            CounterAttraction.instance.AddCounterAttraction(1);
            currentAttractionCount = currentAttractionCount + 1;
           // interactBar.SetCount(currentAttractionCount);
            GetComponent<BoxCollider2D>().enabled = false;

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isColliding = false;
    }
}
