using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    public int currentCount;
    public InteractBar interactBar;
    
    
//    private InputActions farmerInputActions;
    
     private void Awake()
     
    {
        interactBar = GameObject.FindGameObjectWithTag("InteractBar").GetComponent<InteractBar>();
    }
    
    
    private void Start()
    {
        currentCount = 0;
        interactBar.SetCount(currentCount);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) //&& (Input.GetKeyDown(KeyCode.E)))
        {
            InteractCounter.instance.AddCounter(1);
            currentCount = currentCount + 1;
            interactBar.SetCount(currentCount);
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            
        }
    }
}
