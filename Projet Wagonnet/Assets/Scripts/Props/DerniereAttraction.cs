using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Cinemachine;
using PlayerInput = Cinemachine.PlayerInput;
using UnityEngine.SceneManagement;

//using UnityEditor.Experimental.GraphView;


public class DerniereAttraction : MonoBehaviour
{
    
    private InputActions farmerInputActions;
    public Animator animator;
    public Animator MyAnimator;
    private Text Timer;
    //public ParticleSystem Effects;
    //public string sceneName;
    //public Animator fadeSystem;


    [SerializeField] private GameObject player;
    public int currentAttractionCount;
   // public InteractBar interactBar;
    public bool isColliding;
    
    public CinemachineVirtualCamera CameraAttraction; //GroupCamera
    public CinemachineVirtualCamera CameraFin;

//    private InputActions farmerInputActions;
    public static DerniereAttraction instance;
    
     private void Awake()
     
    {
        if (instance != null)
            {
              Destroy(gameObject);
              return;
            }
            instance = this;
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
        Timer = GameObject.Find("Timer").GetComponent<Text>();
        Timer.GetComponent<Chronometre>().enabled = true;
        currentAttractionCount = 0;
        //interactBar.SetCount(currentAttractionCount);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManage.instance.InteractOpen();
            isColliding = true;
        }
    }
    private void DoPressB(InputAction.CallbackContext obj)
    {
        PressB();
    }
    
    private void PressB()                     
    {
        if (isColliding)
        {
            Timer.GetComponent<Chronometre>().enabled = false;
            MyAnimator.SetBool("isHappy",true);
         //   gameObject.transform.Translate(0,1.1f,0);
            //Effects.gameObject.SetActive(false);
            if(gameObject.name == "Sad_Rails")
            {
              gameObject.transform.Translate(0,-0.75f,0);
            }
            else if(gameObject.name == "Sad_Ballon")
            {
                gameObject.transform.Translate(0,1.1f,0);
            }

              else if(gameObject.name == "Lampe_Sad")
            {
            //    gameObject.transform.Translate(0,1.1f,0);
            Debug.Log("ok");
            }
            GameManage.instance.CountAnim.SetBool("isAttraCount", true);
            StartCoroutine(Activation());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isColliding = false;
        GameManage.instance.InteractClose();
    }

    IEnumerator Activation()
    {
        animator.SetFloat("Speed", 0);
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.GetComponent<PlayerInput>().enabled = false;
        CameraAttraction.Priority = 5;
        yield return new WaitForSeconds(1f);
        GameManage.instance.CountAnim.SetBool("isAttraCount", false);
        yield return new WaitForSeconds(0.4f);
        CounterAttraction.instance.AddCounterAttraction(1);
        currentAttractionCount = currentAttractionCount + 1;
        // interactBar.SetCount(currentAttractionCount);
   //     GetComponent<BoxCollider2D>().enabled = false;
        CameraAttraction.Priority = 0;
        yield return new WaitForSeconds(2f);
        GetComponent<BoxCollider2D>().enabled = false;
        player.GetComponent<PlayerInput>().enabled = true;
        CameraFin.Priority = 10;
        //StartCoroutine(loadNextScene());
        


    }
    // public IEnumerator loadNextScene()
    // {
    //     //fadeSystem.SetTrigger("FadeIn");
    //     yield return new WaitForSeconds(1f);
    //     SceneManager.LoadScene(sceneName);
    // }
}
