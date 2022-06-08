using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class DialogueTrigger : MonoBehaviour
{
    private InputActions farmerInputActions;

    public Dialogue dialogue;

    private bool isInRange;

    private GameObject interactUI;
    private GameObject Player;
    public bool isOpen;
    public bool isOnAir;
    public bool isOnAttraction;

    private void Awake()
    {
        farmerInputActions = new InputActions();
        interactUI = GameObject.Find("InteractText");
        Player = GameObject.Find("Player");
    }
         private void OnEnable()
     {
   /*      farmerInputActions.Player.PressB.performed += DoPressB;
         farmerInputActions.Player.PressB.Enable(); */

          farmerInputActions.Player.Jump.performed += DoDialogue;
          farmerInputActions.Player.Jump.Enable();
     }
 /*        private void DoPressB(InputAction.CallbackContext obj)
    {
        PressB();
    }
    private void PressB()                     
    {
         if(isInRange)
        {
            TriggerDialogue();
        }
    } */
    private void DoDialogue(InputAction.CallbackContext obj)
    {
        TheDialogue();
    }
    public void TheDialogue()                     
    {
         if(isInRange)
        {
            if(!isOpen)
            {
                if(isOnAttraction)
                {
                    DialogueManager.instance.isOpen1 = true;
                    StartCoroutine(ChargeDialogue());
                }
                else
                {
                 TriggerDialogue();
                 DialogueManager.instance.isOpen1 = true;
                }
            }
        }
    }

    void Update()
    {
     isOpen = DialogueManager.instance.isOpen1;
     isOnAir = Player.GetComponent<Cinemachine.PlayerInput>().isAirborn;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            isInRange = true;
            GameManage.instance.InteractOpenA();
        //    Player.GetComponent<Cinemachine.PlayerInput>().isEject = true
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = false;
            GameManage.instance.InteractCloseA();
            DialogueManager.instance.EndDialogue();
        }
    }
    private void TriggerDialogue()
    {
        GameManage.instance.InteractCloseA();
        DialogueManager.instance.StartDialogue(dialogue);
    }
        private IEnumerator ChargeDialogue()
    {
      yield return new WaitForSeconds(2.8f);
      TriggerDialogue();
    }
}