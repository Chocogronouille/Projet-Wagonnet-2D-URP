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
    public GameObject PauseMenu;
    public GameObject SelectLevel;
    public GameObject EventSystem;
    public GameObject ButtonScene1;
  //  private Scene TheScene;
    private string theScene;
 //   public GameObject WinMenu;
    public bool isPaused;

    // InteractText
    private GameObject InteractText;
    private Animator InteractAnim;

    // Cassette Recup Text
    private GameObject CassetteText;
    private Animator CassetteAnim;

    public static GameManage instance;

    private void Awake()
    {
        // Interaction
        InteractText = GameObject.Find("InteractText");
        InteractAnim = InteractText.GetComponent<Animator>();

        // Cassette
        CassetteText = GameObject.Find("CassetteRecupText");
        CassetteAnim = CassetteText.GetComponent<Animator>();

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
    }
        public void InteractClose()
    {
        InteractAnim.SetBool("isOpen", false);
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
           isPaused = true;
        } 
            public void StartGame()
    {
        SceneManager.LoadScene("Maxime");
        PauseMenu.SetActive(false); 
        Time.timeScale = 1;
        isPaused = false;
    }
    public void Resume()
    {
        PauseMenu.SetActive(false); 
        Time.timeScale = 1;
        isPaused = false;
    }

    public void Restart()
    {
        SceneManager.LoadScene(theScene);
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
        isPaused = false;
    }

        public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
        PauseMenu.SetActive(false);
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
