using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public KeyCode keyLeft;
    public KeyCode keyRight;
    public KeyCode keyUp;
    public KeyCode keyDown;
    public float walkSpeed;
    public float fastFallSpeed;
    public float jumpForce;
    private int _jumpBuffer;
    public float drag;
    [SerializeField] private int jumpBufferTime;
    [SerializeField] private Rigidbody2D rbCharacter;
    [SerializeField] private bool isAirborn;
    [SerializeField] private int coyoteTime;

    void Update()
    {
        
        if (_jumpBuffer != 0)               //Si la touche de saut a été enfoncée, on décompte les frames de jump buffer
        {
            _jumpBuffer -= 1;
            if (isAirborn == false)
            {
                Jump();                     //Si la touche de saut a été enfoncée dans les temps et que le personnage n'est pas en l'air, il saute
            }
        }

        if (Input.GetKey(keyLeft))      //Quand la touche de gauche est enfoncée, le personnage obtient une vitesse vers la gauche
        {
            EndDrag();
            StartMoveLeft();
        }

        if (Input.GetKeyUp(keyLeft))
        {
            if (isAirborn == false)
            {
                Drag();
            }
        }

        if (Input.GetKey(keyRight))     //Quand la touche de droite est enfoncée, le personnage obtient une vitesse vers la droite
        {
            EndDrag();
            StartMoveRight();
        }
        
        if (Input.GetKeyUp(keyRight))
        {
            if (isAirborn == false)
            {
                Drag();
            }
        }

        if (Input.GetKeyDown(keyUp))        //Quand la touche de saut est enfoncée, on commence a compter les frame de jump buffer
        {
            _jumpBuffer = jumpBufferTime;
        }
        
        if (Input.GetKey(keyDown))          //Quand la touche de chute est enfoncée, le personnage obtient une vitesse vers le bas
        {
            FastFall();
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Drag();
        isAirborn = false;                  //Quand le personnage atterit sur une plateforme, il n'est plus considéré en l'air
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        EndDrag();
        CoyoteTime();                       //Quand le personnage sort d'une plateforme, on lance la coroutine coyote time
    }

    void Jump()                             //Fonction de saut qui donne une impulsion vers le haut au personnage
    {
        isAirborn = true;                   //Le personnage est en l'air
        _jumpBuffer = 0;                    //On arrête le décompte des frames de jump buffer
        rbCharacter.AddForce(new Vector2(0,jumpForce),ForceMode2D.Impulse);
    }

    void StartMoveLeft()
    {
        rbCharacter.velocity = new Vector2(-walkSpeed, rbCharacter.velocity.y);
    }

    void StartMoveRight()
    {
        rbCharacter.velocity = new Vector2(walkSpeed, rbCharacter.velocity.y);
    }

    void FastFall()
    {
        rbCharacter.velocity = new Vector2(rbCharacter.velocity.x, -fastFallSpeed);
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
            StopCoroutine(CoyoteTime());
        }

        yield return null;
    }
    
    void Drag()
    {
        rbCharacter.drag = drag;
    }

    void EndDrag()
    {
        rbCharacter.drag = 0;
    }
}
