using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Cassette : MonoBehaviour
{
    
    private InputActions farmerInputActions;



    public int currentCount;
    public InteractBar interactBar;



//    private InputActions farmerInputActions;
    
     private void Awake()
     
    {
        interactBar = GameObject.FindGameObjectWithTag("InteractBar").GetComponent<InteractBar>();
        //farmerInputActions = new InputActions();
    }
    
     /*
      private void OnEnable()

     {
         farmerInputActions.Player.PressB.performed += DoPressB;
         farmerInputActions.Player.PressB.Enable();
     }
     */
    private void Start()
    {
        currentCount = 0;
        interactBar.SetCount(currentCount);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            InteractCounter.instance.AddCounter(1);
            currentCount = currentCount + 1;
            interactBar.SetCount(currentCount);
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;

        }
    }
    
}
