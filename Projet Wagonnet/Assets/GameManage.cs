using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManage : MonoBehaviour
{
    private InputActions farmerInputActions;
    public InputAction MenuUI;
    public GameObject PauseMenu;
 //   public GameObject WinMenu;
    public bool isPaused;

    public static GameManage instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de GameManage dans la scène");
            return;
        }

        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        farmerInputActions = new InputActions();
    }

    // Update is called once per frame
    void Update()
    {
        if(isPaused)
        {
           Time.timeScale = 0;
        } 
    }

  /*  private void OnEnable()
        {
            farmerInputActions.Player.Menu.performed += DoMenuUI;
            farmerInputActions.Player.Menu.Enable();
        }
        private void DoMenuUI(InputAction.CallbackContext obj)
        {
            Debug.Log("OpenMenu");
           PauseMenu.SetActive(true);
           isPaused = true;
        } */
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
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
