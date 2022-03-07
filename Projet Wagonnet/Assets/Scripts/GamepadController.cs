using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class GamepadController : MonoBehaviour
{
    public float walkSpeed;
    private int _jumpBuffer;
    
    [SerializeField] private Rigidbody2D rbCharacter;
    [SerializeField] private float jumpForce;
    [SerializeField] private float fastFallSpeed;
    [SerializeField] private bool isAirborn;
    [SerializeField] private bool coyoteFloat;
    [SerializeField] private int jumpBufferTime;
    [SerializeField] private float coyoteTime;

    void Update()
    {
        Move();
        
        if (_jumpBuffer != 0)               //Si la touche de saut a été enfoncée, on décompte les frames de jump buffer
        {
            _jumpBuffer -= 1;
            if (isAirborn == false)
            {
                Jump();                     //Si la touche de saut a été enfoncée dans les temps et que le personnage n'est pas en l'air, il saute
            }
        }
        
        if(Input.GetButtonDown("Jump"))
        {
            _jumpBuffer = jumpBufferTime;
        }

        if (Input.GetAxis("Vertical") < -0.7f)
        {
            FastFall();
        }

        if (coyoteFloat == false)
        {
            if (rbCharacter.velocity.y < 0)
            {
                if (isAirborn == false)
                {
                    Debug.Log("Tombe");
                    coyoteFloat = true;
                    StartCoroutine(CoyoteTime());
                }
            }
        }
    }

    void Move()
    {
        rbCharacter.velocity = new Vector2(walkSpeed * Input.GetAxis("Horizontal"), rbCharacter.velocity.y);
    }
    
    void Jump()
    {
        isAirborn = true;
        rbCharacter.AddForce(new Vector2(0,jumpForce),ForceMode2D.Impulse);
    }
    
    void FastFall()
    {
        rbCharacter.velocity = new Vector2(rbCharacter.velocity.x, -fastFallSpeed);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        StopCoroutine(CoyoteTime());
        isAirborn = false;
        coyoteFloat = false;
        Debug.Log("Landed");
    }

    private IEnumerator CoyoteTime()                //Coroutine du coyote time
    {
        Debug.Log("CoyoteTime");
        yield return new WaitForSeconds(coyoteTime);
        isAirborn = true;
        Debug.Log(isAirborn);
        StopCoroutine(CoyoteTime());
    }
}
