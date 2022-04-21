using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameManage : MonoBehaviour
{
    private InputActions farmerInputActions;
    public InputAction movement;
    public GameObject SelectLevel;
    public GameObject EventSystem;
    public GameObject ButtonScene1;
  //  private Scene TheScene;
    public string theScene;
 //   public GameObject WinMenu;
    public bool isPaused;
    private GameObject player;

    // InteractText
    private GameObject InteractText;
    private Animator InteractAnim;

    // Cassette Recup Text
    public GameObject CassetteText;
    public Animator CassetteAnim;

    // UI Count
    [HideInInspector] 
    public GameObject CountText;
    public Animator CountAnim;

    // PauseMenuAnim
    public GameObject PauseMenu;
    private Animator PauseAnim;

    public static GameManage instance;

    private void Awake()
    {
        player = GameObject.Find("Player");
        // Interaction
        InteractText = GameObject.Find("InteractText");
        InteractAnim = InteractText.GetComponent<Animator>();

        // Cassette
        CassetteText = GameObject.Find("CassetteRecupText");
        CassetteAnim = CassetteText.GetComponent<Animator>();

        // Cassette Open 
        CountText = GameObject.Find("UI Count");
        CountAnim = CountText.GetComponent<Animator>();

        // PauseAnim
        PauseAnim = PauseMenu.GetComponent<Animator>();

            if (instance != null)
            {
              Destroy(gameObject);
              return;
            }
            instance = this;

        farmerInputActions = new InputActions();
    }
    // Start is called before the first frame update
    void Start()
    {
         //Cursor.lockState = CursorLockMode.Locked;
         Debug.Log("Awake:" + SceneManager.GetActiveScene().name);
     //  TheScene = SceneManager.GetActiveScene().name;
     theScene = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {

      if(isPaused)
        {
           Time.timeScale = 0;
        } 
    }

    // Interact
    public void InteractOpen()
    {
        InteractAnim.SetBool("isOpen", true);
        player.GetComponent<Cinemachine.PlayerInput>().isInteract = true;
    }
        public void InteractClose()
    {
        InteractAnim.SetBool("isOpen", false);
        player.GetComponent<Cinemachine.PlayerInput>().isInteract = false;
    }

        // Cassette
    public void CassetteOpen()
    {
        CassetteAnim.SetBool("isOpen", true);
        StartCoroutine(IsOpenFalse());
    }
IEnumerator IsOpenFalse()
    {
        yield return new WaitForSeconds(1.1f);
        CassetteAnim.SetBool("isOpen", false);
    }

    private void OnEnable()
        {
            movement = farmerInputActions.Player.Movement;
            movement.Enable();

            farmerInputActions.Player.Menu.performed += DoMenuUI;
            farmerInputActions.Player.Menu.Enable();
        }
        private void DoMenuUI(InputAction.CallbackContext obj)
        {
           Debug.Log("OpenMenu");
           PauseMenu.SetActive(true);
           PauseAnim.SetBool("isPaused", true);
           CountAnim.SetBool("isPaused", true);
           isPaused = true;
           player.GetComponent<Cinemachine.PlayerInput>().isInteract = true;
        } 

            public void StartGame()
    {
        SceneManager.LoadScene("Maxime");
        PauseMenu.SetActive(false); 
        Time.timeScale = 1;
        isPaused = false;
        player.GetComponent<Cinemachine.PlayerInput>().isInteract = false;
    }
    public void Resume()
    {
        CountAnim.SetBool("isPaused", false);
        PauseMenu.SetActive(false); 
        Time.timeScale = 1;
        isPaused = false;
        player.GetComponent<Cinemachine.PlayerInput>().isInteract = false;
    }

    public void Restart()
    {
        SceneManager.LoadScene(theScene);
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
        isPaused = false;
        player.GetComponent<Cinemachine.PlayerInput>().isInteract = false;
    }

        public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
        PauseMenu.SetActive(false);
        player.GetComponent<Cinemachine.PlayerInput>().isInteract = false;
    }
    public void LoadSelectLevel()
    {
     //   EventSystem.GetComponent<EventSystem>().firstSelectedGameObject =  ButtonScene1;
        SceneManager.LoadScene("SelectScene");
        Time.timeScale = 0;
        isPaused = true;
    }
     public void LoadScene1()
    {
        SceneManager.LoadScene("Maxime");
    //    PauseMenu.SetActive(false); 
        Time.timeScale = 1;
        isPaused = false;
    }
         public void LoadScene2()
    {
        SceneManager.LoadScene("LD YAZID");
    //    PauseMenu.SetActive(false); 
        Time.timeScale = 1;
        isPaused = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
