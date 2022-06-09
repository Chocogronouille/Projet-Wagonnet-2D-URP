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


public class Attraction : MonoBehaviour
{
    
    private InputActions farmerInputActions;
    public Animator animator;
    public Animator MyAnimator;
    public ParticleSystem Effects;
    public ParticleSystem BigCheckPoint;
    public string sceneName;
    public Animator fadeSystem;
    private bool _isActivated;
    private Text Timer;
    private bool isInteract1;
    private bool isReact;
    public float CamAnim;
    public float AnimDialogue;
    public AudioClip sound;





    [SerializeField] private GameObject player;
    public int currentAttractionCount;
   // public InteractBar interactBar;
    public bool isColliding;

    public CinemachineVirtualCamera CameraAttraction; //GroupCamera

    
    
    
//    private InputActions farmerInputActions;
     public static Attraction instance;
    
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
         
          farmerInputActions.Player.Jump.performed += DoReact;
          farmerInputActions.Player.Jump.Enable();
     }

         private void DoReact(InputAction.CallbackContext obj)
    {
        TheReact();
    }

    private void Start()
    {
        isReact = false;
        Timer = GameObject.Find("Timer").GetComponent<Text>();
        Timer.GetComponent<Chronometre>().isTiming = true;
        currentAttractionCount = 0;
        //interactBar.SetCount(currentAttractionCount);
    }
    void Update()
    {
        isInteract1 = player.GetComponent<Cinemachine.PlayerInput>().isInteract;
      if(isReact && !isInteract1)
      {
         StartCoroutine(loadNextScene());
      }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManage.instance.InteractOpenA();
            isColliding = true;

        }
    }
    private void DoPressB(InputAction.CallbackContext obj)
    {
    //    PressB();
    }
    
    private void TheReact()                     
    {
        if (isColliding)
        {
            player.GetComponent<Cinemachine.PlayerInput>().animator.SetBool("isHuging", true);
            Timer.GetComponent<Chronometre>().isTiming = false;
            Chronometre.instance.SaveTimer();
            if (_isActivated) return;
            _isActivated = true;
         //   MyAnimator.SetBool("isHappy",true);
         //   gameObject.transform.Translate(0,1.1f,0);
            Effects.gameObject.SetActive(false);
      //      BigCheckPoint.Play();
      /*      if(gameObject.name == "Sad_Rails")
            {
              gameObject.transform.Translate(0,-0.75f,0);
            }
            else if(gameObject.name == "Sad_Ballon")
            {
                gameObject.transform.Translate(0,1.1f,0);
            } */
         //   GameManage.instance.CountAnim.SetBool("isAttraCount", true);
            StartCoroutine(Activation());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        GameManage.instance.InteractCloseA();
        isColliding = false;
    }

    IEnumerator Activation()
    {
        gameObject.GetComponent<DialogueTrigger>().isOnAttraction = true;
   //     gameObject.GetComponent<DialogueTrigger>().TheDialogue();
        animator.SetFloat("Speed", 0);
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.GetComponent<PlayerInput>().enabled = false;
        CameraAttraction.Priority = 5;
        yield return new WaitForSeconds(CamAnim);
        MyAnimator.SetBool("isHappy",true);
        BigCheckPoint.Play();
        AudioManager.instance.PlayClipAt(sound, transform.position);
        if(gameObject.name == "Sad_Rails")
            {
              gameObject.transform.Translate(0,-0.75f,0);
            }
            else if(gameObject.name == "Sad_Ballon")
            {
                gameObject.transform.Translate(0,1.1f,0);
            }
        gameObject.GetComponent<DialogueTrigger>().isOnAttraction = false;
    //    GameManage.instance.CountAnim.SetBool("isAttraCount", false);
    //    CounterAttraction.instance.AddCounterAttraction(1);
    //    currentAttractionCount = currentAttractionCount + 1;
        // interactBar.SetCount(currentAttractionCount);
      //  GetComponent<BoxCollider2D>().enabled = false;
        CameraAttraction.Priority = 0;
        yield return new WaitForSeconds(AnimDialogue);
      //  GetComponent<BoxCollider2D>().enabled = false;
       // StartCoroutine(loadNextScene());
       isReact = true;
    }

    public IEnumerator loadNextScene()
    {
        isReact = false;
        GetComponent<BoxCollider2D>().enabled = false;
        fadeSystem.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneName);
        player.GetComponent<PlayerInput>().enabled = true;
    }
}
