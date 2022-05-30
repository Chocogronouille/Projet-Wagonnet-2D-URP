using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class DialogueTrigger : MonoBehaviour
{
    private InputActions farmerInputActions;

    public Dialogue dialogue;

    private bool isInRange;

    private GameObject interactUI;
    private GameObject Player;
    public bool isOpen;
    public bool isOnAir;

    public static DialogueTrigger instance;

    private void Awake()
    {
          if(instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de DialogueManager dans la scène");
            return;
        }

        instance = this;
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
    private void TheDialogue()                     
    {
         if(isInRange)
        {
            if(!isOpen)
            {
              TriggerDialogue();
              isOpen = true;
            }
        }
    }

    void Update()
    {
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

    void TriggerDialogue()
    {
        GameManage.instance.InteractCloseA();
        DialogueManager.instance.StartDialogue(dialogue);
    }
}