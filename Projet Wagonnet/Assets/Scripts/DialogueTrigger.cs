using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class DialogueTrigger : MonoBehaviour
{
    private InputActions farmerInputActions;

    public Dialogue dialogue;

    private bool isInRange;

    private GameObject interactUI;

    private void Awake()
    {
        farmerInputActions = new InputActions();
        interactUI = GameObject.Find("InteractText");
    }
         private void OnEnable()
     {
         farmerInputActions.Player.PressB.performed += DoPressB;
         farmerInputActions.Player.PressB.Enable();
     }
         private void DoPressB(InputAction.CallbackContext obj)
    {
        PressB();
    }
    private void PressB()                     
    {
         if(isInRange)
        {
            TriggerDialogue();
        }
        Debug.Log("PressB");
    }

    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            isInRange = true;
            GameManage.instance.InteractOpen();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = false;
            GameManage.instance.InteractClose();
            DialogueManager.instance.EndDialogue();
        }
    }

    void TriggerDialogue()
    {
        DialogueManager.instance.StartDialogue(dialogue);
    }
}