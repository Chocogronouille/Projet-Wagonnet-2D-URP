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
    [SerializeField] private int jumpBufferTime;
    [SerializeField] private int coyoteTime;
    
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
            if (isAirborn == false)
            {
                _jumpBuffer = jumpBufferTime;
            }
        }

        if (Input.GetAxis("Vertical") < -0.7f)
        {
            FastFall();
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
        isAirborn = false;
    }
    
    private void OnCollisionExit2D(Collision2D other)
    {
        CoyoteTime();                       //Quand le personnage sort d'une plateforme, on lance la coroutine coyote time
    }
    
    IEnumerator CoyoteTime()                //Coroutine du coyote time
    {
        if (coyoteTime!=0)                  //Si on vient de quitter une plateforme, on décompte les frames pour encore sauter
        {
            coyoteTime -= 1;
        }
        else                                //Quand les frames sont passées, le personnage est en l'air et on stoppe le décompte
        {
            isAirborn = true;
            Debug.Log(isAirborn);
            StopCoroutine(CoyoteTime());
        }

        yield return null;
    }
}
