using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Cinematique : MonoBehaviour
{
    private InputActions farmerInputActions;
    private GameObject CineDebut;
    private GameObject CineMillieu;
    private GameObject CineFin;
    private GameObject Player;
    private GameObject GameManager;
    private bool isTalk;

    private Dialogue dialogue1;
    private void Awake()
    {
        GameManager = GameObject.Find("GameManager");
        farmerInputActions = new InputActions();
        Player = GameObject.Find("Player");
    }
    private void OnEnable()
        {

            farmerInputActions.Player.Jump.performed += DoCinema;
            farmerInputActions.Player.Jump.Enable();
        }
        private void DoCinema(InputAction.CallbackContext obj)
        {
            if(CineDebut.activeInHierarchy == true)
            {
                if(isTalk)
                {
                CineDebut.SetActive(false);
                CineMillieu.SetActive(true);
                DialogueManager.instance.DisplayNextSentence();
                }
                else
                {
                DialogueManager.instance.DNSButton();
                }
            }
            else if(CineMillieu.activeInHierarchy == true)
            {
                 if(isTalk)
                {
                CineMillieu.SetActive(false);
                CineFin.SetActive(true);
                DialogueManager.instance.DisplayNextSentence();
                }
                else
                {
                DialogueManager.instance.DNSButton();
                }
            }
            else if(CineFin.activeInHierarchy == true)
            {
                if(isTalk)
                {
                GameManager.GetComponent<GameManage>().StartGame();
                }
                else
                {
                DialogueManager.instance.DNSButton();
                }
            }
        }
    // Start is called before the first frame update
    void Start()
    {
        CineDebut = GameObject.Find("Cinématique_Début");
        CineMillieu = GameObject.Find("Cinématique_Millieu");
        CineFin = GameObject.Find("Cinématique_Fin");
        CineDebut.SetActive(true);
        CineMillieu.SetActive(false);
        CineFin.SetActive(false);
        dialogue1 = gameObject.GetComponent<DialogueTrigger>().dialogue;
        DialogueManager.instance.StartDialogue(dialogue1);
    }

    // Update is called once per frame
    void Update()
    {
        isTalk =  DialogueManager.instance.isFinished;
    }
}
