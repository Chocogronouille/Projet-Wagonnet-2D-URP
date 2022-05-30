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
    [HideInInspector]
    public string theScene;
    [HideInInspector]
    public bool isPaused;
    private GameObject player;

    // InteractText
    private GameObject InteractText;
    private GameObject InteractTextA;
    private Animator InteractAnim;
    private Animator InteractAnimA;

    // Cassette Recup Text
    [HideInInspector]
    public GameObject CassetteText;
    [HideInInspector]
    public Animator CassetteAnim;

    // UI Count
    [HideInInspector] 
    public GameObject CountText;
    [HideInInspector]
    public Animator CountAnim;
    public float timer;

    // PauseMenuAnim
    public GameObject PauseMenu;
    public GameObject MainMenu;
    private Animator PauseAnim;
    public bool isSelectScene;

    // Settings Menu
    public GameObject SettingsMenu;

    public Toggle FullScreenToggle;
    public Toggle VibrationToggle;
    public Toggle SpeedRunToggle;

    public bool isSpeedRun;

    public GameObject PauseFirstButton, OptionFirstButton, DialogueButton;

    public static GameManage instance;

    private void Awake()
    {
        player = GameObject.Find("Player");
        // Interaction
        InteractText = GameObject.Find("InteractText");
        InteractTextA = GameObject.Find("InteractTextA");
        InteractAnim = InteractText.GetComponent<Animator>();
        InteractAnimA = InteractTextA.GetComponent<Animator>();

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
    //    SettingsMenu.SetActive(false);
 //    SettingsMenu = GameObject.Find("SettingsPanel");
     //Cursor.lockState = CursorLockMode.Locked;
     Debug.Log("Awake:" + SceneManager.GetActiveScene().name);
     theScene = SceneManager.GetActiveScene().name;
     FullScreenToggle.isOn = (PlayerPrefs.GetInt("FullScreen", 1) == 1);
     VibrationToggle.isOn = (PlayerPrefs.GetInt("Vibration", 1) == 1);
     SpeedRunToggle.isOn = (PlayerPrefs.GetInt("SpeedRun", 1) == 1);
    }

    public void TheFullScreenToggle() 
    {
    // Get the current state of our toggle button.
    int enable = FullScreenToggle.isOn ? 1 : 0;
    // Set the PlayerPrefs equal to our current state.
    PlayerPrefs.SetInt("FullScreen", enable);
    } 
    public void TheVibrationToggle() 
    {
    int enable = VibrationToggle.isOn ? 1 : 0;
    PlayerPrefs.SetInt("Vibration", enable);
    } 
    public void TheSpeedRunToggle() 
    {
    int enable = SpeedRunToggle.isOn ? 1 : 0;
    PlayerPrefs.SetInt("SpeedRun", enable);
    } 

    // Update is called once per frame
    void Update()
    {
        // Condition vibration
        if(VibrationToggle.isOn = (PlayerPrefs.GetInt("Vibration", 1) == 1))
        {
            player.GetComponent<Cinemachine.PlayerInput>().isVibrate = true;
        }
        else
        {
           player.GetComponent<Cinemachine.PlayerInput>().isVibrate = false;
        }


        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
        if(timer <= 0)
        {
            CountAnim.SetBool("isTokenCount", false);
        }

      if(isPaused)
        {
           Time.timeScale = 0;
      //     player.GetComponent<Cinemachine.PlayerInput>().isInteract = true;
        } 
    }
    IEnumerator AllowMovement()
    {
        yield return new WaitForSeconds(0.1f);
        player.GetComponent<Cinemachine.PlayerInput>().isInteract = false;
    }

    // Interact
    public void InteractOpen()
    {
        InteractAnim.SetBool("isOpen", true);
      //  player.GetComponent<Cinemachine.PlayerInput>().isInteract = true;
    }
        public void InteractClose()
    {
        InteractAnim.SetBool("isOpen", false);
        player.GetComponent<Cinemachine.PlayerInput>().isInteract = false;
    }

// Interact A
     public void InteractOpenA()
    {
        InteractAnimA.SetBool("isOpen", true);
     //   player.GetComponent<Cinemachine.PlayerInput>().isInteract = true;
    }
        public void InteractCloseA()
    {
        InteractAnimA.SetBool("isOpen", false);
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

         farmerInputActions.Player.PressB.performed += DoPressB;
         farmerInputActions.Player.PressB.Enable();
        }
         private void DoPressB(InputAction.CallbackContext obj)
    {
        PressB();
    }

         private void PressB()                     
    {
         if(SettingsMenu.active)
        {
            SettingsMenu.SetActive(false);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(PauseFirstButton);
        }
        else if(PauseMenu.active)
        {
        CountAnim.SetBool("isPaused", false);
        PauseMenu.SetActive(false); 
        Time.timeScale = 1;
        isPaused = false;
    //    player.GetComponent<Cinemachine.PlayerInput>().isInteract = false;
    StartCoroutine(AllowMovement());
        }
        else if(isSelectScene)
        {
        SceneManager.LoadScene("MainMenu");
  //      player.GetComponent<Cinemachine.PlayerInput>().isInteract = false;
  StartCoroutine(AllowMovement());
        isSelectScene = false;
        }
        Debug.Log("Press BB");
    }
        private void DoMenuUI(InputAction.CallbackContext obj)
        {
           EventSystem.current.SetSelectedGameObject(null);
           EventSystem.current.SetSelectedGameObject(PauseFirstButton);
           PauseMenu.SetActive(true);
           PauseAnim.SetBool("isPaused", true);
           CountAnim.SetBool("isPaused", true);
           isPaused = true;
           player.GetComponent<Cinemachine.PlayerInput>().isInteract = true;
        } 

            public void StartGame()
    {
        SceneManager.LoadScene("LD TUTO");
        PauseMenu.SetActive(false); 
        Time.timeScale = 1;
        isPaused = false;
   //     player.GetComponent<Cinemachine.PlayerInput>().isInteract = false;
   StartCoroutine(AllowMovement());
    }
    public void Resume()
    {
        CountAnim.SetBool("isPaused", false);
        PauseMenu.SetActive(false); 
        Time.timeScale = 1;
        isPaused = false;
    //    player.GetComponent<Cinemachine.PlayerInput>().isInteract = false;
    StartCoroutine(AllowMovement());
    }

    public void Restart()
    {
        SceneManager.LoadScene(theScene);
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
        isPaused = false;
  //      player.GetComponent<Cinemachine.PlayerInput>().isInteract = false;
  StartCoroutine(AllowMovement());
    }

        public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
        PauseMenu.SetActive(false);
   //     player.GetComponent<Cinemachine.PlayerInput>().isInteract = false;
   StartCoroutine(AllowMovement());
    }
    public void LoadSelectLevel()
    {
        SceneManager.LoadScene("SelectScene");
        Time.timeScale = 0;
        isPaused = true;
        isSelectScene = true;
    }
        public void LoadScene1()
    {
        SceneManager.LoadScene("LD TUTO");
        Time.timeScale = 1;
        isPaused = false;
    }
         public void LoadScene2()
    {
        SceneManager.LoadScene("LD BALLON");
        Time.timeScale = 1;
        isPaused = false;
    }
        public void LoadScene3()
    {
        SceneManager.LoadScene("LD RAIL");
        Time.timeScale = 1;
        isPaused = false;
    }

    public void OpenSettings()
    {
       SettingsMenu.SetActive(true);
       EventSystem.current.SetSelectedGameObject(null);
       EventSystem.current.SetSelectedGameObject(OptionFirstButton);
    }
        public void CloseSettings()
    {
       SettingsMenu.SetActive(false);
       EventSystem.current.SetSelectedGameObject(null);
       EventSystem.current.SetSelectedGameObject(PauseFirstButton);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
