using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MiniCam : MonoBehaviour
{

    private GameObject Player;
    public Vector2 ThePos;
    private float PosZ;

    private GameObject MiniMap;
    private GameObject MiniMap1;

    private InputActions farmerInputActions;
    public InputAction movement;
    public bool MiniCamActive;
    // Start is called before the first frame update

    void Awake()
    {
        farmerInputActions = new InputActions();
    }
    void Start()
    {
    Player = GameObject.Find("Player"); 
    MiniMap =  GameObject.Find("MiniMap"); 
    MiniMap1 =  GameObject.Find("MiniMap (1)"); 
    PosZ = -20f;
    }

     private IEnumerator SetCam()
        {
            yield return new WaitForSeconds(0.000001f);
        } 

     private void OnEnable()
        {
            movement = farmerInputActions.Player.Movement;
            movement.Enable();

            farmerInputActions.Player.MiniCam.performed += DoCam;
            farmerInputActions.Player.MiniCam.Enable();
        }

         private void DoCam(InputAction.CallbackContext obj)
        {
             if(MiniCamActive)
           {
           MiniCamActive = false;
           PosZ = -20f;
           MiniMap.transform.localScale = new Vector3(1,1,1);
           MiniMap1.transform.localScale = new Vector3(1,1,1);
           MiniMap.transform.position = new Vector3(1848f,72,0);
           MiniMap1.transform.position = new Vector3(1848,72,0);
           Debug.Log("ok");
           }
           else
           {
           MiniCamActive = true;
           PosZ = -40f;
           MiniMap.transform.localScale = new Vector3(3,3,1);
         //  MiniMap1.transform.localScale = new Vector3(3f,3f,1);
           MiniMap.transform.position = new Vector3(1735f,185f,0);
           MiniMap1.transform.position = new Vector3(1735f,185f,0);
           }
        }

    // Update is called once per frame
    void Update()
    {
        ThePos = Player.transform.localPosition;
        gameObject.transform.localPosition = new Vector3(ThePos.x,ThePos.y,PosZ);
    }
}
