using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Cassette : MonoBehaviour
{
    public AudioClip sound;
    
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
            GameManage.instance.CountAnim.SetBool("isCassCount", true);
            GameManage.instance.CassetteOpen();
       //    CounterCassette.instance.AddCounterCassette(1);
            StartCoroutine(ChangeNumber(collision));
            currentCassetteCount = currentCassetteCount + 1;
          //  interactBar.SetCount(currentCount);
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
      //    Destroy(gameObject,0.00001f);
            AudioManager.instance.PlayClipAt(sound, transform.position);
        }
    }
    IEnumerator ChangeNumber(Collider2D collision)
    {
        var rb = collision.GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        yield return new WaitForSeconds(1f);
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        GameManage.instance.CountAnim.SetBool("isCassCount", false);
        yield return new WaitForSeconds(0.43f);
        CounterCassette.instance.AddCounterCassette(1);
        Destroy(gameObject,0.00001f);
    }

}
