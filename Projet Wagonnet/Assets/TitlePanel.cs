using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TitlePanel : MonoBehaviour
{
    private InputActions farmerInputActions;
    public InputAction movement;

    public string TheActive;

        private void Awake()
    {
        farmerInputActions = new InputActions();
        TheActive = SceneManager.GetActiveScene().name;
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
            if(TheActive == "Ecran_Titre")
            {
              SceneManager.LoadScene("MainMenu");
              TheActive =  "MainMenu";
            }
        } 

    // Update is called once per frame
    void Update()
    {
        
    }
}
