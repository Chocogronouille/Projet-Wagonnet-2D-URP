using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{

    private InputActions farmerInputActions;
    private InputAction movement;
    public Vector2 Direction;
    public float WalkSpeed;
    // Start is called before the first frame update
    void Awake()
    {
       farmerInputActions = new InputActions();    
    }

    private void OnEnable()
    {
       movement = farmerInputActions.Player.Movement;
       movement.Enable();

       farmerInputActions.Player.Jump.performed += DoJump;
       farmerInputActions.Player.Jump.Enable();
    }

    private void DoJump(InputAction.CallbackContext obj)
    {
      Debug.Log("Jump!!");
    }

 /*   private void FixedUpdate()
    {
       Debug.Log("Movement Values " + movement.ReadValue<Vector2>());
    } */

    // Update is called once per frame
    void Update()
    {
       Direction = movement.ReadValue<Vector2>();
       gameObject.transform.Translate(Direction.x * WalkSpeed,Direction.y * WalkSpeed,0);  
    }
}
