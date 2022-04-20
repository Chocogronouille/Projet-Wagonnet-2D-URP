using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Cassette : MonoBehaviour
{
    
//    private InputActions farmerInputActions;



    public int currentCassetteCount;
   // public InteractBar interactBar;



//    private InputActions farmerInputActions;
    
     private void Awake()
     
    {
       // interactBar = GameObject.FindGameObjectWithTag("InteractBar").GetComponent<InteractBar>();
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
        currentCassetteCount = 0;
       // interactBar.SetCount(currentCount);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManage.instance.CassetteOpen();
            CounterCassette.instance.AddCounterCassette(1);
            currentCassetteCount = currentCassetteCount + 1;
          //  interactBar.SetCount(currentCount);
        //    GetComponent<SpriteRenderer>().enabled = false;
          //  GetComponent<BoxCollider2D>().enabled = false;
          Destroy(gameObject,0.00001f);

        }
    }

}
