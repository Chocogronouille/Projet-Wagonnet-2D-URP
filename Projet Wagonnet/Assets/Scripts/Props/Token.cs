using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Token : MonoBehaviour
{
    public AudioClip sound;
    
//    private InputActions farmerInputActions;



    public int currentTokenCount;
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
        currentTokenCount = 0;
       // interactBar.SetCount(currentCount);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // GameManage.instance.CountAnim.SetBool("isCassCount", true);
            // GameManage.instance.CassetteOpen();
       //    CounterCassette.instance.AddCounterCassette(1);
            StartCoroutine(ChangeNumber());
            currentTokenCount = currentTokenCount + 1;
          //  interactBar.SetCount(currentCount);
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
      //    Destroy(gameObject,0.00001f);
            AudioManager.instance.PlayClipAt(sound, transform.position);
        }
    }
    IEnumerator ChangeNumber()
    {
        // yield return new WaitForSeconds(1f);
        // GameManage.instance.CountAnim.SetBool("isCassCount", false);
        yield return new WaitForSeconds(0.43f);
        CounterToken.instance.AddCounterToken(1);
        Destroy(gameObject,0.00001f);
    }

}
