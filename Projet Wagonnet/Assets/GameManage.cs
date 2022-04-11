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
 //   public GameObject WinMenu;
    public bool isPaused;

    public static GameManage instance;

    private void Awake()
    {
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
       
    }

    // Update is called once per frame
    void Update()
    {

      if(isPaused)
        {
           Time.timeScale = 0;
        } 
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
        SceneManager.LoadScene("LD YAZID");
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
        SceneManager.LoadScene("LD YAZID");
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
